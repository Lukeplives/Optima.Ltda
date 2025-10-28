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

    public InimigoSettings.tipoInimigo tipoInimigo;

    private float alturaAtaque;
    private bool travarAltura = false;

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
        tipoInimigo = settings.tipo;

        
        //spawnPosition = settings.spawnPosition;
        //spawnDelay = settings.spawnDelay;


        player = GameObject.FindGameObjectWithTag("Player").transform;
        

        timeBtwShots = startTimeBtwShots;
    }
    public void Initialize(InimigoSettings newSettings)
    {
        settings = newSettings;
        hitPoints = settings.hitPoints;
        


    }

    protected virtual void Update()
    {
        if (player == null) return;

        float distancia = Vector2.Distance(transform.position, player.position);

        if (distancia > stoppingDistance)
        {
            Vector2 posAlvo = player.position;

            if (travarAltura)
            {
                posAlvo.y = alturaAtaque;
            }
            transform.position = Vector2.MoveTowards(transform.position, posAlvo, speed * Time.deltaTime);
        }
        else
        {
            if (!travarAltura)
            {
                alturaAtaque = transform.position.y;
                travarAltura = true;
            }

            transform.position = new Vector2(transform.position.x, alturaAtaque);
            Atacar();
        }
        /*if (player != null)
        {
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        }
        else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance)
        {
            transform.position = this.transform.position;
            
        }
        else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
        }*/
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
    
    protected virtual void Atacar()
    {
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

    
}
