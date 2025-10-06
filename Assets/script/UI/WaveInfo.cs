/*using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class WaveInfo : MonoBehaviour
{
    [Header("Referencias de OBJ")]
    public GameObject painel;
    public Transform enemiesContainer;
    public GameObject enemyEntryPrefab;
    public TextMeshProUGUI titleText;

    [Header("Atributos")]

    public float displayTime = 45f;

    private Coroutine hideRoutine;

    public void ShowWaveInfo(WaveData wave)
    {
        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
        }

        foreach (Transform child in enemiesContainer)
        {
            Destroy(child.gameObject);
        }

        titleText.text = "NOVA WAVE";

        foreach (var spawnData in wave.inimigosWave)
        {
            GameObject entry = Instantiate(enemyEntryPrefab, enemiesContainer);
           UnityEngine.UI.Image icon = entry.transform.Find("Icon").GetComponent<UnityEngine.UI.Image>();
            TextMeshProUGUI countText = entry.transform.Find("CountText").GetComponent<TextMeshProUGUI>();

            icon.sprite = spawnData.inimigo.icone;
            countText.text = "x" + spawnData.qtd;
        }
        painel.SetActive(true);
        hideRoutine = StartCoroutine(HideAfterDelay());

    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        painel.SetActive(false);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


}*/

using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WaveInfo : MonoBehaviour
{
    [Header("Referências")]
    public GameObject painel;
    public Transform enemiesContainer; // atribuir no inspector (RectTransform)
    public GameObject enemyEntryPrefab;
    public TMP_Text titleText;

    [Header("Ajustes")]
    public float displayTime = 4f;

    private Coroutine hideRoutine;

    public void ShowWaveInfo(WaveData wave)
    {
        // segurança básica
        if (painel == null || enemiesContainer == null || enemyEntryPrefab == null || titleText == null)
        {
            Debug.LogError("WaveInfoUI: faltam referências no inspector! painel/enemiesContainer/enemyEntryPrefab/titleText");
            return;
        }

        // para evitar problemas com coroutines anteriores
        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        // limpa filhos de forma segura (iterando para trás)
        for (int i = enemiesContainer.childCount - 1; i >= 0; i--)
        {
            var go = enemiesContainer.GetChild(i).gameObject;
            // se estiver em modo editor e executando no editor, prefira DestroyImmediate; em runtime Destroy.
            if (Application.isPlaying)
                Destroy(go);
            else
                DestroyImmediate(go);
        }

        titleText.text = "NOVA WAVE";

        // popula
        foreach (var spawnData in wave.inimigosWave)
        {
            if (spawnData == null || spawnData.inimigo == null)
            {
                Debug.LogWarning("WaveInfoUI: spawnData ou spawnData.inimigo nulo em wave " + (wave != null ? wave.name : "null"));
                continue;
            }

            GameObject entry = Instantiate(enemyEntryPrefab, enemiesContainer);
            // tente achar os componentes com GetComponentInChildren para evitar problemas de caminho
            var icon = entry.GetComponentInChildren<UnityEngine.UI.Image>();
            var countText = entry.GetComponentInChildren<TMP_Text>();

            if (icon == null || countText == null)
            {
                Debug.LogError("EnemyEntry prefab precisa ter um Image e um TextMeshProUGUI como filhos diretos ou herdados.");
                Destroy(entry);
                continue;
            }

            icon.sprite = spawnData.inimigo.icone; // assume InimigoSettings tem 'Sprite icone'
            countText.text = "x" + spawnData.qtd;
        }

        painel.SetActive(true);
        hideRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        painel.SetActive(false);
    }
}
