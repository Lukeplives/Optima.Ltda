using UnityEngine;

public class CutsceneAudioBinder : MonoBehaviour
{
    public AudioSource audioSrc;

    void Start()
    {
        if (SettingsManager.Instance != null && audioSrc != null)
        {
            audioSrc.volume = SettingsManager.Instance.volumeMaster;
        }
    }

    void Update()
    {
        if (SettingsManager.Instance != null && audioSrc != null)
        {
            audioSrc.volume = SettingsManager.Instance.volumeMaster;
        }
    }
}
