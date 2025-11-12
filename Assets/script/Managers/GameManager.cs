using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Submarino")]
    public Submarino submarinoData;
    
    [Header("Recursos jogador")]

    public int QtdFerro;
    public float QtdComb;

    public int ferroCritico = 15;
    public float combCritico = 500;
    [SerializeField] private float decrementoComb;
    public bool gameover;

    [SerializeField] private LayerMask layerItens;
    
    public event Action OnTileStateChanged;

    [Header("SubManagers")]
    public BuildManager buildManager;
    public UIManager uiManager;
    public WaveManager waveManager;
    public HoverManager hoverManager;


    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }
    void Start()
    {
        
        if (SceneManager.GetActiveScene().name == "MainScene")
        {

            if(waveManager == null)
            {
                FindFirstObjectByType<WaveManager>();
            }
            
        }

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            QtdComb += 1000;
            QtdFerro += 1000;
            uiManager.AtualizarRecursosHUD();
        }
        if (submarinoData != null)
        {
            AtualizarCombustível();

            uiManager.AtualizarRecursosHUD();

            if (QtdComb <= 0)
            {
                buildManager.customCursor.gameObject.SetActive(false);
                Cursor.visible = true;
                uiManager.ExibirGameOver();
                if (submarinoData != null)
                {
                    Destroy(submarinoData.gameObject);
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (uiManager.isPaused)
                {
                    uiManager.AlternarPause();
                }
                else
                {
                    uiManager.AlternarPause();
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (uiManager.isPaused)
                uiManager.AlternarPause();
            else
                uiManager.AlternarPause();
        }

        DetectarCliqueItem();
    }
    

    private void AtualizarCombustível()
    {
        if (QtdComb > 0)
        {
            QtdComb -= decrementoComb * Time.deltaTime;

        }
        else
        {
            QtdComb = 0;
        }

        uiManager.AtualizarRecursosHUD();
        uiManager.AtualizarIndicadores(QtdFerro, QtdComb);
    }

    public void BuyBuilding(Building building)
    {
        if (QtdFerro >= building.custoRec && QtdComb >= building.custoComb)
        {
            QtdFerro -= building.custoRec;
            QtdComb -= building.custoComb;

            uiManager.AtualizarRecursosHUD();
            uiManager.AtualizarIndicadores(QtdFerro, QtdComb);

            buildManager.StartBuilding(building);
        }
    }

    public void PlayerDead(GameObject player)
    {
        if (gameover) return;
        gameover = true;
        DisablePlayerComponents(player);

        Time.timeScale = 0f;

        uiManager.MostrarGameOver(true);
    }

    void DisablePlayerComponents(GameObject player)
    {
        MonoBehaviour[] scriptsPlayer = player.GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour script in scriptsPlayer)
        {
            script.enabled = false;
        }

        Collider2D[] collidersPlayer = player.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in collidersPlayer)
        {
            collider.enabled = false;
        }

        Rigidbody2D[] rbPlayer = player.GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D rb in rbPlayer)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        SpriteRenderer[] spritesPlayer = player.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprites in spritesPlayer)
        {
            sprites.enabled = false;
        }
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void AlternarModoTiro()
    {
        foreach (var torretas in TorretaBasica.TodasTorretas)
        {
            if (torretas == null) continue;
            if (torretas.munInfinita)
            {
                torretas.AlternarControleManual();
            }
        }
    }

    public void NotificarMudançaTile()
    {
        OnTileStateChanged?.Invoke();
    }

    public void DanoAoPlayer(float dano)
    {
        QtdComb -= dano;
        if (QtdComb <= 0)
        {
            QtdComb = 0;
            PlayerDead(submarinoData.gameObject);
        }
    }

    private void DetectarCliqueItem()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerItens);

            if(hit.collider != null)
            {
                Item item = hit.collider.GetComponent<Item>();

                if(item != null)
                {
                    item.Coletar();
                }
            }
        }
    }
    


}
