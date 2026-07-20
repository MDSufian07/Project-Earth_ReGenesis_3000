using UnityEngine;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] Health playerHealth;
    [SerializeField] GameObject gameOverUI;

    private void Update()
    {
        if(playerHealth.currentHealth <= 0)
        {
           Invoke(nameof(Gameover), .3f);
        }
    }

    private void Gameover()
    {
        gameOverUI.SetActive(true);
    }
}
