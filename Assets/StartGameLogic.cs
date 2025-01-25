using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameLogic : MonoBehaviour
{
    public GameObject leaderboards;
    public GameObject credits;

    private void Start()
    {
        credits.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("KfirScene");
    }

    public void LeaderboardsReveal()
    {
        leaderboards.SetActive(true);
    }

    public void ExitLeadboards()
    {
        leaderboards.SetActive(false);
    }
    public void EnterCredits()
    {
        credits.SetActive(true);
    }
    public void ExitCredits()
    {
        credits.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
