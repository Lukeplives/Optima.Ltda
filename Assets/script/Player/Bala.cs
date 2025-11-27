
using Unity.VisualScripting;
using UnityEngine;

public class Bala : MonoBehaviour
{

    [Header("Referências")]

    [SerializeField] private Rigidbody2D rb;
    private Transform alvo;

    [Header("Atributos")]
    [SerializeField] private float balaVelocidade = 5f;
    public int danoBala = 1;

    private Vector2? direcaoManual = null;

    public enum TipoDeBala
    {
        Metralhadora,
        Bombardeira,
        Sniper,

    }

    [Header("Tipo da bala")]
    public TipoDeBala tipo;

    [Header("Configs bala Explosiva")]
    public float raioExplosão;
    public int danoExplosão;
    public LayerMask enemyMask;
    [SerializeField] GameObject efeitoExplosao;





    public void SetTarget(Transform _alvo)
    {
        alvo = _alvo;
    }
    private void FixedUpdate()
    {
        Vector2 direcao;
        if (direcaoManual != null)
        {
            direcao = direcaoManual.Value;
        }
        else if (alvo)
        {
            direcao = (alvo.position - transform.position).normalized;
        }
        else
        {
            return;
        }
        
        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angulo);
        

        rb.linearVelocity = direcao * balaVelocidade;


    }
    public void SetDirection(Vector2 dir)
    {
        direcaoManual = dir.normalized;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if(damageable == null)
        {
            damageable = other.gameObject.gameObject.GetComponentInParent<IDamageable>();
        }
        if (damageable != null)
        {
            damageable.TomaDano(danoBala);
        }
        else
        {
            Inimigo inimigo = other.gameObject.GetComponent<Inimigo>();
            if (inimigo != null)
            {
                inimigo.TomaDano(danoBala);
            }
        }


        //OnHit(other);
        switch (tipo)
        {
            case TipoDeBala.Bombardeira:
                Explodir();

                break;

        }

        Destroy(gameObject);
    }

    private void Explodir()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, raioExplosão, enemyMask);
        foreach (Collider2D hit in hits)
        {

            Inimigo inimigo = hit.GetComponent<Inimigo>();
            inimigo.TomaDano(danoExplosão);
        }

        if (efeitoExplosao != null)
        {
            Instantiate(efeitoExplosao, transform.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (tipo == TipoDeBala.Bombardeira)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, raioExplosão);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
