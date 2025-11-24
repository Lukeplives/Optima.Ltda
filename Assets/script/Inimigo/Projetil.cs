using UnityEngine;

public class Projetil : MonoBehaviour
{
    public ProjetilSettings settings;

    private Vector2 direction;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float speed = settings.speed;
        int dano = settings.dano;

        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player != null)
        {
            direction = (player.position - transform.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }

        rb.linearVelocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.DanoAoPlayer(settings.dano);
            Destroy(gameObject);
        }
    }
}
