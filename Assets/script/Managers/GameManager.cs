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
    public Submarino submarinoData;
    
    [Header("UI/Dados")]

    public int QtdFerro;
    public float QtdComb;
    [SerializeField] private float decrementoComb;
    public bool gameover;
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

    public event Action OnTileStateChanged;

    [Header("SubManagers")]
    public BuildManager buildManager;
    public UIManager uiManager;


    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }
    void Start()
    {
        
        if (SceneManager.GetActiveScene().name == "MainScene")
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
        if(Input.GetKeyDown(KeyCode.F5))
        {
            QtdComb += 1000;
            QtdFerro += 1000;
            uiManager.AtualizarRecursosHUD();
        }
        if (submarinoData != null)
        {
            if (QtdComb >= 0)
            {
                QtdComb -= decrementoComb * Time.deltaTime;
            }
            else
            {
                QtdComb = 0;
            }

            uiManager.AtualizarRecursosHUD();

            if (QtdComb <= 0)
            {
                buildManager.customCursor.gameObject.SetActive(false);
                Cursor.visible = true;
                uiManager.ExibirGameOver();
                if (submarinoData != null)
                {
                    Destroy(submarinoData.gameObject);
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (uiManager.isPaused)
                {
                    uiManager.AlternarPause();
                }
                else
                {
                    uiManager.AlternarPause();
                }
            }

        }

        if (waveSpawner != null && currentWave >= waveSpawner.waves.Length)
        {
            Time.timeScale = 0f;
            uiManager.MostrarWin(true);
            buildManager.customCursor.gameObject.SetActive(false);
            Cursor.visible = true;
            if (submarinoData != null)
            {
                Destroy(submarinoData.gameObject);
            }

        }
        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (uiManager.isPaused)
                uiManager.AlternarPause();
            else
                uiManager.AlternarPause();
        }
    }

    public void BuyBuilding(Building building)
    {
        if (QtdFerro >= building.custoRec && QtdComb >= building.custoComb)
        {
            QtdFerro -= building.custoRec;
            QtdComb -= building.custoComb;
            buildManager.StartBuilding(building);
        }
    }

    public void PlayerDead(GameObject player)
    {
        if (gameover) return;
        gameover = true;
        DisablePlayerComponents(player);

        Time.timeScale = 0f;

        uiManager.MostrarGameOver(true);
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
        foreach (var torretas in TorretaBasica.TodasTorretas)
        {
            if (torretas == null) continue;
            if (torretas.munInfinita)
            {
                torretas.AlternarControleManual();
            }
        }
    }

    public void NotificarMudançaTile()
    {
        OnTileStateChanged?.Invoke();
    }

    public void DanoAoPlayer(float dano)
    {
        QtdComb -= dano;
        if (QtdComb <= 0)
        {
            QtdComb = 0;
            PlayerDead(submarinoData.gameObject);
        }
    }
    


}
