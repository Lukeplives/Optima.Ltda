using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [Header("Referencias")]
    public WaveSpawner waveSpawner;
    public BossController bigBoss;
    public RadarWave radarWaveUi;
    [SerializeField] private GameObject botaoModoTiro;
    public Slider progressSlider;

    [Header("Configs")]
    public float timeBtwWaves;
    public bool wavesComecaram;
    private int currentWave = 0;
    private int totalEtapas;
    public int etapasCompletas;


    public event Action AllWavesCompleted;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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


            StartCoroutine(StartWaves());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
    {
        Debug.Log("CHEAT: Pulando waves e iniciando BOSS");

        StartCoroutine(CheatIrParaBoss());
    }
    }

    IEnumerator StartWaves()
    {
        wavesComecaram = true;
        for (int i = 0; i < waveSpawner.waves.Length; i++)
        {
            radarWaveUi?.AtualizarRadar(waveSpawner.waves[i]);
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


        bigBoss?.StartBossFight();

        
        Animator botaoAnimador = botaoModoTiro.GetComponent<Animator>();
        ToolTipTrigger botaoToolTip = botaoModoTiro.GetComponent<ToolTipTrigger>();
        Button botaoComponent = botaoModoTiro.GetComponent<Button>();
        botaoComponent.interactable = true;
        botaoToolTip.enabled = true;
        botaoAnimador.SetTrigger("ModoTiro");
        
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

        GameManager.Instance.uiManager.MostrarWin(true);
        Time.timeScale = 0f;
    }

    private void AtualizarSlider()
    {
        if (progressSlider == null) return;
        float progresso = (float)etapasCompletas / totalEtapas;
        progressSlider.value = progresso;
    }
    
    public bool TodasWavesConcluidas()
    {
        return wavesComecaram && currentWave >= waveSpawner.waves.Length;
    }

    IEnumerator CheatIrParaBoss()
    {
        var inimigos = FindObjectsOfType<Inimigo>();
        var inimigosKamikaze = FindObjectsOfType<InimigoKamikaze>();
        foreach(var i in inimigos)
        {
            Destroy(i.gameObject);
        }

        foreach(var k in inimigosKamikaze)
        {
            Destroy(k.gameObject);
        }

        

        etapasCompletas = waveSpawner.waves.Length;
        
        AtualizarSlider();

        StopAllCoroutines();

        yield return null;

        Debug.Log("CHEAT: Boss iniciado");

        AllWavesCompleted?.Invoke();
        bigBoss.StartBossFight();

        botaoModoTiro.SetActive(true);
    }
}
