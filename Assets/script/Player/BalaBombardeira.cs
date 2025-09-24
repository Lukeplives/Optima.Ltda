using UnityEngine;
using UnityEngine.Rendering;

public class BalaBombardeira : MonoBehaviour
{
    private Transform alvo;
    [Header("Referências")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject exploZona;


    [Header("Atributos")]
    [SerializeField] private float balaVelocidade = 5f;
    public int danoBala = 1;
    

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
        other.gameObject.GetComponent<Inimigo>().TomaDano(danoBala);
        Explode();
    }

    private void Explode()
    {
        exploZona.SetActive(true);
        Instantiate(exploZona, transform.position, Quaternion.identity);

        Destroy(gameObject, exploZona.GetComponent<ZonadeExplosão>().exploDuração);

    }
    


}
