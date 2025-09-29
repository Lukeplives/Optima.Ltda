using UnityEngine;
using UnityEngine.Rendering;

public class BalaBombardeira : Bala
{/*
    [Header("Explosão")]
    [SerializeField] private float raioExplosão = 3f;
    [SerializeField] private int danoExplosão = 2;
    public LayerMask enemyMask;

    protected override void OnHit(Collision2D other)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, raioExplosão, enemyMask);
        foreach (Collider2D hit in hits)
        {
            Inimigo inimigo = hit.GetComponent<Inimigo>();
            if (inimigo != null)
            {
                inimigo.TomaDano(danoExplosão);
            }
        }


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioExplosão);
    }

*/

}
