
using UnityEngine;

public class InimigoKamikaze : Inimigo
{


    [Header("Kamikaze configs")]
    public GameObject explosionEffect;
    [SerializeField] private int danoExplosão;

    private bool wasDestroyed;

    protected override void Start()
    {
        base.Start();
    }


    protected override void Update()
    {
        if (player == null) { return; }

        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        OlharPlayer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Submarino playerScript = other.GetComponent<Submarino>();
            if (playerScript != null)
            {
                GameManager.Instance.DanoAoPlayer(danoExplosão);
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
    {
        if (wasDestroyed) return;

        wasDestroyed = true;
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        RadarWave radar = FindFirstObjectByType<RadarWave>();
        if(radar != null)
        {
            radar.RemoverInimigo(tipoInimigo);
        }
        Destroy(gameObject);
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
