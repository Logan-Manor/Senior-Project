using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class imgswap : MonoBehaviour
{
    public Image profileImage; // Assign the UI Image component here in the inspector
    public Sprite boyImage; // Assign the boy's sprite here in the inspector
    public Sprite girlImage; // Assign the girl's sprite here in the inspector
    private bool isBoyImage = true; // Default is boy image

    void Start()
    {
        // Load the character type from PlayerPrefs
        isBoyImage = PlayerPrefs.GetInt("CharacterType", 1) == 1; // Default to 1 (boy)
        profileImage.sprite = isBoyImage ? boyImage : girlImage;
    }

    // This method is called to switch the images
    public void SwitchImage()
    {
        isBoyImage = !isBoyImage; // Toggle the boolean
        profileImage.sprite = isBoyImage ? boyImage : girlImage;
        
        // Save the new state to PlayerPrefs
        PlayerPrefs.SetInt("CharacterType", isBoyImage ? 1 : 0);
        PlayerPrefs.Save(); // Ensure the data is saved immediately
    }
}
