using System;
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

    public event Action OnBossDefeated;
    private Submarino player;

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
    }
    
    void Derrotado()
    {
        Debug.Log("Boss Derrotado");
        OnBossDefeated?.Invoke();
        Destroy(gameObject);
    }
}
