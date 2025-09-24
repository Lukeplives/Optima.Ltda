using UnityEngine;

public class Bala : MonoBehaviour
{
    private Transform alvo;
    [Header("Referências")]
    [SerializeField] private Rigidbody2D rb;



    [Header("Atributos")]
    [SerializeField] private float balaVelocidade = 5f;
    public int danoBala = 1;

    /*public enum DanoExtra
    {
        metralhadora,
        sniper,
        bombardeira
    }

    public DanoExtra tipos;*/
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
        other.gameObject.GetComponent<Inimigo>().TomaDano(danoBala);
        Destroy(gameObject);
    }

}
