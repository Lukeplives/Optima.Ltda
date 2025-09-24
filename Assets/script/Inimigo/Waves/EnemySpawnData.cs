using System.Numerics;
using UnityEngine;

public class EnemySpawnData : MonoBehaviour
{
    public enum SpawnSide
    {
        Left,
        Right
    }

    public InimigoSettings inimigo;
    public float spawnDelay;
    public UnityEngine.Vector2 customPos;
    public SpawnSide lado;


}
