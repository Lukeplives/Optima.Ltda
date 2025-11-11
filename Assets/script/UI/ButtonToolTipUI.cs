using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonToolTipUI : MonoBehaviour
{
    public string message;
    void OnMouseEnter()
    {
        GameManager.Instance.uiManager.ShowToolTip(message);
    }
    void OnMouseExit()
    {
        GameManager.Instance.uiManager.HideToolTip();
    }


}
