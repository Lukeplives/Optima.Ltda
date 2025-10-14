using System;
using UnityEngine;

public class BossPart : MonoBehaviour, IDamageable
{
    public int maxHP = 1500;
    public int atualHP;
    public event Action<BossPart> OnPartDestroyed;

    protected virtual void Awake()
    {
        atualHP = maxHP;
    }

    public virtual void TomaDano(int qtdDano)
    {
        atualHP -= qtdDano;
        if (atualHP <= 0) { Morre(); }

    }
    
    protected virtual void Morre()
    {
        OnPartDestroyed?.Invoke(this);
        gameObject.SetActive(false);
    }

}
