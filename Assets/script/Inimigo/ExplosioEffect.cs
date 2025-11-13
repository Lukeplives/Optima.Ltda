using UnityEngine;

public class ExplosioEffect : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;
    private float timer;
    void OnEnable()
    {
        timer = lifetime;
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ObjectPool.Instance.Despawn(gameObject);
        }
    }
}
