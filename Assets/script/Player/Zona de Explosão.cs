using UnityEngine;

public class ZonadeExplosão : MonoBehaviour
{
    private Collider2D col;
    [SerializeField] private int exploDano;
    public float exploDuração = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Inimigo"))
        {
            collision.GetComponent<Inimigo>().TomaDano(exploDano);
            Destroy(gameObject, exploDuração);
        }
        
    }
}
