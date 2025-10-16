using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class BossWeakPoint : BossPart, IDamageable
{
    public BossController boss;

    private void Start()
    {
        if (boss == null) 
        { boss = GetComponentInParent<BossController>(); }

    }

    public override void TomaDano(int qtdDano)
    {
        base.TomaDano(qtdDano);
        if(boss != null)
        {
            boss.DanoPontoFraco(qtdDano);
            boss.TomaDano(qtdDano);
            Debug.Log("Tomou dano");
        }
    }
}
