using UnityEngine;
using UnityEditor;

public class TorretaBasica : MonoBehaviour
{
    public TorretaSettings settings;
    [Header("Referencias de Obj")]
    [SerializeField] private Transform turretRotationPoint;
    public LayerMask enemyMask;
    public GameObject balaPrefab;
    [SerializeField] private Transform firingPoint;
    public Building torretaBuild;




    [Header("Atributos")]
    public float targetingRange = 5f;
    public float rotationSpeed = 5f;
    public float bps = 1f; //Balas por segundo
    public float tempoDeVida;
    public int danoTorreta;


    private float timeUntilFire;
    private Transform target;

    /*private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }*/
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        torretaBuild = GetComponent<Building>();
        enemyMask = settings.enemyMask;
        balaPrefab = settings.balaPrefab;


        danoTorreta = settings.danoTorreta;
        targetingRange = settings.targetingRange;
        rotationSpeed = settings.rotationSpeed;
        bps = settings.bps;
        tempoDeVida = settings.tempoDeVida;
    }

    // Update is called once per frame
    private void Update()
    {
        tempoDeVida -= 1 * Time.deltaTime;
        if (torretaBuild.originTile != null)
        {
             if (tempoDeVida <= 0)
        {
            torretaBuild.originTile.isOccupied = false;
            torretaBuild.originTile = null;
            Destroy(gameObject);
        }
        }
       

        if (target == null)
        {
            FindTarget();
            return;
        }
        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }


    private void Shoot()
    {
        GameObject balaObj = Instantiate(balaPrefab, firingPoint.position, Quaternion.identity);
        Bala balaScript = balaObj.GetComponent<Bala>();
        balaScript.danoBala = danoTorreta;
        balaScript.SetTarget(target);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void OnDestroy()
    {
        if (torretaBuild.originTile != null)
        {
            torretaBuild.originTile.isOccupied = false;

        }
    }
}
