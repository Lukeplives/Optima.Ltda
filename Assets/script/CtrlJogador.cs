using UnityEngine;
using UnityEngine.InputSystem;


public class CtrlJogador : MonoBehaviour
{
    Rigidbody2D rb;
    public float movSpeed = 5f;

    Vector2 moveDirection = Vector2.zero;
    public InputAction playerControls;

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //float moveX = Input.GetAxis("Horizontal");
        //float moveY = Input.GetAxis("Vertical");

        moveDirection = playerControls.ReadValue<Vector2>();

    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * movSpeed, moveDirection.y * movSpeed);
    }
}
