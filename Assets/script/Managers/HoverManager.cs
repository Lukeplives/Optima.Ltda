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

    void Start()
    {
        canvasRect = toolTipCanva.GetComponent<RectTransform>();
        tooltipRect = toolTipObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (!toolTipObject.activeSelf)
            return;

        Vector2 mousePos = Input.mousePosition;
        Vector2 anchoredPos;

        // Converte para posição no canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, mousePos, toolTipCanva.renderMode == RenderMode.ScreenSpaceOverlay ? null : toolTipCanva.worldCamera, out anchoredPos);

        Vector2 tooltipSize = tooltipRect.sizeDelta;
        Vector2 canvasSize = canvasRect.sizeDelta;

        Vector2 finalOffset = toolTipOffset;

        //
        // ----------- DETECÇÃO DA BORDA DIREITA --------------
        //
        if (anchoredPos.x + tooltipSize.x + finalOffset.x > canvasSize.x / 2)
        {
            finalOffset.x = -Mathf.Abs(finalOffset.x); // inverte horizontal
            tooltipRect.pivot = new Vector2(1, tooltipRect.pivot.y);
        }
        else
        {
            finalOffset.x = Mathf.Abs(finalOffset.x);
            tooltipRect.pivot = new Vector2(0, tooltipRect.pivot.y);
        }

        //
        // ----------- DETECÇÃO DA BORDA SUPERIOR --------------
        //
        if (anchoredPos.y + tooltipSize.y + Mathf.Abs(finalOffset.y) > canvasSize.y / 2)
        {
            finalOffset.y = -Mathf.Abs(finalOffset.y); // vira para baixo
            tooltipRect.pivot = new Vector2(tooltipRect.pivot.x, 1);
        }
        else
        {
            finalOffset.y = Mathf.Abs(finalOffset.y); // padrão para cima
            tooltipRect.pivot = new Vector2(tooltipRect.pivot.x, 0);
        }

        // Aplica offset após ajustar pivô
        anchoredPos += finalOffset;

        // Atualiza posição final
        tooltipRect.anchoredPosition = anchoredPos;
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
