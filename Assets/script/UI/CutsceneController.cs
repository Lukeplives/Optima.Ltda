using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutsceneController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nomeDoVideo = "CalmaCalabreso.mp4";
    public string nextScene = "MainScene";

    void Start()
    {
         string caminho = System.IO.Path.Combine(Application.streamingAssetsPath, nomeDoVideo);

        videoPlayer.url = caminho;


        videoPlayer.Play();
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextScene);
        
    }

    public void SkipCutscene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
