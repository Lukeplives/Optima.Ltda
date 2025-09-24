using System.Collections;
using UnityEngine;

public class VeioFerro : MonoBehaviour
{

    [SerializeField] private VeioScriptableObject VeioFerroConfig;
    public DetectorCano NDetect;
    public Transform NSpawn, SSpawn, ESpawn, WSpawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator SpawnRec(float intervalo, GameObject Recurso, Transform SpawnLocation)
    {
        yield return new WaitForSeconds(intervalo);
        GameObject RecursoPlus = Instantiate(Recurso, SpawnLocation.position, Quaternion.identity);
        StartCoroutine(SpawnRec(intervalo, Recurso, SpawnLocation));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Belt"))
        {
            StartCoroutine(SpawnRec(VeioFerroConfig.intervalo, VeioFerroConfig.RecPrefab, NSpawn));
        }
    }
}
