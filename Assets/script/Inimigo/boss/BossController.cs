using System;
using System.Collections;
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

    void Awake()
    {
        atualHP = maxHP;
        player = FindFirstObjectByType<Submarino>();

    }
    void Update()
    {
        if(player != null)
        {
            transform.position = player.transform.position;
        }
        
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
        /*TorretaBasica[] torretas = FindObjectsOfType<TorretaBasica>();
        foreach (TorretaBasica torreta in torretas)
        {
            if (!torreta.munInfinita)
            {
                torreta.podeAtirar = false;
                Debug.Log("Pem ativado");
            }
        }

        yield return new WaitForSeconds(tempoPEMAtivado);

        foreach (TorretaBasica torreta in torretas)
        {
            if (!torreta.munInfinita)
            {
                torreta.podeAtirar = true;

            }
        }*/
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
        TorretaBasica[] torretas = FindObjectsOfType<TorretaBasica>();
        foreach (var torretasDesliga in torretas)
        {
            if (!torretasDesliga.munInfinita)
            {
                torretasDesliga.podeAtirar = false;
            }

            yield return new WaitForSeconds(tempoPEMAtivado);

            foreach (var torretasAtiva in torretas)
            {
                torretasAtiva.podeAtirar = true;
            }
            Debug.Log("Pem terminou, torretas reativadas");
        }
    }
    
    public void DanoPontoFraco(int dano)
    {
        if(golpeCarregando)
        {
            danoDuranteCarga += dano;
        }
    }
}
