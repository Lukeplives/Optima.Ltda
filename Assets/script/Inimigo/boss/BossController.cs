using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Partes do Boss")]
    public TentaculoSpawner tentaEsquerdo;
    public TentaculoSpawner tentaDireito;
    public BossWeakPoint pontoFraco;

    [Header("Configs")]
    public int maxHP = 1500;
    public int atualHP;
    public int fase = 0;

    [Header("Movimentação")]
    public float alturaFixaAcimaDoPlayer = 5f; 
    public float velocidade = 5f;

    private FeedbackDamage feedbackDamage;


    [Header("Configs PEM")]
    [SerializeField] float tempoPEMAtivado;
    public float tempoDeCarga;
    public int danoParaCancelar;
    private int danoDuranteCarga;
    public float tempoRecarga;
    private bool golpeCarregando = false;
    private bool golpeCancelado = false;

    
    [Header("PLayer")]
    public ControleManual controleManualTorreta;
    private Submarino player;


    private Coroutine pemCoroutine;
    public event Action OnBossDefeated;

    void Start()
    {
        if(danoParaCancelar <= 0)
        {
            danoParaCancelar = 20;
            Debug.Log("O burro esqueceu de setar o dano no inspetor cabeçao");
        }
    }
    void Awake()
    {
        atualHP = maxHP;
        player = FindFirstObjectByType<Submarino>();
        feedbackDamage = GetComponent<FeedbackDamage>();

    }
    void Update()
    {
        /*if(player != null)
        {
            transform.position = player.transform.position;
        }*/

         if (player == null) return;
    float targetX = player.transform.position.x;
    float targetY = player.transform.position.y + alturaFixaAcimaDoPlayer;
    float targetZ = transform.position.z;

    Vector3 targetPos = new Vector3(targetX, targetY, targetZ);

    transform.position = Vector3.MoveTowards(
        transform.position,
        targetPos,
        velocidade * Time.deltaTime
    );
        
    }

    public void StartBossFight()
    {
        Debug.Log("boss fight iniciada");
        gameObject.SetActive(true);
        tentaDireito?.StartSpawning();
        tentaEsquerdo?.StartSpawning();      

        if(controleManualTorreta != null)
        {
            controleManualTorreta.AtivarControle(true);
        }  
    }

    public void TomaDano(int qtdDano)
    {
        atualHP -= qtdDano;
        feedbackDamage.Flash();
        if (atualHP <= 0)
        {
            atualHP = 0;
            Derrotado();
        }
        else
        {
            UpdateFase();
        }
    }

    void UpdateFase()
    {
        float porcentagem = (float)atualHP / maxHP;
        if (porcentagem <= 0.25) { fase = 1; OnEnterFase(1); }


    }

    void OnEnterFase(int faseAtual)
    {
        tentaEsquerdo.SetFase(faseAtual);
        tentaDireito.SetFase(faseAtual);

        if(pemCoroutine == null)
        {
            pemCoroutine = StartCoroutine(CicloPEM());
        }
    }

    void Derrotado()
    {
        Debug.Log("Boss Derrotado");
        if(pemCoroutine != null)
        {
            StopCoroutine(pemCoroutine);
        }
        OnBossDefeated?.Invoke();
        Destroy(gameObject);
        if(controleManualTorreta != null)
        {
            controleManualTorreta.AtivarControle(false);
        }
    }

    IEnumerator CicloPEM()
    {
        while (true)
        {
            yield return StartCoroutine(CarregarPem());
            if (!golpeCancelado)
            {
                yield return StartCoroutine(AtivarPem());
            }
            else
            {
                Debug.Log("Golpe Cancelado!");
            }

            golpeCancelado = false;
            danoDuranteCarga = 0;
            Debug.Log("Boss recarregando o PEM");
            yield return new WaitForSeconds(tempoDeCarga);
        }
    }

    private IEnumerator CarregarPem()
    {
        golpeCarregando = true;
        danoDuranteCarga = 0;
        Debug.Log("Boss Carregando o PEM");

        float tempo = 0f;
        while (tempo < tempoDeCarga)
        {
            if (danoDuranteCarga >= danoParaCancelar)
            {
                golpeCancelado = true;
                golpeCarregando = false;
                yield break;
            }
            tempo += Time.deltaTime;
            yield return null;
        }

        golpeCarregando = false;
    }


    private IEnumerator AtivarPem()
    {
        Debug.Log("PEM ativado");

        var torretasTotais = new List<TorretaBasica>(TorretaBasica.TodasTorretas);

        foreach (var torretasDesliga in TorretaBasica.TodasTorretas)
        {
            if (torretasDesliga == null) continue;
            if (!torretasDesliga.munInfinita)
            {
                torretasDesliga.DesativarTorretaPEM();
            }
        }

        var torretasChama = new List<Torretalançachama>(Torretalançachama.TodasTorretasChama);

        foreach (var chamaDesliga in Torretalançachama.TodasTorretasChama)
        {
            if (chamaDesliga == null) continue;

            if(chamaDesliga != null)
            {
                chamaDesliga.DesativarTorretaPEM();
            }
        }
        
        yield return new WaitForSeconds(tempoPEMAtivado);

        torretasTotais = new List<TorretaBasica>(TorretaBasica.TodasTorretas);

        foreach (var torretasAtiva in TorretaBasica.TodasTorretas)
        {
            if (torretasAtiva == null) continue;
            torretasAtiva.ReativarTorreta();
        }

        torretasChama = new List<Torretalançachama>(Torretalançachama.TodasTorretasChama);
        foreach(var chamaAtiva in Torretalançachama.TodasTorretasChama)
        {
            if (chamaAtiva == null) continue;

            if(chamaAtiva != null)
            {
                chamaAtiva.ReativarTorreta();
            }
        }
        Debug.Log("Pem terminou, torretas reativadas");
    }
    
    public void DanoPontoFraco(int dano)
    {
        if(golpeCarregando)
        {
            danoDuranteCarga += dano;
        }
    }
}
