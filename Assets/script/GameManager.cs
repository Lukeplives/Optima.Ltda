using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections;
public class GameManager : MonoBehaviour
{
    [Header("Submarino")]
    private Building buildingToPlace;
    public GameObject grid;
    public Transform submarino, bordas;
    public Submarino submarinoData;
    public CustomCursor customCursor;
    public Tile[] tiles;

    [Header("UI/Dados")]

    [SerializeField] TextMeshProUGUI numFerro, numHP, numComb;
    public int QtdFerro;
    public float QtdComb;
    [SerializeField] private float decrementoComb;
    
    public WaveSpawner waveSpawner;
    public float timeBtwWaves;

    private int currentWave = 0;

    [Header("Dados do caminho")]
    public Transform startPoint;
    public Transform endPoint;
    public Slider progressSlider;

    private float totalDistance;


    void Start()
    {
        totalDistance = Vector3.Distance(startPoint.position, endPoint.position);
        progressSlider.minValue = 0;
        progressSlider.maxValue = totalDistance;

        StartCoroutine(StartWaves());
    }

    IEnumerator StartWaves()
    {
        while (currentWave < waveSpawner.waves.Length)
        {
            waveSpawner.StartWave(currentWave);
            yield return new WaitForSeconds(timeBtwWaves);
            currentWave++;
        }
    }


    void Update()
    {
        if (submarinoData != null)
        {
            bordas.position = new Vector2(submarino.position.x, 0);

            numComb.text = QtdComb.ToString();
            numFerro.text = QtdFerro.ToString();
            numHP.text = submarinoData.hp.ToString();

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
                    Building newTorreta = Instantiate(buildingToPlace, nearestTile.transform.position, quaternion.identity, submarino);
                    newTorreta.originTile = nearestTile;
                    buildingToPlace = null;
                    nearestTile.isOccupied = true;
                    customCursor.gameObject.SetActive(false);
                    grid.SetActive(false);
                    Cursor.visible = true;
                }


            }
            QtdComb -= decrementoComb * Time.deltaTime;


            if (submarinoData.hp < 0 || QtdComb == 0)
            {
                Destroy(submarinoData.gameObject);
            }

        }

        float distanceTraveled = Vector3.Distance(startPoint.position, submarino.position);

        progressSlider.value = distanceTraveled;
        
    }

    public void BuyBuilding(Building building)
    {
        if (QtdFerro >= building.custoRec && QtdComb >= building.custoComb)
        {
            customCursor.gameObject.SetActive(true);
            customCursor.GetComponent<SpriteRenderer>().sprite = building.GetComponent<SpriteRenderer>().sprite;
            Cursor.visible = false;


            QtdFerro -= building.custoRec;
            QtdComb -= building.custoComb;
            buildingToPlace = building;
            grid.SetActive(true);
        }
    }

 

 
}
