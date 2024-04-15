using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI; // For buttons
using TMPro; // For TextMeshPro elements

public class LevelManager : MonoBehaviour
{
    public Button[] levelButtons; // Assign this array in the Inspector
    public Image original;

    public TMP_Text levelNameText;
    public TMP_Text bronzeTimeText;
    public TMP_Text silverTimeText;
    public TMP_Text goldTimeText;
    public TMP_Text Difficulty;
    public GameObject detailPanel; // The panel that will display the level details
    public Button play; // Activate and deactivate play button

    //private List<Level> levels; // Assuming Level and LevelList classes are already defined

    // UI images for stars
    public Image bronzeStarImage;
    public Image silverStarImage;
    public Image goldStarImage;

    // Star Sprites
    public Sprite emptyStar;
    public Sprite bronzeStar;
    public Sprite silverStar;
    public Sprite goldStar;

    void Start()
    {
        InitializeButtons();
        UpdateDetailPanel(0);
    }


    void InitializeButtons()
    {
        Debug.Log("Initializing buttons, count: " + levelButtons.Length);
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int index = i; // Local copy for use in lambda expression
            if (levelButtons[i] != null) // Check if the button is not null
            {
                levelButtons[i].onClick.AddListener(() => UpdateDetailPanel(index));
            }
            else
            {
                Debug.LogWarning("Button at index " + i + " is not assigned.");
            }
        }
    }

    public void UpdateDetailPanel(int levelIndex)
    {
        // Accessing selected level from PlayerManager
        var levels = PlayerManager.Instance.Levels;
        if (levelIndex < 0 || levelIndex >= levels.Count) return;

        var selectedLevel = levels[levelIndex];

        // Assuming all UI components were correctly assigned in the Inspector
        levelNameText.text = selectedLevel.LevelName;
        //string imagepath = Path.Combine(Application.dataPath, selectedLevel.ImagePath);
        Sprite imageToLoad = Resources.Load<Sprite>(selectedLevel.ImagePath);
        if (imageToLoad != null)
        {
            original.sprite = imageToLoad;
        }
        else
        {
            Debug.LogError("Image not found");
        }


        bronzeTimeText.text = $"Bronze: {selectedLevel.BronzeTime}s";
        silverTimeText.text = $"Silver: {selectedLevel.SilverTime}s";
        goldTimeText.text = $"Gold: {selectedLevel.GoldTime}s";
        Difficulty.text = selectedLevel.Difficulty;
        play.interactable = selectedLevel.IsUnlocked;

        //Debug.Log(Difficulty);
        Debug.Log(selectedLevel.BestTime);

        float bestTime = selectedLevel.BestTime;

        // Reset stars to empty at the start
        bronzeStarImage.sprite = emptyStar;
        silverStarImage.sprite = emptyStar;
        goldStarImage.sprite = emptyStar;


        Debug.Log(selectedLevel.GoldTime + " " + selectedLevel.BestTime);
        // Update stars based on best time
        if (bestTime <= selectedLevel.GoldTime)
        {
            // If best time is less than or equal to gold time, all stars are filled
            bronzeStarImage.sprite = bronzeStar;
            silverStarImage.sprite = silverStar;
            goldStarImage.sprite = goldStar;
        }
        else if (bestTime <= selectedLevel.SilverTime)
        {
            // If best time is less than or equal to silver time, fill bronze and silver stars
            bronzeStarImage.sprite = bronzeStar;
            silverStarImage.sprite = silverStar;
        }
        else if (bestTime <= selectedLevel.BronzeTime)
        {
            // If best time is less than or equal to bronze time, fill only the bronze star
            bronzeStarImage.sprite = bronzeStar;
        }
        else
        {
            // If BestTime is null, reset all stars to empty
            bronzeStarImage.sprite = emptyStar;
            silverStarImage.sprite = emptyStar;
            goldStarImage.sprite = emptyStar;
        }


        //Debug.Log(selectedLevel.LevelName);


        detailPanel.SetActive(true); // Show the panel with updated information
    }
}

