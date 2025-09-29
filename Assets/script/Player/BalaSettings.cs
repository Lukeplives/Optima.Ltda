using UnityEngine;

[CreateAssetMenu(fileName = "new Bala", menuName = "Bala")]
public class BalaSettings : ScriptableObject
{
    public float balaVelocidade = 5f;
    public int danoBala = 1;

    public int multiplicadorDano;

    public enum DanoExtra
    {
        Basico,
        Voador,
        Grande
    }

    public enum TipoDeBala
    {
        Metralhadora,
        Bombardeira,
        Sniper,

    }
    [Header("Tipo da bala")]
    public TipoDeBala tipo;

    [Header("Configs bala Explosiva")]
    public float raioExplosão;
    public int danoExplosão;
    public LayerMask enemyMask;

}
