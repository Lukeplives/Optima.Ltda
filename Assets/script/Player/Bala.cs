using UnityEngine;

public class Bala : MonoBehaviour
{

    [Header("Referências")]

    [SerializeField] private Rigidbody2D rb;
    private Transform alvo;





    [Header("Atributos")]
    [SerializeField] private float balaVelocidade = 5f;
    public int danoBala = 1;

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
        if (!alvo)
        {
            return;
        }
        Vector2 direction = (alvo.position - transform.position).normalized;

        rb.linearVelocity = direction * balaVelocidade;


    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        /*if (other.gameObject.GetComponent<Inimigo>().tipos.Equals(Inimigo.Tipos.Grande) && tipos == DanoExtra.sniper)
        {
            danoBala *= 2;
        }*/

        Inimigo inimigo = other.gameObject.GetComponent<Inimigo>();
        if (inimigo != null)
        {
            inimigo.TomaDano(danoBala);
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

    /*protected virtual void OnHit(Collision2D other)
    {

    }*/
    
        private void OnDrawGizmosSelected()
    {
        if (tipo == TipoDeBala.Bombardeira)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, raioExplosão);
        }
    }

}
