using System.Collections.Generic;
using UnityEngine;


public class BuildManager : MonoBehaviour
{
    [Header("Referências")]
    public Transform submarino;
    public GameObject grid;
    public CustomCursor customCursor;
    public Tile[] tiles;

    private Building buildingToPlace;

    public enum TipoTorreta
    {
        TorretaMetralhadora,
        TorretaSniper,
        TorretaExplosiva,
        TorretaLancaChama
    }

    private Dictionary<TipoTorreta, ObjectPool.PoolTag> mapaTorretaParaPool;
    void Awake()
    {
        mapaTorretaParaPool = new Dictionary<TipoTorreta, ObjectPool.PoolTag>()
        {
            {TipoTorreta.TorretaMetralhadora , ObjectPool.PoolTag.TorretaMetralhadora},
            {TipoTorreta.TorretaSniper, ObjectPool.PoolTag.TorretaSniper },
            {TipoTorreta.TorretaExplosiva, ObjectPool.PoolTag.TorretaExplosiva},
            {TipoTorreta.TorretaLancaChama, ObjectPool.PoolTag.TorretaLancaChama},
        };
    }
    /*void Update()
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
    }*/

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && buildingToPlace != null)
        {
            ConstruirTorreta();
        }
    }

    public void ConstruirTorreta()
    {
        Tile nearestTile = null;
        float shortestDistance = float.MaxValue;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        foreach (Tile tile in tiles)
        {
            float dist = Vector2.Distance(tile.transform.position, mouseWorldPos);
            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                nearestTile = tile;
            }
        }

        if (nearestTile == null || nearestTile.isOccupied) return;

        if(buildingToPlace == null)
        {
            Debug.LogWarning("nenhuma torreta selecionada");
            return;
        }

        TipoTorreta tipo = buildingToPlace.tagTorreta;
        if (!mapaTorretaParaPool.TryGetValue(tipo, out ObjectPool.PoolTag poolTag))
        {
        Debug.LogWarning($"Nenhum pool mapeado para tipo {tipo}");
        return;
        }

        GameObject novaTorreta = ObjectPool.Instance.SpawnFromPool(poolTag, nearestTile.transform.position, Quaternion.identity);

        if (novaTorreta != null)
        {
            novaTorreta.transform.SetParent(submarino);
        }

        Building scriptTorreta = novaTorreta.GetComponent<Building>();
        if (scriptTorreta != null)
        {
            scriptTorreta.originTile = nearestTile;
        }

        var torretaComponent = novaTorreta.GetComponent<TorretaBasica>();
        if (torretaComponent != null)
        {
           
            torretaComponent.enabled = true;
            torretaComponent.munAtual = torretaComponent.munMax;
            
        }

        nearestTile.isOccupied = true;
        buildingToPlace = null;

        grid.SetActive(false);
        customCursor.gameObject.SetActive(false);
        Cursor.visible = true;

        GameManager.Instance.NotificarMudançaTile();
    }

    public void SelecionarTorreta(Building torretaPrefab)
    {
        buildingToPlace = torretaPrefab;
        grid.SetActive(true);
        customCursor.AtivarCursor(torretaPrefab.ArmaSprite.sprite);
        Cursor.visible = false;
    }
    
    

    /*public void StartBuilding(Building building)
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
    }*/
}
