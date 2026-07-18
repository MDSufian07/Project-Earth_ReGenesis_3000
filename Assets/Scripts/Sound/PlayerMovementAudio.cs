using UnityEngine;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerMovementAudio : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource footstepSource;
        [SerializeField] private AudioSource jumpSource;

        [Header("Settings")]
        [SerializeField] private float walkPitch = 1f;
        [SerializeField] private float sprintPitch = 1.35f;
        [SerializeField] private float jumpFootstepDisableTime = 2f;

        private float footstepResumeTime;

        private void Update()
        {
            bool moving =
                InputManager.Instance.MoveInput.sqrMagnitude > 0.01f;

            bool sprinting =
                InputManager.Instance.SprintHeld && moving;

            if (InputManager.Instance.JumpPressed)
            {
                if (jumpSource != null)
                    jumpSource.Play();

                footstepResumeTime =
                    Time.time + jumpFootstepDisableTime;

                if (footstepSource != null &&
                    footstepSource.isPlaying)
                {
                    footstepSource.Stop();
                }
            }

            if (moving &&
                Time.time >= footstepResumeTime)
            {
                if (footstepSource != null)
                {
                    footstepSource.pitch =
                        sprinting ? sprintPitch : walkPitch;

                    if (!footstepSource.isPlaying)
                        footstepSource.Play();
                }
            }
            else
            {
                if (footstepSource != null &&
                    footstepSource.isPlaying)
                {
                    footstepSource.Stop();
                }
            }
        }
    }
}