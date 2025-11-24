using UnityEngine;
using UnityEngine.EventSystems;

public class TooltIpTriggerUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    public string mensagemFixa;
    
     public bool usarGameManager = false;
    public bool usarUIManager = false;

    public string tipoRecurso;
    public bool sliderProgresso;

    public void OnPointerEnter(PointerEventData eventData)
    {
        string mensagem = mensagemFixa;
        if (usarGameManager)
        {
            if (tipoRecurso == "ferro")
                mensagem = $"<b>Ferro:</b> {GameManager.Instance.QtdFerro}\nUsado para construir torretas";
            else if (tipoRecurso == "combustivel")
                mensagem = $"<b>Combustível:</b> {GameManager.Instance.QtdComb:N0}\nUsado para construir torretas, caso acabe o submarino se torna inativo...";

        }
        if (usarUIManager && sliderProgresso)
        {
            float progresso = GameManager.Instance.waveManager.etapasCompletas;
            mensagem = $"<b>Onda Atual:</b> {progresso}";
        }
        GameManager.Instance.hoverManager.ShowToolTip(mensagem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.hoverManager.HideToolTip();
    }


}
