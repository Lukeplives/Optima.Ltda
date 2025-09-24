using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NovoItem", menuName = "Item")]
public class ItemSettings : ScriptableObject
{

    public GameObject prefabItem;
    public float upGas;
    public int upFerro;

    public int upPeças;

    public int dropChance;


}
