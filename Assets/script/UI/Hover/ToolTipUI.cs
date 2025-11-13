using System;
using TMPro;
using UnityEngine;

public class ToolTipUI : MonoBehaviour
{


    public static ToolTipUI Instance;

    void Awake()
    {
        if(Instance != null && Instance != this )
        {
            ObjectPool.Instance.Despawn(this.gameObject);
        } else
        {
            Instance = this;
        }
    }
    void Start()
    {

    }
    




    void Update()
    {

        
    }
}
