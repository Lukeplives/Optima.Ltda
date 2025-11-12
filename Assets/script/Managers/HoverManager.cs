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

            anchoredPos += toolTipOffset;

            tooltipRect.anchoredPosition = anchoredPos;

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
