
using UnityEngine;

public class InimigoKamikaze : Inimigo
{


    [Header("Kamikaze configs")]
    public GameObject explosionEffect;
    [SerializeField] private int danoExplosão;

    protected override void Start()
    {
        base.Start();
    }


    protected override void Update()
    {
        if (player == null) { return; }

        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Submarino playerScript = other.GetComponent<Submarino>();
            if (playerScript != null)
            {
                playerScript.hp -= danoExplosão;
            }

            Explodir();
        }
    }

    public override void TomaDano(int dano)
    {
        base.TomaDano(dano);
        Explodir();
    }

    private void Explodir()
    {   if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        }
        Destroy(gameObject);
    }
}
