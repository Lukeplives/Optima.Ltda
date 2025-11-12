using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonToolTipUI : MonoBehaviour
{
    public string message;
    void OnMouseEnter()
    {
        GameManager.Instance.hoverManager.ShowToolTip(message);
    }
    void OnMouseExit()
    {
        GameManager.Instance.hoverManager.HideToolTip();
    }


}
