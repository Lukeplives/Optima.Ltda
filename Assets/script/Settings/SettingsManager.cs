using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public float mouseSensitivity;
    public float volumeMaster;

    private void Awake()
    {
         if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetVolume(float value)
    {
        SettingsData.volume = value;
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("volume", value);
    }

    public void SetSensitivity(float value)
    {
        SettingsData.mouseSensitivity = value;
        PlayerPrefs.SetFloat("sensitivity", value);
    }

    public void LoadSettings()
    {
        //SettingsData.volume = PlayerPrefs.GetFloat("volume", 1f);
        //SettingsData.mouseSensitivity = PlayerPrefs.GetFloat("sensitivity", 1f);

        mouseSensitivity = PlayerPrefs.HasKey("mouseSensitivity")
            ? PlayerPrefs.GetFloat("mouseSensitivity")
            : 0.5f;

        volumeMaster = PlayerPrefs.HasKey("volumeMaster")
            ? PlayerPrefs.GetFloat("volumeMaster")
            : 0.5f;

        AudioListener.volume = SettingsData.volume;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
