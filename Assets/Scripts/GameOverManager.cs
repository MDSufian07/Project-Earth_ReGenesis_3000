using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] Health playerHealth;
    [SerializeField] GameObject gameOverUI;

    private void Update()
    {
        if(playerHealth.currentHealth <= 0)
        {
            Gameover();
        }
    }

    private void Gameover()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0;
        
    }
}
