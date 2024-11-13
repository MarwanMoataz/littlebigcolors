using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverSceneManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI leaderboardText;
    public GameObject nextLevelButton;
    public GameObject retryButton;
    public GameObject mainMenuButton;

    private int score;
    private string currentLevel;

    void Start()
    {
        // Load score and level data from PlayerPrefs
        score = PlayerPrefs.GetInt("Score");
        currentLevel = PlayerPrefs.GetString("CurrentLevel"); // Retrieve the current level name
        if (string.IsNullOrEmpty(currentLevel))
        {
            Debug.LogError("CurrentLevel is not set correctly.");
            return;  // Exit if currentLevel is empty
        }
        // Display score and level
        scoreText.text = "Score: " + score;
        levelText.text = "Level: " + currentLevel;
        DisplayLeaderboard(currentLevel);


        void DisplayLeaderboard(string levelName)
        {
            // Get the top 3 scores for the current level from PlayerPrefs
            int score1 = PlayerPrefs.GetInt(levelName + "_Score1", 0);
            int score2 = PlayerPrefs.GetInt(levelName + "_Score2", 0);
            int score3 = PlayerPrefs.GetInt(levelName + "_Score3", 0);

            // Display the leaderboard for the level
            leaderboardText.text = "Top Scores for " + levelName + ":\n";
            leaderboardText.text += "1. " + score1 + "\n";
            leaderboardText.text += "2. " + score2 + "\n";
            leaderboardText.text += "3. " + score3 + "\n";
        }



        // Show next level button if there is a next level
        int nextLevelNum;
        if (int.TryParse(currentLevel.Substring(5), out nextLevelNum))  // Safely parse the level number
        {
            string nextLevel = "Level" + (nextLevelNum + 1).ToString();
            if (Application.CanStreamedLevelBeLoaded(nextLevel))  // Check if next level exists
            {
                nextLevelButton.SetActive(true);
            }
            else
            {
                nextLevelButton.SetActive(false);  // Hide if no next level
            }
        }
        else
        {
            Debug.LogError("Failed to parse the level number from the current level.");
        }
    }

    public void RetryLevel()
    {
        // Reload the current level by name
        SceneManager.LoadScene(PlayerPrefs.GetString("CurrentLevel"));
    }

    public void NextLevel()
    {
        // Increment the level name and load the next level
        int nextLevelNum = int.Parse(currentLevel.Substring(5)) + 1; // Extract number from "LevelX"
        string nextLevel = "Level" + nextLevelNum.ToString();

        if (Application.CanStreamedLevelBeLoaded(nextLevel)) // Ensure the next level exists
        {
            PlayerPrefs.SetString("CurrentLevel", nextLevel); // Save next level in PlayerPrefs
            SceneManager.LoadScene(nextLevel); // Load the next level
        }
        else
        {
            Debug.Log("No more levels available. Returning to main menu.");
            GoToMainMenu(); // If no more levels, return to main menu
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Load main menu (replace with your actual main menu scene name)
    }
}
