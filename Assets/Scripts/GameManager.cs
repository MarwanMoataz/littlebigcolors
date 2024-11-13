using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI timerText;
    public GameObject requiredColorSphere; // Display required color as sphere

    public int score = 0;
    private Color currentRequiredColor;

    public float timeRemaining = 10f;
    private bool gameOver = false;

    private int highScore = 0;
    private string currentLevel = "Level1";

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            currentLevel = PlayerPrefs.GetString("CurrentLevel");
        }
        SetNewRequiredColor();

        UpdateScore(0);

    }

    private void Update()
    {
        if (gameOver) return;

        timeRemaining -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Ceil(timeRemaining).ToString();

        if (timeRemaining <= 0)
        {
            EndGame(false); // End game if time runs out
        }
    }

    public void UpdateScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }

   public void CollectColor(Color collectedColor)
{
    if (gameOver) return;

    if (collectedColor == currentRequiredColor)
    {
        UpdateScore(10);
        AudioManager.instance.PlayNextCollectibleSound(); // Play next sound in sequence
    }
    else
    {
        UpdateScore(-5);
        AudioManager.instance.PlayWrongCollectibleSound(); // Play wrong sound and reset sequence
    }

    SetNewRequiredColor();
}

    void SetNewRequiredColor()
    {
        Color[] colors = { Color.red, Color.green, Color.blue };
        currentRequiredColor = colors[Random.Range(0, colors.Length)];

        // Change the color of the UI Image to match the required color
        requiredColorSphere.GetComponent<Image>().color = currentRequiredColor;
    }

    public void EndGame(bool win)
    {
        gameOver = true;
        

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        // Pass the score and level to the GameOver scene
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetString("CurrentLevel", SceneManager.GetActiveScene().name);  // Save the current level name

        SceneManager.LoadScene("GameOverScene"); // Load the GameOver scene


        UpdateLeaderboard(SceneManager.GetActiveScene().name, score);


    }

    void UpdateLeaderboard(string levelName, int newScore)
    {
        // Get the current top 3 scores for the current level from PlayerPrefs
        int score1 = PlayerPrefs.GetInt(levelName + "_Score1", 0);
        int score2 = PlayerPrefs.GetInt(levelName + "_Score2", 0);
        int score3 = PlayerPrefs.GetInt(levelName + "_Score3", 0);

        // Update the leaderboard for the level (sort the scores)
        if (newScore > score1)
        {
            PlayerPrefs.SetInt(levelName + "_Score3", score2);
            PlayerPrefs.SetInt(levelName + "_Score2", score1);
            PlayerPrefs.SetInt(levelName + "_Score1", newScore);
        }
        else if (newScore > score2)
        {
            PlayerPrefs.SetInt(levelName + "_Score3", score2);
            PlayerPrefs.SetInt(levelName + "_Score2", newScore);
        }
        else if (newScore > score3)
        {
            PlayerPrefs.SetInt(levelName + "_Score3", newScore);
        }

        // Save the updated leaderboard for the level
        PlayerPrefs.Save();
    }

    public void RetryLevel()
    {
        // Reload the current level by its name
        SceneManager.LoadScene(PlayerPrefs.GetString("CurrentLevel"));
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current level
    }

    public void NextLevel()
    {
        int nextLevelNum = int.Parse(currentLevel.Substring(5)) + 1; // Extract number from "LevelX" and increment
        currentLevel = "Level" + nextLevelNum.ToString();

        // Save the next level name in PlayerPrefs
        PlayerPrefs.SetString("CurrentLevel", currentLevel);

        SceneManager.LoadScene(currentLevel); // Load the next level by name
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0); // Load the main menu scene (assumed to be index 0 in build settings)
    }
}
