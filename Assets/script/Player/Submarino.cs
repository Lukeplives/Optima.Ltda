using UnityEngine;

public class Submarino : MonoBehaviour
{

    private Rigidbody2D submarino;
    public float subSpeed;
    public float hp => GameManager.Instance.QtdComb;

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


    }

}
