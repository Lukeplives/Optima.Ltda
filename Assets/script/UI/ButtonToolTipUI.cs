using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonToolTipUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea] public string toolTipMessage = "Trocar modo de tiro";
    private ToolTipUI tooltip;

    void Start()
    {
        tooltip = FindFirstObjectByType<ToolTipUI>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(tooltip != null)
        {
            tooltip.Show(toolTipMessage, Input.mousePosition);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(tooltip != null)
        {
            tooltip.Hide();
        }
    }


}
