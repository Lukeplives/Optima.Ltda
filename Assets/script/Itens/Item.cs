using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemSettings settings;
    private SpriteRenderer spriteRenderer;
    public float upGas;
    public int upFerro;
    
    public int upPeças;

    private ItemSettings.TipoItem tipo;
    private GameManager gameManager;
     private Submarino player;



    public ItemSettings.TipoItem recurso;

    public ObjectPool.PoolTag poolTag;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        recurso = settings.tipo;
        upGas = settings.upGas;
        upFerro = settings.upFerro;
        upPeças = settings.upPeças;
        tipo = settings.tipo;

        poolTag = settings.tagPool;
       
    }

    public void Initialize(ItemSettings newSettings)
    {
        settings = newSettings;

        if (spriteRenderer != null && settings.lootsprite != null)
        {
            spriteRenderer.sprite = settings.lootsprite;
        }
        player = FindFirstObjectByType<Submarino>();
        gameManager = FindFirstObjectByType<GameManager>();



    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Coletar()
    {
        switch (settings.tipo)
        {
            case ItemSettings.TipoItem.Ferro:

                gameManager.QtdFerro += upFerro;
                break;

            case ItemSettings.TipoItem.Gas:

                gameManager.QtdComb += upGas;
                break;
        }
        gameManager.uiManager.AtualizarRecursosHUD();
        ObjectPool.Instance.Despawn(gameObject);

    }
}
