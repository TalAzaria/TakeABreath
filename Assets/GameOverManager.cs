using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    //[SerializeField] private GameObject gameOverUI; 

    public void EndGame()
    {
        Time.timeScale = 0;
        Debug.Log("game over");
        //gameOverUI.SetActive(true);
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