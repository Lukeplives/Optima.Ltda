using UnityEngine;
using UnityEngine.Rendering;

public class AmmoUIFollow : MonoBehaviour
{
    public Transform torreta;
    public Vector3 offset;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (torreta == null)
        {
            ObjectPool.Instance.Despawn(gameObject);
            return;
        }

        Vector3 screenPos = cam.WorldToScreenPoint(torreta.position + Vector3.up * 1f);

        transform.position = screenPos + offset;
    }
}
