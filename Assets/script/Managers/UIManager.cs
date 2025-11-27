using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [Header("Referências de HUD")]
    public TextMeshProUGUI ferroText;
    public TextMeshProUGUI combustivelText;
    [Header("Sprite recursos")]
    public Image ferroIcone;
    public Image combustivelIcone;

    [Header("Sprites de Estado ferro")]
    public Sprite ferroNormal;
    public Sprite ferroBaixo;

    [Header("Sprites de Estado Combústivel")]

    public Sprite combNormal;
    public Sprite combBaixo;


    [Header("Painéis")]
    public GameObject winPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject optionsPanel;




    [Header("Configurações")]
    public KeyCode pauseKey = KeyCode.Escape;
    public bool isPaused = false;
    public Slider volumeMasterSlider;
    public Slider volumeBGMSlider;
    public Slider volumeCutsceneSlider;
    
    
    void Start()
    {
        AtualizarRecursosHUD();
        MostrarWin(false);
        MostrarPause(false);
        MostrarGameOver(false);

        // desativa eventos para evitar reset no início
        volumeMasterSlider.onValueChanged.RemoveAllListeners();
        volumeBGMSlider.onValueChanged.RemoveAllListeners();
        volumeCutsceneSlider.onValueChanged.RemoveAllListeners();

        // aplica o valor salvo
        volumeMasterSlider.value = SettingsManager.instance.masterVolume;
        volumeBGMSlider.value = SettingsManager.instance.bgmVolume;
        volumeCutsceneSlider.value = SettingsManager.instance.cutsceneVolume;

        // agora reassina os eventos
        volumeMasterSlider.onValueChanged.AddListener(SettingsManager.instance.SetMasterVolume);
        volumeBGMSlider.onValueChanged.AddListener(SettingsManager.instance.SetBGMVolume);
        volumeCutsceneSlider.onValueChanged.AddListener(SettingsManager.instance.SetCutsceneVolume);

        volumeMasterSlider.value = SettingsManager.instance.masterVolume;
        volumeBGMSlider.value = SettingsManager.instance.bgmVolume;
        volumeCutsceneSlider.value = SettingsManager.instance.cutsceneVolume;

        Debug.Log($"master {SettingsManager.instance.masterVolume}, bgm {SettingsManager.instance.bgmVolume}, cutscene {SettingsManager.instance.cutsceneVolume}");



    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            AlternarPause();
        }



       
        
    }

    public void AtualizarRecursosHUD()
    {
        ferroText.text = GameManager.Instance.QtdFerro.ToString();
        combustivelText.text = GameManager.Instance.QtdComb.ToString("N0");

    }

    public void AtualizarIndicadores(float ferro, float combustivel)
    {
        ferroIcone.sprite = ferro <= GameManager.Instance.ferroCritico ? ferroBaixo : ferroNormal;
        combustivelIcone.sprite = combustivel <= GameManager.Instance.combCritico ? combBaixo : combNormal;
    }

    public void MostrarWin(bool ativo)
    {
        winPanel.SetActive(ativo);
    }

    public void MostrarPause(bool ativo)
    {
        
        if(pausePanel.activeSelf)
        {
            optionsPanel.SetActive(ativo);
        }
        pausePanel.SetActive(ativo);

    }

    public void MostrarGameOver(bool ativo)
    {
        gameOverPanel.SetActive(ativo);
    }

    public void AlternarPause()
    {
        isPaused = !isPaused;

        MostrarPause(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        Cursor.visible = isPaused;
    }

    public void ExibirGameOver()
    {
        MostrarPause(false);
        MostrarWin(false);
        MostrarGameOver(true);

        Time.timeScale = 0f;
    }


    

}
