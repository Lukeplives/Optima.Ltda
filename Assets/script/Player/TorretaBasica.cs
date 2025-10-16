using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;


public class TorretaBasica : MonoBehaviour
{
    public TorretaSettings settings;
    [Header("Referencias de Obj")]
    [SerializeField] private Transform turretRotationPoint;
    public LayerMask enemyMask;
    public GameObject balaPrefab;
    [SerializeField] private Transform firingPoint;
    private Building torretaBuild;




    [Header("Atributos")]
    public float targetingRange = 5f;
    public float rotationSpeed = 5f;
    public float bps = 1f; //Balas por segundo
  
    public int danoTorreta;

    public int munMax;
    public int munAtual;

    public bool munInfinita;
    public bool modoManual = false;


    private float timeUntilFire;
    private Transform target;

    public bool podeAtirar = true;



    [Header("UI")]
    public GameObject ammoUIPrefab;

    private Slider ammoSlider;


    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
    void Start()
    {
        torretaBuild = GetComponent<Building>();
        enemyMask = settings.enemyMask;
        balaPrefab = settings.balaPrefab;


        danoTorreta = settings.danoTorreta;
        targetingRange = settings.targetingRange;
        rotationSpeed = settings.rotationSpeed;
        bps = settings.bps;
        munInfinita = settings.munInfinita;
        


            munMax = settings.munMax;
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
    private void Update()
    {
        if (modoManual && munInfinita)
        {
            ControleManual();
        }
        else
        {
            AtirarAutomatico();
        }




    }
    private void ControleManual()
    {
        Vector3 posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicaoMouse.z = 0f;

        Vector2 dir = posicaoMouse - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        turretRotationPoint.rotation = Quaternion.Euler(0, 0, angle-90);
        timeUntilFire += Time.deltaTime;
        if(Input.GetMouseButtonDown(0) && timeUntilFire >= 1f/bps)
        {
             Shoot();    
            timeUntilFire = 0f;

        }
    }
    private void AtirarAutomatico()
    {
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
    
    public void AlternarControleManual()
    {
        if (!munInfinita) { return; }
        modoManual = !modoManual;
        Debug.Log($"torreta {(modoManual ? "em controle manual" : "automático")}");
        
    }


    private void Shoot()
    {
        if (!munInfinita && munAtual <= 0 || !podeAtirar) { return; }
        GameObject balaObj = Instantiate(balaPrefab, firingPoint.position, Quaternion.identity);
        Bala balaScript = balaObj.GetComponent<Bala>();

        balaScript.danoBala = danoTorreta;
        if(modoManual && munInfinita)
        {
            Vector3 posMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            posMouse.z = 0f;
            Vector2 dir = (posMouse - firingPoint.position).normalized;
            balaScript.SetDirection(dir);
        }
        else
        {
            balaScript.SetTarget(target); 
        }
       
        if (!munInfinita)
        {
            munAtual--;

            ammoSlider.value = munAtual;



            if (munAtual <= 0)
            {
                if (torretaBuild.originTile != null)
                {
                    torretaBuild.originTile.isOccupied = false;
                    torretaBuild.originTile = null;

                }
                Destroy(gameObject);
            }
        }
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
