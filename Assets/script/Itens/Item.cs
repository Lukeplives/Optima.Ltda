using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemSettings settings;
    public float upGas;
    public int upFerro;
    
    public int upPeças;
    private GameManager gameManager ;
     private Submarino player;

    public enum TipoItem
    {
        Ferro,
        Gas,
        Peça
    }

    public TipoItem recurso;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Submarino>();
        gameManager = FindFirstObjectByType<GameManager>();

        upGas = settings.upGas;
        upFerro = settings.upFerro;
        upPeças = settings.upPeças;



       
    }

    public void Initialize(ItemSettings newSettings)
    {
        settings = newSettings;

        upGas = settings.upGas;
        upFerro = settings.upFerro;
        upPeças = settings.upPeças;

        
        GetComponent<SpriteRenderer>().sprite = settings.lootsprite;

    }

    // Update is called once per frame
    void Update()
    {

    }

     void OnMouseDown()
    {
        Debug.Log("Clicou");
        switch(settings.tipo)
        {
            case ItemSettings.TipoItem.Ferro:
                gameManager.QtdFerro += upFerro;    
                Destroy(gameObject);
            break;

            case ItemSettings.TipoItem.Gas:
                gameManager.QtdComb += upGas;
                Destroy(gameObject);
            break;

            case ItemSettings.TipoItem.Peça:
                player.hp += 40;
                player.peçasNum++;
                Destroy(gameObject);
            break;
        }

    }
}
