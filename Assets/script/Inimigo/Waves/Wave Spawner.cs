using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    
    [SerializeField] private Transform playerposition;
    public WaveData[] waves;

    public event Action OnWaveCompleted;

    private List<GameObject> inimigosVivos = new List<GameObject>();
    private bool waveAtiva = false;
    private int waveAtualIndex = 0;

    [Header("Offsets spawn")]
    public float offsetHorizontal;
    public float offsetVertical;

    [Header("Altura dos inimigos voadores")]
    public float minAltura = 3f;
    public float maxAltura = 7f;

    private void Update()
    {
        if (playerposition != null)
        {
            transform.position = playerposition.position;
        }
        if (waveAtiva && inimigosVivos.Count == 0)
        {
            waveAtiva = false;
            Debug.Log("Wave concluída");
            OnWaveCompleted?.Invoke();
        }

    }

    public void StartWave(int index)
    {
        if (index < waves.Length)
        {
            waveAtualIndex = index;
            StartCoroutine(SpawnWave(waves[index]));
        }
    }

    IEnumerator SpawnWave(WaveData wave)
    {
        waveAtiva = true;
        inimigosVivos.Clear();
        foreach (var spawnData in wave.inimigosWave)
        {

            Vector2 spawnPos = CalcularSpawnPosition(spawnData.lado, spawnData.customPos);
            if(spawnData.inimigo.tipo == InimigoSettings.tipoInimigo.Voador)
            {
                float alturaAleatoria = UnityEngine.Random.Range(minAltura, maxAltura);
                spawnPos.y += alturaAleatoria;
            }
            
            GameObject inimigoNovo = Instantiate(spawnData.inimigo.prefabInimigo, spawnPos, Quaternion.identity);

            inimigosVivos.Add(inimigoNovo);
            Inimigo inimigoComponent = inimigoNovo.GetComponent<Inimigo>();
            if (inimigoComponent != null)
            {
                inimigoComponent.onDeath += () => RemoverInimigo(inimigoNovo);
                inimigoComponent.Initialize(spawnData.inimigo);
            }

            yield return new WaitForSeconds(spawnData.spawnDelay);
        }
    }

    private Vector2 CalcularSpawnPosition(InimigoSettings.SpawnSide lado, Vector2 customOffset)
    {
        Vector2 basePos = transform.position;
        switch(lado)
        {
            case InimigoSettings.SpawnSide.Left:
                return new Vector2(basePos.x - offsetHorizontal, basePos.y + offsetVertical);
            case InimigoSettings.SpawnSide.Right:
                return new Vector2(basePos.x + offsetHorizontal, basePos.y + offsetVertical);
            default:
                return basePos;
        }
    }

    private void RemoverInimigo(GameObject inimigo)
    {
        inimigosVivos.Remove(inimigo);
    }
}
