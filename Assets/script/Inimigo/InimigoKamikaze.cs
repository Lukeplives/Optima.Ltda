
using Unity.VisualScripting;
using UnityEngine;

public class InimigoKamikaze : Inimigo
{


    [Header("Kamikaze configs")]
    private ObjectPool.PoolTag kamikazeEffectTag = ObjectPool.PoolTag.KamikazeExplosao;
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
        if (kamikazeEffectTag.IsUnityNull())
        {
            ObjectPool.Instance.SpawnFromPool(kamikazeEffectTag, transform.position, Quaternion.identity);
        }

        RadarWave radar = FindFirstObjectByType<RadarWave>();
        if (radar != null)
        {
            radar.RemoverInimigo(tipoInimigo);
        }
        ObjectPool.Instance.Despawn(gameObject);
    }
    
    public override void OnDisable()
    {
        base.OnDisable();
        wasDestroyed = false;
    }
}
