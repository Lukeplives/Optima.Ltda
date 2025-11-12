using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    public string message;
    public bool usarStatusTorreta = false;


    void Awake()
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {

        GameManager.Instance.hoverManager.ShowToolTip(message);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.hoverManager.HideToolTip();
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
