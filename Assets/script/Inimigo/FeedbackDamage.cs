using System.Collections;
using UnityEngine;

public class FeedbackDamage : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] spriteRenderers;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashTime = 0.1f;

    private Color[] coresOriginais;
    private bool isFlashing = false;

    void Awake()
    {
        if (spriteRenderers == null || spriteRenderers.Length == 0)
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        }
        coresOriginais = new Color[spriteRenderers.Length];
        
        for(int i = 0; i < spriteRenderers.Length; i++)
        {
            coresOriginais[i] = spriteRenderers[i].color;
        }
            
        


    }

    public void Flash()
    {

            if (!isFlashing)
            {
                StartCoroutine(FlashCoroutine());
            }

    }
    
    private IEnumerator FlashCoroutine()
    {
        isFlashing = true;
        foreach (SpriteRenderer sprites in spriteRenderers)
        {
            sprites.color = damageColor;
        }

        yield return new WaitForSeconds(flashTime);

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null)
            {
                spriteRenderers[i].color = coresOriginais[i];
            }
        }

        isFlashing = false;
    }
}
