using Invector.vCharacterController;
using Sound;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.InputSystem;

public class VideoController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject videoCanvas;
    [SerializeField] private PlayerMovementAudio playerInput;

    private void Start()
    {
        // Disable all player input
        if (playerInput != null)
            playerInput.enabled = false;

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
            videoPlayer.Play();
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        if (videoCanvas != null)
            videoCanvas.SetActive(false);

        // Enable player input
        if (playerInput != null)
            playerInput.enabled = true;
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnVideoFinished;
    }
}