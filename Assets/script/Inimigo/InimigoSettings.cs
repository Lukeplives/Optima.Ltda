using UnityEngine;

[CreateAssetMenu(fileName = "Novo Inimigo", menuName = "Inimigo")]
public class InimigoSettings : ScriptableObject
{
    public GameObject prefabInimigo;
    public enum SpawnSide
    {
        Left,
        Right
    }

    public enum tipoInimigo
    {
        Terrestre,
        Kamikaze,
        Grande,
        Voador
    }
    public SpawnSide lado;
    public tipoInimigo tipo;

    public float speed;
    public float stoppingDistance;
    public float retreatDistance;
    public int hitPoints;

    public float startTimeBtwShots;

    public GameObject projetil;
}
