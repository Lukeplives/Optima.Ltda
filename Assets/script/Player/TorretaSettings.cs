using UnityEngine;

[CreateAssetMenu(fileName = "Nova Torreta", menuName = "Torreta")]
public class TorretaSettings : ScriptableObject
{

    [Header("Atributos")]
    public string nomeTorreta;
    public int custoRec;
    public int custoComb;
    public float targetingRange = 5f;
    public float rotationSpeed = 5f;
    public float bps = 1f;

    public float tempoDeVida;
    public int danoTorreta;

    public int munMax;
    public bool munInfinita;

    public ObjectPool.PoolTag tagBala;
    public BuildManager.TipoTorreta tagTorreta;


    [Header("Referencias de Obj")]

    public LayerMask enemyMask;
    public GameObject balaPrefab;
    


}
