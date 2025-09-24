using UnityEngine;

public class Building : MonoBehaviour
{
    public TorretaSettings torretaSettings;
    public int custoRec;
    public int custoComb;
    public Tile originTile;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        custoComb = torretaSettings.custoComb;
        custoRec = torretaSettings.custoRec;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
