using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public CreatureOxygen playerOxygen;
    public GameObject spawnPoint;
    private Vector2 originalPosition;
    CapsuleCollider2D collisionCollider;
    PlayerMovement playerMovement;

    public Canvas UI;

    private float wibbleAmount = 0.3f;
    private float wibbleDuration = 0.1f;
    private float wibbleSpeed = 2f;

    private void Start()
    {
        // Time.timeScale = 0;
        playerMovement = this.GetComponent<PlayerMovement>();
        playerMovement.enabled = false;

        collisionCollider = this.GetComponent<CapsuleCollider2D>();
        collisionCollider.enabled = false;

        originalPosition = spawnPoint.transform.position;
        this.transform.position = originalPosition;

        StartCoroutine(WibbleAndMoveDownEffect());

        playerOxygen.OnDepleted += OnPlayerOxygenDepleted;

        UI.enabled = false;
    }

    void StartGame()
    {
        //Time.timeScale = 1;
        this.GetComponent<PlayerMovement>().enabled = true;
        collisionCollider.enabled = true;
        this.GetComponent<CreatureOxygen>().Levels = 100;
        UI.enabled = true;
    }

    private void OnPlayerOxygenDepleted(GameObject player)
    {
        EndGame();
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        Debug.Log("game over");
        NpcsManager.Instance.OnEndgame();
        MainUI.Instance.OnEndgame();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1; 
        Application.Quit();
    }


    IEnumerator WibbleAndMoveDownEffect()
    {
        float elapsedTime = 0f;
        float moveDuration = 2f;
        Vector2 targetPosition = originalPosition - new Vector2(0, 5);

        while (elapsedTime < moveDuration)
        {
            float wibbleOffset = Mathf.Sin(Time.time * wibbleSpeed) * wibbleAmount;
            transform.position = Vector2.Lerp(originalPosition, targetPosition, elapsedTime / moveDuration);
            transform.position = new Vector2(transform.position.x + wibbleOffset, transform.position.y);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        StartGame();
    }
}