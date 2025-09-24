using System;
using UnityEngine;

public class LinhadeChegada : MonoBehaviour
{
    [SerializeField] private Submarino player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.peçasNum >= 3)
        {
            Debug.Log("Venceu!");
        }
        else
        {
            Debug.Log("Perdeu :(");
        }
        
    }
}
