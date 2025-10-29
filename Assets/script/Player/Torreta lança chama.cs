using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;


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
    [Header("Interação PEM")]
    public static List<Torretalançachama> TodasTorretasChama = new List<Torretalançachama>();
    
    private bool desativadoPEM = false;
    private float tempoRotaçãoDesativada;
    private Quaternion rotaçãoNormal;
    private Quaternion rotaçãoPEM;
    private SpriteRenderer[] spriteRenderers;
    private Color corNormal;
    private Color corDesativado;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(raioChama <= 0)
        {
            Debug.Log("Valor do raio da chama muito baixo");
            raioChama = 1.5f;
        }
        TodasTorretasChama.Add(this);
        tempoRotaçãoDesativada = 1.5f;


        torretaBuild = GetComponent<Building>();

        enemyMask = settings.enemyMask;

        targetingRange = settings.targetingRange;
        rotationSpeed = settings.rotationSpeed;
        tempoDeVida = settings.tempoDeVida;

        munAtual = munMax;

        if(torretaBuild != null && torretaBuild.originTile != null)
        {
            SpriteRenderer tileSprite = torretaBuild.originTile.GetComponent<SpriteRenderer>();
            SpriteRenderer torretaSprite = GetComponent<SpriteRenderer>();
            if(tileSprite != null && torretaSprite != null)
            {
                torretaSprite.sprite = tileSprite.sprite;
            }
        }


        if (ammoUIPrefab != null)
        {
            GameObject uiObject = Instantiate(ammoUIPrefab, transform.position + Vector3.up, Quaternion.identity);
            uiObject.transform.SetParent(transform);
            ammoSlider = uiObject.GetComponentInChildren<Slider>();
            ammoSlider.maxValue = munMax;
            ammoSlider.value = munAtual;
        }


        rotaçãoNormal = turretRotationPoint.rotation;
        rotaçãoPEM = Quaternion.Euler(0, 0, -180);
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        if(spriteRenderers.Length > 0)
        {
            corNormal = spriteRenderers[0].color;
            corDesativado = new Color(corNormal.r * 0.5f, corNormal.g * 0.5f, corNormal.b * 0.5f, corNormal.a * 0.8f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (desativadoPEM || !podeAtirar) return;

        if (munAtual <= 0)
        {
            if (torretaBuild.originTile != null)
                torretaBuild.originTile.isOccupied = false;
            Destroy(gameObject);
            return;
        }

        if (!podeAtirar)
        {
            efeitoChama.SetActive(false);
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
                    inimigo.TomaDano(Mathf.CeilToInt(danoPorSegundo * Time.deltaTime));
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

    public void DesativarTorretaPEM()
    {
        if (desativadoPEM) return;

        desativadoPEM = true;
        podeAtirar = false;
        efeitoChama.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(RotacionarETrocarCor(rotaçãoPEM, corDesativado));
    }

    public void ReativarTorreta()
    {
        if (!desativadoPEM) return;
        desativadoPEM = false;
        StopAllCoroutines();
        StartCoroutine(RotacionarETrocarCor(rotaçãoNormal, corNormal, reativando: true));
    }
    

    IEnumerator RotacionarETrocarCor(Quaternion alvoRot, Color corAlvo, bool reativando = false)
    {
        float t = 0;
        Quaternion inicioRot = turretRotationPoint.localRotation;
        Color corInicial = spriteRenderers[0].color;

        while (t < 1f)
        {
            t += Time.deltaTime / tempoRotaçãoDesativada;
            turretRotationPoint.localRotation = Quaternion.Lerp(inicioRot, alvoRot, t);

            foreach (var sr in spriteRenderers)
            {
                sr.color = Color.Lerp(corInicial, corAlvo, t);
            }
            yield return null;
        }
        
        if(reativando)
        {
            podeAtirar = true;
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


    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(firingPoint.position, raioChama);
    }

    private void OnDestroy()
    {
        TodasTorretasChama.Remove(this);
    }
}
