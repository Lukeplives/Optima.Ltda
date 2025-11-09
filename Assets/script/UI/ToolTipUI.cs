using TMPro;
using UnityEngine;

public class ToolTipUI : MonoBehaviour
{
    [SerializeField] private GameObject toolTipObject;
    [SerializeField] private TextMeshProUGUI tooltipText;

    void Awake()
    {
        toolTipObject.SetActive(true);
    }

    public void Show(string message, Vector3 position)
    {
        toolTipObject.SetActive(true);
        tooltipText.text = message;
        
        toolTipObject.transform.position = position;
    }

    public void Hide()
    {
        toolTipObject.SetActive(false);
    }

    void Update()
    {
        if(toolTipObject.activeSelf)
        {
            toolTipObject.transform.position = Input.mousePosition + new Vector3(10, -10, 0);
        }
    }
}
