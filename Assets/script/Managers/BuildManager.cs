using UnityEngine;


public class BuildManager : MonoBehaviour
{
    [Header("Referências")]
    public Transform submarino;
    public GameObject grid;
    public CustomCursor customCursor;
    public Tile[] tiles;

    private Building buildingToPlace;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && buildingToPlace != null)
        {
            Tile nearestTile = null;
            float shortestDistance = float.MaxValue;
            foreach (Tile tile in tiles)
            {
                float dist = Vector2.Distance(tile.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (dist < shortestDistance)
                {
                    shortestDistance = dist;
                    nearestTile = tile;
                }
            }
            if (nearestTile.isOccupied == false)
            {
                Building newTorreta = Instantiate(buildingToPlace, nearestTile.transform.position, Quaternion.identity, submarino);
                newTorreta.originTile = nearestTile;
                buildingToPlace = null;
                nearestTile.SetOccupied(true);
                customCursor.gameObject.SetActive(false);
                grid.SetActive(false);
                Cursor.visible = true;
                GameManager.Instance.NotificarMudançaTile();
            }


        }
    }
    
    public void StartBuilding(Building building)
    {
        buildingToPlace = building;
        customCursor.gameObject.SetActive(true);

        SpriteRenderer armaSprite = building.ArmaSprite;
        if (armaSprite != null)
        {
            customCursor.GetComponent<SpriteRenderer>().sprite = armaSprite.sprite;
        }
        else
        {
            customCursor.GetComponent<SpriteRenderer>().sprite = building.GetComponent<SpriteRenderer>().sprite;
        }

        Cursor.visible = false;
        grid.SetActive(true);
    }
}
