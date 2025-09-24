using System;
using Unity.VisualScripting;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public InimigoSettings settings;
    [Header("Atributos")]
    [SerializeField] private float speed;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float retreatDistance;
    [SerializeField] private int hitPoints;
     private float timeBtwShots;
    [SerializeField] private float startTimeBtwShots;
    public float spawnDelay;

    [Header("Referencias")]

    public GameObject projetil;

    public GameObject prefabInimigo;

    public Vector2 spawnPosition;

    public Transform player;

    /*public enum Tipos
    {
        Basico,
        Voador,
        Grande
    }

    public Enum tipos;*/

    void Start()
    {
        speed = settings.speed;
        stoppingDistance = settings.stoppingDistance;
        retreatDistance = settings.retreatDistance;
        hitPoints = settings.hitPoints;
     
        startTimeBtwShots = settings.startTimeBtwShots;

        prefabInimigo = settings.prefabInimigo;
        spawnPosition = settings.spawnPosition;
        spawnDelay = settings.spawnDelay;


        if (player != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        

        timeBtwShots = startTimeBtwShots;
    }
    public void Initialize(InimigoSettings newSettings)
    {
        settings = newSettings;
        hitPoints = settings.hitPoints;



    }

    void Update()
    {
        if (player != null)
        {
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        }
        else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance)
        {
            transform.position = this.transform.position;
            if (timeBtwShots <= 0)
            {
                Instantiate(projetil, transform.position, Quaternion.identity);
                timeBtwShots = startTimeBtwShots;
            }
            else
            {
                timeBtwShots -= Time.deltaTime;
            }
        }
        else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
        }
    }
    
    public void TomaDano(int dano)
    {
        hitPoints -= dano;

        if (hitPoints <= 0)
        {
            GetComponent<LootBag>().InstanciaItem(transform.position);
            Destroy(gameObject);
        }
    } 

    /*
    public InimigoSettings settings;
    public GameObject player;
    public float speed;
    [SerializeField] private int hitPoints = 2;

    private float distance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        speed = settings.speed;
        distance = settings.distance;
        hitPoints = settings.hitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }

    public void TomaDano(int dano)
    {
        hitPoints -= dano;

        if (hitPoints <= 0)
        {
            
            Destroy(gameObject);
        }
    }
    
*/
}
