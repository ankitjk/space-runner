using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject scoreObject;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverScreen;
    public TextMeshProUGUI finalScoreText;
    public GameObject spawnerObject;
    public GameObject wing;
    public GameObject body;
    public GameObject tail;
    public GameObject rearWing;
    public GameObject enemy;

    private float score;
    private bool isGameOver;

    void Start()
    {
        score = 0f;
        isGameOver = false;
    }

    void Update()
    {
        if (!isGameOver)
        {
            // update score
            score += Time.deltaTime;
            scoreText.text = score.ToString("F1");
        }
    }

    public void GameOver()
    {
        isGameOver = true;

        // deactivate player
        wing.SetActive(false);
        body.SetActive(false);
        tail.SetActive(false);
        rearWing.SetActive(false);

        // deactivate enemy
        enemy.SetActive(false);

        // deactvate obstacle spawner
        spawnerObject.SetActive(false);

        // stop showing score in the top left corner
        scoreObject.SetActive(false);

        // show game over screen
        gameOverScreen.SetActive(true);

        // display final score
        finalScoreText.text = "Final Score:\n" + score.ToString("F1");
    }

    public void RestartGame()
    {
        // reload the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
        // load the menu screen (which has buildIndex of 0)
        SceneManager.LoadScene(0);
    }

}
