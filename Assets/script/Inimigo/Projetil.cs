using UnityEngine;

public class Projetil : MonoBehaviour
{
    [SerializeField] private float speed;
    private Transform player;
    private Vector2 target;
    public int dano;

    public ProjetilSettings settings;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dano = settings.dano;
        speed = settings.speed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player != null)
        {


            target = new Vector2(player.position.x, player.position.y);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            DestroyProjetil();
        }
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.DanoAoPlayer(dano);
            DestroyProjetil();
        }
    }

    void DestroyProjetil()
    {
        Destroy(gameObject);
    }
}
