using System.Collections;

using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Itens possiveis de spawnar")]
    [SerializeField] private ObjectPool.PoolTag[] itensDisponíveis;

    [Header("Intervalo de tempo entre spawns (segundos)")]
    [SerializeField] private float minDelay = 5f;
    [SerializeField] private float maxDelay = 15f;

    [Header("Altura do spawn")]
    [SerializeField] private float minAltura = 3f;
    [SerializeField] private float maxAltura = 3f;

    [Header("Posição base do spawn")]
    [SerializeField] private float posX = 10f;
    [SerializeField] private float posZ = 0f;

    [Header("Controle")]
    [SerializeField] private bool ativo = true;
    [SerializeField] private Transform player;


    void Start()
    {
        StartCoroutine(CicloSpawn());
    }


    private IEnumerator CicloSpawn()
    {
        while (ativo)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            SpawnItemAleatório();
        }
    }

    private void SpawnItemAleatório()
    {
        if (itensDisponíveis.Length == 0)
        {
            Debug.Log("Não tem itens para dropar");
            return;
        }

        int indexItem = Random.Range(0, itensDisponíveis.Length);
        ObjectPool.PoolTag tagSelecionada = itensDisponíveis[indexItem];

        float y = Random.Range(minAltura, maxAltura);
        if(player != null)
        {
            Vector3 posicaoSpawn = new Vector3(posX + player.position.x, y, posZ);
            GameObject itemSpawnado = ObjectPool.Instance.SpawnFromPool(tagSelecionada, posicaoSpawn, Quaternion.identity);
            //Instantiate(itensDisponíveis[indexItem], posicaoSpawn, Quaternion.identity);
        }


        
    }
}
