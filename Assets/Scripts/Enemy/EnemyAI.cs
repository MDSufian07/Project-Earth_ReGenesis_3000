using System.Collections;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    public class EnemyAI : MonoBehaviour
    {
        private static readonly int Walk = Animator.StringToHash("Walk");

        [Header("Target")]
        [SerializeField] private Transform player;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float rotateSpeed = 8f;
        [SerializeField] private float stopDistance = 1.3f;

        [Header("Gravity")]
        [SerializeField] private float gravity = -25f;

        [Header("Attack")]
        [SerializeField] private float damage = 10f;
        [SerializeField] private float attackCooldown = 1.5f;
        [SerializeField] private float attackRange = 1.5f;

        [Header("Audio")]
        [SerializeField] private AudioSource deathSound;
        [SerializeField] private AudioSource hitSound;

        private Animator animator;
        private CharacterController controller;

        private Vector3 velocity;

        private bool canAttack = true;
        private bool isDead;
        private bool isAttacking;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();

            animator.applyRootMotion = false;
        }

        private void Start()
        {
            if (player == null)
            {
                GameObject obj = GameObject.FindGameObjectWithTag("Player");

                if (obj != null)
                    player = obj.transform;
            }
        }

        private void Update()
        {
            if (isDead || player == null)
                return;

            ApplyGravity();

            Vector3 direction = player.position - transform.position;
            direction.y = 0;

            float distance = direction.magnitude;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotateSpeed * Time.deltaTime);
            }

            if (isAttacking)
            {
                animator.SetBool(Walk, false);
                return;
            }

            if (distance > stopDistance)
            {
                animator.SetBool(Walk, true);

                controller.Move(direction.normalized * moveSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool(Walk, false);

                if (canAttack)
                    StartCoroutine(AttackRoutine());
            }
        }

        private void ApplyGravity()
        {
            if (controller.isGrounded && velocity.y < 0)
                velocity.y = -2f;

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }

        private IEnumerator AttackRoutine()
        {
            canAttack = false;
            isAttacking = true;

            animator.SetTrigger("Attack");

            while (isAttacking)
                yield return null;

            yield return new WaitForSeconds(attackCooldown);

            canAttack = true;
        }

        // Animation Event
        public void AttackHit()
        {
            if (player == null || isDead)
                return;

            Vector3 dir = player.position - transform.position;
            dir.y = 0;

            if (dir.magnitude > attackRange)
                return;

            float angle = Vector3.Angle(transform.forward, dir);

            if (angle > 60f)
                return;

            if (hitSound != null)
                hitSound.Play();

            Health hp = player.GetComponent<Health>();

            if (hp != null)
                hp.TakeDamage(damage);
        }

        // Animation Event
        public void EndAttack()
        {
            isAttacking = false;
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
            isAttacking = false;

            if (deathSound != null)
                deathSound.Play();

            StopAllCoroutines();

            animator.SetBool(Walk, false);
            animator.ResetTrigger("Attack");
            animator.ResetTrigger("Damage");

            // Disable CharacterController immediately
            if (controller != null)
                controller.enabled = false;

            // Disable all colliders immediately
            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }

            animator.applyRootMotion = true;
            animator.SetTrigger("Death");

            enabled = false;

            // Destroy after 7 seconds
            Destroy(gameObject, 7f);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
#endif
    }
}