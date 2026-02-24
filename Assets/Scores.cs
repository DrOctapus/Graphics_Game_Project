using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Scores : MonoBehaviour
{
    public int maxHealth = 50;
    public int maxEnergy = 100;

    public ShipHUD hud;
    public GameObject gameOverScreen;
    public TMP_Text finalScoreText;

    int health = 0;
    float energy = 0;
    int ore = 0;
    bool isDead = false;

    void Start()
    {
        health = maxHealth;
        energy = maxEnergy;
        hud.UpdateHealth(health);
        hud.UpdateEnergy(Mathf.FloorToInt(energy));
        hud.UpdateOre(ore);
        
        Time.timeScale = 1f;
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
    }

    public float energyDrainSpeed = 1.0f; // How much energy to lose per second

    private void Update()
    {
        if (isDead) return;

        ChangeEnergy(-energyDrainSpeed * Time.deltaTime);
    }
    public void ChangeHealth(int amount)
    {
        if (isDead) return;
        health += amount;
        if(health < 0)
        {
            health = 0;
            GameOver();
        }
        hud.UpdateHealth(health);
    }

    public void MaximHealth()
    {
        health = maxHealth;
        hud.UpdateHealth(health);
    }

    public void ChangeEnergy(float amount)
    {
        if (isDead) return;
        energy += amount;
        if(energy < 0)
        {
            energy = 0;
            GameOver();
        }
        hud.UpdateEnergy(Mathf.FloorToInt(energy));
    }

    public void MaximEnergy()
    {
        energy = maxEnergy;
        hud.UpdateEnergy(Mathf.FloorToInt(energy));
    }

    public void ChangeOre(int amount)
    {
        ore += amount;
        hud.UpdateOre(ore);
    }

    void GameOver()
    {
        isDead = true;
        finalScoreText.text = "Game Over\nFinal Score: " + ore.ToString();
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
