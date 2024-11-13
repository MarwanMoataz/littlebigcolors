using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Ensure you have this namespace for TextMeshPro
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    public GameObject levelButtonPrefab;  // Prefab for the level buttons
    public Transform buttonContainer;     // The container where the buttons will be placed
    public TMP_Text buttonTextPrefab;     // A reference to a TMP_Text prefab (for the level names)

    private void Start()
    {
        LoadAvailableLevels();
    }

    void LoadAvailableLevels()
    {
        int levelCount = 2; // Adjust the number of levels accordingly

        // Loop through the levels and instantiate buttons
        for (int i = 1; i <= levelCount; i++)
        {
            // Instantiate a button from the prefab and set it as a child of the container
            GameObject levelButton = Instantiate(levelButtonPrefab, buttonContainer);

            // Get the TextMeshProUGUI component of the button to set its text
            TMP_Text buttonText = levelButton.GetComponentInChildren<TMP_Text>();

            // Set the button text to match the level number (e.g., "Level 1", "Level 2")
            buttonText.text = "Level " + i;

            // Add a listener to load the level when the button is clicked
            int levelIndex = i;  // Local copy to avoid closure issue in loop
            levelButton.GetComponent<Button>().onClick.AddListener(() => LoadLevel(levelIndex));
        }
    }

    void LoadLevel(int levelIndex)
    {
        string levelName = "Level" + levelIndex;
        SceneManager.LoadScene(levelName);  // Load the selected level by name
    }
}
