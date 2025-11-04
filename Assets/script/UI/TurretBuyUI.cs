using UnityEngine;
using UnityEngine.UI;

public class TurretBuyUI : MonoBehaviour
{
    [Header("Referencias de OBJ")]
    public TorretaSettings torretaSettings;
    public GameManager gameManager;
    public Color corAtiva = Color.white;
    public Color corBloqueada = new Color(0.5f, 0.5f, 0.5f, 1);

    private Button botao;
    private Image imagem;

    void Start()
    {
        botao = GetComponent<Button>();
        imagem = GetComponent<Image>();

        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }

        gameManager.OnTileStateChanged += AtualizarEstado;

    }


    void Update()
    {
        AtualizarEstado();

    }

    void AtualizarEstado()
    {
        if (gameManager == null || torretaSettings == null) return;

        bool recursosSuficientes =
        gameManager.QtdFerro >= torretaSettings.custoRec &&
        gameManager.QtdComb >= torretaSettings.custoComb;

        bool existemTilesLivres = false;
        foreach(Tile tile in GameManager.Instance.buildManager.tiles)
        {
            if(!tile.isOccupied)
            {
                existemTilesLivres = true;
                break;
            }
        }

        botao.interactable = recursosSuficientes && existemTilesLivres;
        imagem.color = recursosSuficientes ? corAtiva : corBloqueada;
    }

    void OnDestroy()
    {
        if(gameManager != null)
        {
            gameManager.OnTileStateChanged -= AtualizarEstado;
        }
    }
}
