using UnityEngine;

public class SpriteProjetilVoador : MonoBehaviour
{
    public Sprite[] spritesPossiveis;
    
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (spritesPossiveis.Length > 0)
        {
            int index = Random.Range(0, spritesPossiveis.Length);
            sr.sprite = spritesPossiveis[index];
        }
    }

}
