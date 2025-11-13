using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject prefabDropada;
    public List<ItemSettings> lootList = new List<ItemSettings>();

    ItemSettings GetItemDropado()
    {
        int randomNumber = Random.Range(1, 101);
        List<ItemSettings> itensPossiveis = new List<ItemSettings>();
        foreach (ItemSettings item in lootList)
        {
            if (randomNumber <= item.dropChance)
            {
                itensPossiveis.Add(item);
            }
        }
        if (itensPossiveis.Count > 0)
        {
            ItemSettings itemDroppado = itensPossiveis[Random.Range(0, itensPossiveis.Count)];
            return itemDroppado;
        }
        return null;
    }

    public void InstanciaItem(Vector3 SpawnPosition)
    {
        ItemSettings itemDroppado = GetItemDropado();
        if (itemDroppado == null) return;

        GameObject itemGameObject = null;
        string pooltag = itemDroppado.tagPool.ToString();
        
        itemGameObject = ObjectPool.Instance.SpawnFromPool(pooltag, SpawnPosition, Quaternion.identity);
        Item itemScript = itemGameObject.GetComponent<Item>();
        if(itemScript != null)
        {
            itemScript.Initialize(itemDroppado);
        }

        
    }
}
