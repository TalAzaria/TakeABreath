using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public CreatureOxygen playerOxygen;

    private void Start()
    {
        playerOxygen.OnDepleted += OnPlayerOxygenDepleted;
    }

    private void OnPlayerOxygenDepleted(GameObject player)
    {
        EndGame();
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        Debug.Log("game over");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1; // Resume the game
        Application.Quit(); // Quit the application (does not work in the editor)
    }
}