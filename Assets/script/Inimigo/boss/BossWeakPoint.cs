using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class BossWeakPoint : BossPart
{
    public BossController boss;

    private void Start()
    {
        if (boss == null) { GetComponentInParent<BossController>(); }

    }

    public override void TomaDano(int qtdDano)
    {
        base.TomaDano(qtdDano);
        if(boss != null)
        {
            boss.TomaDano(qtdDano);
        }
    }
}
