using UnityEngine;

public class SpawnItem : MonoBehaviour
{
 //projetil que vai atirar
    public GameObject projectile;

    //onde spawna o projetil
    public Transform NspawnLocation, SspawnLocation, WspawnLocation, EspawnLocation;

    //rotação do projetil no spawn
    public Quaternion spawnRotation;

    public float spawnTime = 2f;

    public float timeSinceSpawned = 2f;

    public DetectionSpawner detectionZone;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(detectionZone.detectedObjs.Count > 0)
        {
            timeSinceSpawned += Time.deltaTime;

            if(timeSinceSpawned >= spawnTime)
            {
                Instantiate(projectile,NspawnLocation.position, spawnRotation);
                timeSinceSpawned = 0;
            }
        }else
        {
            timeSinceSpawned = 2f;
        }

        
        
    }
}
