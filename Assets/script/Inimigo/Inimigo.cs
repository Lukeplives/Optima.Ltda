using System;
using Unity.VisualScripting;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public InimigoSettings settings;
    [Header("Atributos")]
    public float speed;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float retreatDistance;
    public int hitPoints;
     private float timeBtwShots;
    [SerializeField] private float startTimeBtwShots;
    public float spawnDelay;

    [Header("Referencias")]

    public GameObject projetil;

    public GameObject prefabInimigo;

    public Vector2 spawnPosition;

    public Transform player;

    public event Action onDeath;
    protected virtual void Start()
    {
        speed = settings.speed;
        stoppingDistance = settings.stoppingDistance;
        retreatDistance = settings.retreatDistance;
        hitPoints = settings.hitPoints;
     
        startTimeBtwShots = settings.startTimeBtwShots;

        prefabInimigo = settings.prefabInimigo;
        //spawnPosition = settings.spawnPosition;
        //spawnDelay = settings.spawnDelay;


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

    protected virtual void Update()
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
    
    public virtual void TomaDano(int dano)
    {
        hitPoints -= dano;

        if (hitPoints <= 0)
        {
            onDeath?.Invoke();
            GetComponent<LootBag>().InstanciaItem(transform.position);
            Destroy(gameObject);
        }
    } 

    
}
