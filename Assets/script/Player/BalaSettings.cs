using UnityEngine;

[CreateAssetMenu(fileName = "new Bala", menuName = "Bala")]
public class BalaSettings : ScriptableObject
{
    public float balaVelocidade = 5f;
    public int danoBala = 1;
    
    public enum DanoExtra
    {
        Basico,
        Voador,
        Grande
    }

}
