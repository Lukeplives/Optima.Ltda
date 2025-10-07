using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Transform playerposition;
    public WaveData[] waves;



    private List<GameObject> inimigosVivos = new List<GameObject>();
    private bool waveAtiva = false;
    private int waveAtualIndex = 0;

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
            Vector2 spawnPos = (Vector2)transform.position + spawnData.customPos;
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

    private void RemoverInimigo(GameObject inimigo)
    {
        inimigosVivos.Remove(inimigo);
    }
}
