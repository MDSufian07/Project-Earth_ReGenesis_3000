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
                Input.GetKey(KeyCode.W) ||
                Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.D);

            bool sprinting =
                (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && moving;

            // Jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (jumpSource != null)
                    jumpSource.Play();

                // Disable footsteps for 2 seconds
                footstepResumeTime = Time.time + jumpFootstepDisableTime;

                if (footstepSource != null && footstepSource.isPlaying)
                    footstepSource.Stop();
            }

            // Footstep
            if (moving && Time.time >= footstepResumeTime)
            {
                if (footstepSource != null)
                {
                    footstepSource.pitch = sprinting ? sprintPitch : walkPitch;

                    if (!footstepSource.isPlaying)
                        footstepSource.Play();
                }
            }
            else
            {
                if (footstepSource != null && footstepSource.isPlaying)
                    footstepSource.Stop();
            }
        }
    }
}