using UnityEngine;

public class Building : MonoBehaviour
{
    public TorretaSettings torretaSettings;
    public int custoRec => torretaSettings.custoRec;
    public int custoComb => torretaSettings.custoComb;
    public Tile originTile;

    [SerializeField] private SpriteRenderer torretaSprite;
    public SpriteRenderer ArmaSprite => torretaSprite;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        /*custoComb = torretaSettings.custoComb;
        custoRec = torretaSettings.custoRec;*/

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
