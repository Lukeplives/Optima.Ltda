using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;


public class Torretalançachama : MonoBehaviour
{
    [Header("Referencias de OBJ")]
    public TorretaSettings settings;
    [SerializeField] private Transform turretRotationPoint;
    public LayerMask enemyMask;
    [SerializeField] private Transform firingPoint;
    private Building torretaBuild;
    private Transform target;

    public GameObject efeitoChama;


    [Header("Atributos")]

    [SerializeField] private float raioChama;
    [SerializeField] private float danoPorSegundo;
    [SerializeField] private float tempoDeVida;
    public float targetingRange = 5f;
    public float rotationSpeed = 5f;

    public int munMax;
    private int munAtual;
    [SerializeField] private float munGasta;

    [SerializeField] bool podeAtirar = true;
    

    [Header("UI")]
    public GameObject ammoUIPrefab;
    private Slider ammoSlider;

    private float timeUntilFire;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        torretaBuild = GetComponent<Building>();

        enemyMask = settings.enemyMask;

        targetingRange = settings.targetingRange;
        rotationSpeed = settings.rotationSpeed;
        tempoDeVida = settings.tempoDeVida;

        munAtual = munMax;

        if (ammoUIPrefab != null)
        {
            GameObject uiObject = Instantiate(ammoUIPrefab, transform.position + Vector3.up, Quaternion.identity);
            uiObject.transform.SetParent(transform);
            ammoSlider = uiObject.GetComponentInChildren<Slider>();
            ammoSlider.maxValue = munMax;
            ammoSlider.value = munAtual;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (munAtual <= 0)
        {
            if (torretaBuild.originTile != null)
                torretaBuild.originTile.isOccupied = false;
            Destroy(gameObject);
            return;
        }


        Collider2D[] hits = Physics2D.OverlapCircleAll(firingPoint.position, raioChama, enemyMask);

        if (hits.Length > 0)
        {
            efeitoChama.SetActive(true);

            foreach (Collider2D hit in hits)
            {
                Inimigo inimigo = hit.GetComponent<Inimigo>();
                if (inimigo != null)
                {

                    inimigo.TomaDano((int)(danoPorSegundo * Time.deltaTime));
                }
            }


            munAtual -= Mathf.CeilToInt(munGasta * Time.deltaTime);
            ammoSlider.value = munAtual;

        }
        else
        {
            efeitoChama.SetActive(false);
        }

        if (target == null)
            {
                FindTarget();
                return;
            }
        RotateTowardsTarget();
        

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

    
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(firingPoint.position, raioChama);
    }
}
