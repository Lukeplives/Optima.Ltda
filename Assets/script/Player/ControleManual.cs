using UnityEngine;

public class ControleManual : MonoBehaviour
{
    private Camera cam;
    private bool controleAtivo = false;

    void Awake()
    {
        cam = Camera.main;
    }

    public void AtivarControle(bool ativo)
    {
        controleAtivo = ativo;
    }

    void Update()
    {

        
    }
}
