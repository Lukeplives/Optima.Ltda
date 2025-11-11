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

    [Header("Tooltip")]
    [SerializeField] private GameObject toolTipObject;
    [SerializeField] private TextMeshProUGUI tooltipText;
    public Canvas toolTipCanva;
    public Vector2 toolTipOffset = new Vector2(15f, -15f); 

    public RectTransform canvasRect;
    public RectTransform tooltipRect;


    [Header("Configurações")]
    public KeyCode pauseKey = KeyCode.Escape;
    

    public bool isPaused = false;
    
    
    void Start()
    {
        AtualizarRecursosHUD();
        MostrarWin(false);
        MostrarPause(false);
        MostrarGameOver(false);

        canvasRect = canvasRect.GetComponent<RectTransform>();
        tooltipRect = toolTipObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            AlternarPause();
        }



        if (toolTipObject.activeSelf)
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 anchoredPos;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            mousePos,
            null,
            out anchoredPos
            );
            
            anchoredPos += toolTipOffset;

            tooltipRect.anchoredPosition = anchoredPos;

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
    
        public void ShowToolTip(string message)
    {
        toolTipObject.SetActive(true);
        tooltipText.text = message;
        
        
    }

    public void HideToolTip()
    {
        toolTipObject.SetActive(false);
    }
}
