using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Video
{
    public class IntroVideoPlayer : MonoBehaviour
    {
        public VideoPlayer videoPlayer;
        public string nextScene = "01_MainMenu";

        private void Start()
        {
            videoPlayer.loopPointReached += EndReached;
        }

        private void EndReached(VideoPlayer vp)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}