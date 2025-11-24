using UnityEngine;

public class DestroyParticula : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var ps = GetComponent<ParticleSystem>();
        Destroy(gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
