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
        if (!controleAtivo) { return; }
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if(hit.collider != null)
            {
                TorretaBasica torreta = hit.collider.GetComponent<TorretaBasica>();
                if(torreta != null && torreta.munInfinita)
                {
                    torreta.AlternarControleManual();
                }
            }
        }
    }
}
