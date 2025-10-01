using UnityEditor.ShaderGraph.Internal;
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

    public void TomaDano(int dano)
    {
        hitPoints -= dano;

        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        if (GetComponent<LootBag>() != null)
        {
            GetComponent<LootBag>().InstanciaItem(transform.position);
        }
        }
    }

    private void Explodir()
    {   if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
