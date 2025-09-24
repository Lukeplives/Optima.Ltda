using UnityEngine;
using TMPro;

public class UIcontroller : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI numFerro;
    [SerializeField] TextMeshProUGUI numCombustível;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }

    public void UpdatenumFerro()
    {
        numFerro.text = gameManager.QtdFerro.ToString();

    }

    public void UpdateNumComb()
    {
        numCombustível.text = gameManager.QtdComb.ToString();
    }


}
