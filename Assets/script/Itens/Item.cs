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
       
    }

    public void Initialize(ItemSettings newSettings)
    {
        settings = newSettings;

        /*upGas = settings.upGas;
        upFerro = settings.upFerro;
        upPeças = settings.upPeças;
        tipo = settings.tipo;*/

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

     void OnMouseDown()
    {

        switch(settings.tipo)
        {
            case ItemSettings.TipoItem.Ferro:
                Debug.Log("Coletou ferro");
                gameManager.QtdFerro += upFerro;    
                Destroy(gameObject);
            break;

            case ItemSettings.TipoItem.Gas:
                Debug.Log("Coletou gasolina");
                gameManager.QtdComb += upGas;
                Destroy(gameObject);
            break;

            case ItemSettings.TipoItem.Peça:
                Debug.Log("Coletou peças");
                player.hp += 40;
                player.peçasNum++;
                Destroy(gameObject);
            break;
        }

    }
}
