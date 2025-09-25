using System.Numerics;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData
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
