using UnityEngine;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    public SpriteRenderer cursorImage;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
    }

    public void AtivarCursor(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        gameObject.SetActive(true);
    }

    public void DesativarCursor()
    {
        gameObject.SetActive(false);
    }
}
