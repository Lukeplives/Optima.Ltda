using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NovoItem", menuName = "Item")]
public class ItemSettings : ScriptableObject
{
    public Sprite lootsprite;
    public string lootname;
    public int dropChance;


    public float upGas;
    public int upFerro;

    public int upPeças;

    public enum TipoItem
    {
        Ferro,
        Gas,
        Peça
    }

    public TipoItem tipo;



}
