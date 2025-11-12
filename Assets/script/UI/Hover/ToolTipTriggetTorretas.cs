using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTriggetTorretas : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Prefab da torreta")]
    public GameObject torretaSpawnada;

    [TextArea] 
    public string mensagemExtra;

    private ITooltipInfo tooltipInfo;
        void Start()
    {
        if (torretaSpawnada != null)
            tooltipInfo = torretaSpawnada.GetComponent<ITooltipInfo>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
         string textoFinal = "";

        if (tooltipInfo != null)
            textoFinal = tooltipInfo.GetTooltipText();

        if (!string.IsNullOrEmpty(mensagemExtra))
            textoFinal += "\n" + mensagemExtra;

        GameManager.Instance.hoverManager.ShowToolTip(textoFinal);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.hoverManager.HideToolTip();
    }


}
