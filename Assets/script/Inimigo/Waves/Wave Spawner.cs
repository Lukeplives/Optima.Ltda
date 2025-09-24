using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Transform playerposition;
    public WaveData[] waves;
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;

    public Transform upperRightSpawnPoint;
    public Transform upperLeftSpawnPoint;



    private int waveAtualIndex = 0;

    private void Update()
    {
        if (playerposition != null)
        {
          transform.position = playerposition.position;
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
        foreach (var spawnData in wave.inimigosWave)
        {
            Vector2 spawnPos = spawnData.customPos;
            if (spawnPos.x < 0)
            {
                spawnPos = leftSpawnPoint.position;
                if (spawnPos.y >= 1)
                {
                    spawnPos = upperLeftSpawnPoint.position;
                }
                

            }
            else
            {
                spawnPos = rightSpawnPoint.position;
                if (spawnPos.y >= 1)
                {
                    spawnPos = upperRightSpawnPoint.position;
                }
                
            }
            
            GameObject inimigoNovo = Instantiate(spawnData.inimigo.prefabInimigo, spawnPos, Quaternion.identity);
            Inimigo inimigoComponent = inimigoNovo.GetComponent<Inimigo>();
            if (inimigoComponent != null)
            {
                inimigoComponent.Initialize(spawnData.inimigo);
            }

            yield return new WaitForSeconds(spawnData.spawnDelay);
        }
    }
}
