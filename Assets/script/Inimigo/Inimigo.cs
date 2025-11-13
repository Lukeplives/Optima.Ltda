using System;
using System.Collections;
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
    private int hitPointsMax;
    private FeedbackDamage feedbackDamage;
     private float timeBtwShots;
    [SerializeField] private float startTimeBtwShots;
    public float spawnDelay;

    public InimigoSettings.tipoInimigo tipoInimigo;

    public ObjectPool.PoolTag tipoPool;

    private float alturaAtaque;
    private bool travarAltura = false;

    private bool morreu = false;

    private bool inicializado = false;

    [Header("Referencias")]

    public GameObject projetil;

    public GameObject prefabInimigo;

    public Vector2 spawnPosition;

    public Transform player;

    public event Action onDeath;

    private Collider2D col;
    private SpriteRenderer sprite;

    void Awake()
    {
        feedbackDamage = GetComponent<FeedbackDamage>();
        col = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();

        hitPointsMax = hitPoints;
        
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
        player = GameObject.FindGameObjectWithTag("Player").transform;


        timeBtwShots = startTimeBtwShots;

        tipoPool = settings.tipoPool;
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
            Morrer();
        }
    }

    public void Morrer()
    {
        if (morreu) return;
        morreu = true;

        onDeath?.Invoke();

        if (col) col.enabled = false;
        if (sprite) sprite.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        GetComponent<LootBag>().InstanciaItem(transform.position);

        StartCoroutine(DesativarDepoisDeFrame());
        
        
    }
    
    private IEnumerator DesativarDepoisDeFrame()
    {
        yield return null;
        ObjectPool.Instance.Despawn(gameObject);
    }

    protected virtual void Atacar()
    {
        if (timeBtwShots <= 0)
        {
            //Instantiate(projetil, transform.position, Quaternion.identity);

            switch(tipoInimigo)
            {
                case InimigoSettings.tipoInimigo.Terrestre:
                    ObjectPool.Instance.SpawnFromPool("ProjetilTerrestre", transform.position,Quaternion.identity);
                    break;
                case InimigoSettings.tipoInimigo.Voador:
                    ObjectPool.Instance.SpawnFromPool("ProjetilVoador", transform.position,Quaternion.identity);
                    break;
                case InimigoSettings.tipoInimigo.Grande:
                    ObjectPool.Instance.SpawnFromPool("ProjetilGrande", transform.position,Quaternion.identity);
                    break;
            }
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

        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnEnable()
    {
        if (inicializado)
        {
            ResetarEstado();
        }
        inicializado = true;
    }

    public virtual void OnDisable()
    {

    }
    
    private void ResetarEstado()
    {
        hitPoints = hitPointsMax;
        if (col) col.enabled = true;
        if (sprite) sprite.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Inimigos");
        morreu = false;
    }


}