using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TentaculoSpawner : BossPart
{
    [Header("Spawn")]
    public InimigoSettings inimigoToSpawn;
    public Transform spawnPoint;
    public float spawnDelay;
    private bool spawning;
    private Coroutine spawnRoutine;
    protected override void Awake()
    {
        base.Awake();
    }

    public void SetFase(int fase)
    {
        spawnDelay = Mathf.Max(1f, spawnDelay - fase);
    }

    public void StartSpawning()
    {
        if (!spawning)
        {
            spawning = true;
            spawnRoutine = StartCoroutine(SpawnLoop());
        }

    }
    public void StopSpawning()
    {
        spawning = false;
        StopAllCoroutines();

    }

    IEnumerator SpawnLoop()
    {
        spawning = true;
        while (spawning && atualHP > 0)
        {
            if (inimigoToSpawn != null && spawnPoint != null)
            {
                GameObject inimigoSpawnado = null;
                if(!inimigoToSpawn.tipoPool.IsUnityNull())
                {
                    inimigoSpawnado = ObjectPool.Instance.SpawnFromPool(inimigoToSpawn.tipoPool, spawnPoint.position, Quaternion.identity);
                }
                var inimigoComponent = inimigoSpawnado.GetComponent<Inimigo>();
                if (inimigoComponent != null)
                {
                    inimigoComponent.Initialize(inimigoToSpawn);
                }
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }

    protected override void Morre()
    {
        StopSpawning();
        base.Morre();
    }
}
