using UnityEngine;

public class Submarino : MonoBehaviour
{

    private Rigidbody2D submarino;
    public float subSpeed;
    public int hp;

    public int peçasNum;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        submarino = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //submarino.linearVelocity = new Vector2(subSpeed, 0);
        transform.Translate(Vector2.right * subSpeed * Time.deltaTime);
        if (hp <= 0)
        {
            hp = 0;
            GameManager.Instance.PlayerDead(gameObject);
        }

        if(Input.GetKeyDown(KeyCode.F6))
        {
            hp += 1000;
        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projetil Inimigo")
        {
            hp -= collision.gameObject.GetComponent<Projetil>().dano;
            
        }
    }
}
