using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadarWave : MonoBehaviour
{
    [Header("Referencias de Texto")]
    public GameObject radarPanel;
    public GameObject voadorUI;
    public GameObject terrestreUI;
    public GameObject grandeUI;
    public GameObject kamikazeUI;

    public TextMeshProUGUI qtdVoador;
    public TextMeshProUGUI qtdTerrestre;
    public TextMeshProUGUI qtdGrande;
    public TextMeshProUGUI qtdKamikaze;

        int totalVoador = 0;
        int totalGrande = 0;
        int totalTerrestre = 0;
        int totalKamikaze = 0;

    public void AtualizarRadar(WaveData wave)
    {
        totalVoador = 0;
        totalGrande = 0;
        totalTerrestre = 0;
        totalKamikaze = 0;

        foreach (var spawnData in wave.inimigosWave)
        {
            switch (spawnData.tipoInimigo)
            {
                case InimigoSettings.tipoInimigo.Terrestre:
                    totalTerrestre++;
                    break;
                case InimigoSettings.tipoInimigo.Voador:
                    totalVoador++;
                    break;
                case InimigoSettings.tipoInimigo.Grande:
                    totalGrande++;
                    break;
                case InimigoSettings.tipoInimigo.Kamikaze:
                    totalKamikaze++;
                    break;
            }

        }

        AtualizarUI();
        voadorUI.SetActive(totalVoador > 0);
        kamikazeUI.SetActive(totalKamikaze > 0);
        grandeUI.SetActive(totalGrande > 0);
        terrestreUI.SetActive(totalTerrestre > 0);
    }

    void AtualizarUI()
    {
        qtdVoador.text = $"x{totalVoador.ToString()}";
        qtdTerrestre.text = $"x{totalTerrestre.ToString()}";
        qtdGrande.text = $"x{totalGrande.ToString()}";
        qtdKamikaze.text = $"x{totalKamikaze.ToString()}";

    }
    
    public void RemoverInimigo(InimigoSettings.tipoInimigo tipo)
    {
        
        switch (tipo)
        {
            case InimigoSettings.tipoInimigo.Terrestre:
                totalTerrestre = Mathf.Max(0,totalTerrestre - 1);
                break;
            case InimigoSettings.tipoInimigo.Voador:
                totalVoador = Mathf.Max(0, totalVoador -1);
                break;
            case InimigoSettings.tipoInimigo.Grande:
                totalGrande = Mathf.Max(0, totalGrande - 1);
                
                break;
            case InimigoSettings.tipoInimigo.Kamikaze:
                totalKamikaze = Mathf.Max(0, totalKamikaze - 1);
                break;
        }
        AtualizarUI();


    }
}
