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
    private FeedbackDamage feedbackDamage;
     private float timeBtwShots;
    [SerializeField] private float startTimeBtwShots;
    public float spawnDelay;

    public InimigoSettings.tipoInimigo tipoInimigo;

    private float alturaAtaque;
    private bool travarAltura = false;

    private bool morreu = false;

    [Header("Referencias")]

    public GameObject projetil;

    public GameObject prefabInimigo;

    public Vector2 spawnPosition;

    public Transform player;

    public event Action onDeath;

    void Awake()
    {
        feedbackDamage = GetComponent<FeedbackDamage>();
    }
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

        OlharPlayer();
    }

    public virtual void TomaDano(int dano)
    {
        if (morreu) return;
        hitPoints -= dano;
        feedbackDamage?.Flash();
        if (hitPoints <= 0)
        {
            morreu = true;

            onDeath?.Invoke();
            RadarWave radar = FindFirstObjectByType<RadarWave>();
            if(radar != null)
            {
                radar.RemoverInimigo(tipoInimigo);
            }
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
    
    void OlharPlayer()
    {
        if (player == null) return;
        
        if(player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        } else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}