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




    [Header("Configurações")]
    public KeyCode pauseKey = KeyCode.Escape;
    public bool isPaused = false;
    public Slider volumeSlider;
    public Slider sensitivitySlider;
    
    
    void Start()
    {
        AtualizarRecursosHUD();
        MostrarWin(false);
        MostrarPause(false);
        MostrarGameOver(false);
        sensitivitySlider.value = SettingsManager.Instance.mouseSensitivity;
        volumeSlider.value = SettingsManager.Instance.volumeMaster;

        volumeSlider.onValueChanged.AddListener(SettingsManager.Instance.SetVolume);
        sensitivitySlider.onValueChanged.AddListener(SettingsManager.Instance.SetSensitivity);


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
