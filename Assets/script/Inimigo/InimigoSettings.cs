using UnityEngine;

[CreateAssetMenu(fileName = "Novo Inimigo", menuName = "Inimigo")]
public class InimigoSettings : ScriptableObject
{
    public GameObject prefabInimigo;
    //public float spawnDelay;
    //public Vector2 spawnPosition;
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;
    public int hitPoints;

    public float startTimeBtwShots;

    public GameObject projetil;
}
