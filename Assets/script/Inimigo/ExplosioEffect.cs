using UnityEngine;

public class ExplosioEffect : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;
    void Start()
    {
        Destroy(gameObject, lifetime);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
