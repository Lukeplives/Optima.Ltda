using TMPro;
using UnityEngine;

public class HoverManager : MonoBehaviour
{
    [Header("Tooltip")]
    [SerializeField] private GameObject toolTipObject;
    [SerializeField] private TextMeshProUGUI tooltipText;
    public Canvas toolTipCanva;
    public Vector2 toolTipOffset = new Vector2(15f, -15f); 

    public RectTransform canvasRect;
    public RectTransform tooltipRect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasRect = canvasRect.GetComponent<RectTransform>();
        tooltipRect = toolTipObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
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
            // Começa com o offset padrão
            Vector2 finalOffset = toolTipOffset;

            // Pega os tamanhos
            Vector2 tooltipSize = tooltipRect.sizeDelta;
            Vector2 canvasSize = canvasRect.sizeDelta;

            // Se estiver muito próximo da borda direita, inverte o lado
            if (anchoredPos.x + tooltipSize.x / 2 + finalOffset.x > canvasSize.x / 2)
                finalOffset.x *= -1; // Inverte horizontalmente

            // Se estiver muito próximo da borda inferior, empurra pra cima
            if (anchoredPos.y - tooltipSize.y / 2 + finalOffset.y < -canvasSize.y / 2)
                finalOffset.y = Mathf.Abs(finalOffset.y);

            // Aplica offset
            anchoredPos += finalOffset;

            // Clampa pra evitar sair da tela
            anchoredPos.x = Mathf.Clamp(anchoredPos.x, -canvasSize.x / 2 + tooltipSize.x / 2, canvasSize.x / 2 - tooltipSize.x / 2);
            anchoredPos.y = Mathf.Clamp(anchoredPos.y, -canvasSize.y / 2 + tooltipSize.y / 2, canvasSize.y / 2 - tooltipSize.y / 2);

            // Atualiza posição final
            tooltipRect.anchoredPosition = anchoredPos;


            /*anchoredPos += toolTipOffset;

            
            Vector2 tooltipSize = tooltipRect.sizeDelta;
            Vector2 canvasSize = canvasRect.sizeDelta;

            
            anchoredPos.x = Mathf.Clamp(anchoredPos.x, -canvasSize.x / 2 + tooltipSize.x / 2, canvasSize.x / 2 - tooltipSize.x / 2);
            anchoredPos.y = Mathf.Clamp(anchoredPos.y, -canvasSize.y / 2 + tooltipSize.y / 2, canvasSize.y / 2 - tooltipSize.y / 2);

            tooltipRect.anchoredPosition = anchoredPos;*/

        }
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
