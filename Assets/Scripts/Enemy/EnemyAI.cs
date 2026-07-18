using System.Collections;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAI : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform player;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float rotateSpeed = 5f;
        [SerializeField] private float stopDistance = 1f;

        [Header("Attack")]
        [SerializeField] private float damage = 10f;
        [SerializeField] private float attackCooldown = 1.5f;
        [SerializeField] private float hitDelay = 0.4f;
        
        [SerializeField] private AudioSource deathSound;

        private Animator animator;

        private bool canAttack = true;
        private bool isDead;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            animator.applyRootMotion = false;
        }

        private void Start()
        {
            if (player == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

                if (playerObj != null)
                    player = playerObj.transform;
            }
        }

        private void Update()
        {
            if (isDead)
                return;

            if (player == null)
                return;

            Vector3 direction = player.position - transform.position;
            direction.y = 0;

            float distance = direction.magnitude;

            // Rotate
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotateSpeed * Time.deltaTime);
            }

            // Move
            if (distance > stopDistance)
            {
                transform.position += direction.normalized * moveSpeed * Time.deltaTime;
                animator.SetBool("Walk", true);
            }
            else
            {
                animator.SetBool("Walk", false);

                if (canAttack)
                    StartCoroutine(AttackRoutine());
            }
        }

        private IEnumerator AttackRoutine()
        {
            canAttack = false;

            animator.SetTrigger("Attack");

            yield return new WaitForSeconds(hitDelay);

            // Player Health প্রতি বার নতুন করে নেওয়া হচ্ছে
            if (player != null)
            {
                Health playerHealth = player.GetComponent<Health>();

                if (playerHealth != null)
                {
                    Debug.Log("Enemy Hit Player");
                    playerHealth.TakeDamage(damage);
                }
            }

            yield return new WaitForSeconds(attackCooldown);

            canAttack = true;
        }

        public void PlayDamageAnimation()
        {
            if (isDead)
                return;

            animator.SetTrigger("Damage");
        }

        public void Die()
        {
            if (isDead)
                return;

            isDead = true;

            deathSound.Play();

            StopAllCoroutines();
            enabled = false;

            animator.SetBool("Walk", false);
            animator.ResetTrigger("Attack");
            animator.ResetTrigger("Damage");

            animator.applyRootMotion = true;  

            animator.SetTrigger("Death");

            Destroy(gameObject, 7f);
        }
    }
}