using UnityEngine;
using TMPro;

public class ShipHUD : MonoBehaviour
{
    [Header("UI References")]
    public GameObject hudRoot;
    public TextMeshProUGUI oreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI energyText;

    public void SetVisibility(bool isVisible)
    {
        hudRoot.SetActive(isVisible);
    }

    public void UpdateOre(int newOre)
    {
        oreText.text = "" + newOre;
    }

    public void UpdateHealth(int newHealth)
    {
        healthText.text = "HEALTH: " + newHealth;
    }

    public void UpdateEnergy(int newEnergy)
    {
        energyText.text = "ENERGY: " + newEnergy;
    }
}