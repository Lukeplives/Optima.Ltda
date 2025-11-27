using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    [Header("Audio Mixer")]
    public AudioMixer mixer;

    [Header("Volumes")]
    public float masterVolume = 0.5f;
    public float cutsceneVolume = 0.5f;
    public float bgmVolume = 0.5f;



    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        PlayerPrefs.DeleteAll();

        CarregarConfigs();
        AplicarTodasConfigs();
    }

    void Start()
    {
        Debug.Log($"{masterVolume}, {cutsceneVolume}, {bgmVolume}");
    }


    // ===== VOLUME =====

    public void SetMasterVolume(float v)
    {

        masterVolume = v;
        float volume = Mathf.Log10(Mathf.Clamp(v, 0.0001f, 1f)) * 20;
        mixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", v);
    }

    public void SetCutsceneVolume(float v)
    {

        cutsceneVolume = v;
        float volume = Mathf.Log10(Mathf.Clamp(v, 0.0001f, 1f)) * 20;
        mixer.SetFloat("CutsceneVolume", volume);
        PlayerPrefs.SetFloat("CutsceneVolume", v);
    }

    public void SetBGMVolume(float v)
    {

        bgmVolume = v;
        float volume = Mathf.Log10(Mathf.Clamp(v, 0.0001f, 1f)) * 20;
        mixer.SetFloat("BGMVolume", volume);
        PlayerPrefs.SetFloat("BGMVolume", v);
    }


    // ===== SALVAR/CARREGAR =====

    public void CarregarConfigs()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        cutsceneVolume = PlayerPrefs.GetFloat("CutsceneVolume", 0.5f);
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
    }

    public void AplicarTodasConfigs()
    {
        SetMasterVolume(masterVolume);
        SetCutsceneVolume(cutsceneVolume);
        SetBGMVolume(bgmVolume);

    }
}
