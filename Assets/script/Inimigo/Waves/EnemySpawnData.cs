using System.Numerics;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{

    public InimigoSettings inimigo;
    public float spawnDelay;
    public UnityEngine.Vector2 customPos;
    public InimigoSettings.SpawnSide lado;

    public InimigoSettings.tipoInimigo tipoInimigo;
    


}
