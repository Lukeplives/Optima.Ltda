using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Submarino")]
    private Building buildingToPlace;
    public GameObject grid;
    public Transform submarino;
    public Submarino submarinoData;
    public CustomCursor customCursor;
    public Tile[] tiles;

    [Header("UI/Dados")]

    [SerializeField] TextMeshProUGUI numFerro, numHP, numComb;
    public int QtdFerro;
    public float QtdComb;
    [SerializeField] private float decrementoComb;
    public GameObject deathScreen;
    private bool gameover = false;
    public GameObject winScreen;
    private bool gameWin = false;

    [Header("Waves")]
    public WaveSpawner waveSpawner;
    public float timeBtwWaves;

    private int currentWave = 0;
    public event Action AllWavesCompleted;
    public BossController bigBoss;

    public RadarWave radarWaveUi;

    [SerializeField] private GameObject botaoModoTiro;


    [Header("Dados do caminho")]
    public Slider progressSlider;

    private int totalEtapas;
    private int etapasCompletas;


    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }
    void Start()
    {
        if (waveSpawner == null)
        {
            waveSpawner = FindFirstObjectByType<WaveSpawner>();
        }

        if (bigBoss == null)
        {
            bigBoss = FindFirstObjectByType<BossController>();
        }

        totalEtapas = waveSpawner.waves.Length + 1;
        progressSlider.minValue = 0f;
        progressSlider.maxValue = 1f;
        progressSlider.value = 0f;
        

        waveSpawner.OnWaveCompleted += OnWaveCompleta;
        bigBoss.OnBossDefeated += OnBossDerrotado;


        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            StartCoroutine(StartWaves());
            AllWavesCompleted += () =>
            {
                bigBoss.StartBossFight();
                botaoModoTiro.SetActive(true);
            }; 
            
        }

    }

    IEnumerator StartWaves()
    {
        

        for (int i = 0; i < waveSpawner.waves.Length; i++)
        {
            radarWaveUi.AtualizarRadar(waveSpawner.waves[i]);
            bool waveTerminou = false;
            Action waveHandler = () => waveTerminou = true;

            waveSpawner.OnWaveCompleted += waveHandler;
            waveSpawner.StartWave(i);

            yield return new WaitUntil(() => waveTerminou);
            waveSpawner.OnWaveCompleted -= waveHandler;

            yield return new WaitForSeconds(timeBtwWaves);
        }

        Debug.Log("Waves finalizadas, começando boss");
        AllWavesCompleted?.Invoke();
    }


    void Update()
    {
        if (submarinoData != null)
        {


            numComb.text = QtdComb.ToString("N0");
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
            if (QtdComb >= 0)
            {
                QtdComb -= decrementoComb * Time.deltaTime;
            }
            else
            {
                QtdComb = 0;
            }
            


            if (submarinoData.hp <= 0 || QtdComb <= 0)
            {
                submarinoData.hp = 0;
                customCursor.gameObject.SetActive(false);
                Cursor.visible = true;
                deathScreen.SetActive(true);
            if(submarinoData != null)
            {
                Destroy(submarinoData.gameObject); 
            }
            }

        }

        
        if(currentWave >= waveSpawner.waves.Length)
        {
            Time.timeScale = 0f;
            winScreen.SetActive(true);
            gameWin = true;  
            customCursor.gameObject.SetActive(false);
            Cursor.visible = true;
            if(submarinoData != null)
            {
                Destroy(submarinoData.gameObject); 
            }
            
        }


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

    public void PlayerDead(GameObject player)
    {
        if (gameover) return;
        gameover = true;
        DisablePlayerComponents(player);

        Time.timeScale = 0f;

        deathScreen.SetActive(true);
    }

    void DisablePlayerComponents(GameObject player)
    {
        MonoBehaviour[] scriptsPlayer = player.GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour script in scriptsPlayer)
        {
            script.enabled = false;
        }

        Collider2D[] collidersPlayer = player.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in collidersPlayer)
        {
            collider.enabled = false;
        }

        Rigidbody2D[] rbPlayer = player.GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D rb in rbPlayer)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        SpriteRenderer[] spritesPlayer = player.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprites in spritesPlayer)
        {
            sprites.enabled = false;
        }
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void OnWaveCompleta()
    {
        etapasCompletas++;
        AtualizarSlider();
    }

    private void OnBossDerrotado()
    {
        etapasCompletas++;
        AtualizarSlider();
        botaoModoTiro.SetActive(false);
    }

    private void AtualizarSlider()
    {
        float progresso = (float)etapasCompletas / totalEtapas;
        progressSlider.value = progresso;
    }

    public void AlternarModoTiro()
    {
        foreach(var torretas in TorretaBasica.TodasTorretas)
        {
            if (torretas == null) continue;
            if(torretas.munInfinita)
            {
                torretas.AlternarControleManual();
            }
        }
    }

}
