using System.Collections.Generic;
using UnityEngine;
using TMPro; // Ensure you're using TextMeshPro for the input field and name display
using System.IO;
using System.Linq;
using UnityEngine.UI; // Make sure to include this namespace for UI components

[System.Serializable]
public class UserProfile
{
    public string userName;
    public int characterType; // Assuming 0 for girl, 1 for boy, etc.

    public UserProfile(string name, int character)
    {
        userName = name;
        characterType = character;
    }
}

public class ProfileManager : MonoBehaviour
{
    public TMP_InputField nameInputField; // Reference to the TextMeshPro input field
    public TMP_Dropdown profileDropdown; // Reference to the UI Dropdown component
    public TextMeshProUGUI nameText; // For displaying the selected profile's name
    public Image profileImage; // For displaying the profile picture
    public Sprite boyImage; // Assign in the inspector
    public Sprite girlImage; // Assign in the inspector

    public int currentProfileIndex = -1; // -1 means no profile is selected


    public List<UserProfile> profiles = new List<UserProfile>();
    private string profilesFilePath => $"{Application.persistentDataPath}/profiles.json";

    void Start()
    {
        LoadProfiles();
        nameInputField.gameObject.SetActive(false); // Initially hide the input field
        profileDropdown.gameObject.SetActive(false); // Initially hide the dropdown
        nameInputField.onEndEdit.AddListener(delegate { CreateProfile(nameInputField.text, 0); }); // Default character type as 0
        profileDropdown.onValueChanged.AddListener(delegate { SelectProfile(profileDropdown.value); });
    }

    public void ShowInputField(bool isEditMode)
    {
        nameInputField.gameObject.SetActive(true);
        nameInputField.ActivateInputField();
        nameInputField.Select();

        if (isEditMode && currentProfileIndex >= 0 && currentProfileIndex < profiles.Count)
        {
            nameInputField.text = profiles[currentProfileIndex].userName; // Pre-fill with current name if editing
            nameInputField.onEndEdit.RemoveAllListeners(); // Remove all previous listeners
            nameInputField.onEndEdit.AddListener(delegate { EditProfile(nameInputField.text); });
        }
        else
        {
            nameInputField.text = ""; // Clear the field if creating a new profile
            nameInputField.onEndEdit.RemoveAllListeners(); // Remove all previous listeners
            nameInputField.onEndEdit.AddListener(delegate { CreateProfile(nameInputField.text, 0); });
        }
    }


    public void CreateProfile(string name, int characterType = 0)
    {
        if (!string.IsNullOrEmpty(name))
        {
            UserProfile newProfile = new UserProfile(name, characterType);
            profiles.Add(newProfile);
            SaveProfiles();
            nameInputField.text = ""; // Clear the input field
            nameInputField.gameObject.SetActive(false); // Optionally hide the field
        }
    }

    private void UpdateDropdown()
    {
        profileDropdown.ClearOptions();  // Clears current options
        List<string> options = profiles.Select(profile => profile.userName).ToList();
        profileDropdown.AddOptions(options);  // Adds new options
    }

    public void ShowDropdown()
    {
        profileDropdown.gameObject.SetActive(true);  // Makes the dropdown visible
    }

    public void SelectProfile(int index)
    {
        if (index >= 0 && index < profiles.Count)
        {
            currentProfileIndex = index;
            PlayerPrefs.SetString("CurrentUserName", profiles[index].userName);
            PlayerPrefs.SetInt("CurrentUserCharacter", profiles[index].characterType);
            PlayerPrefs.Save();

            nameText.text = profiles[index].userName; // Update the name text
            profileImage.sprite = profiles[index].characterType == 0 ? girlImage : boyImage; // Update the profile picture based on character type

            profileDropdown.gameObject.SetActive(false); // Hide the dropdown after selection
        }
    }
    public void EditProfile(string newName)
    {
        if (currentProfileIndex >= 0 && currentProfileIndex < profiles.Count && !string.IsNullOrEmpty(newName))
        {
            profiles[currentProfileIndex].userName = newName; // Update the name in the profile
            SaveProfiles();

            nameText.text = newName; // Update the UI to reflect the new name

            nameInputField.text = ""; // Clear the input field
            nameInputField.gameObject.SetActive(false); // Optionally hide the field
            profileDropdown.captionText.text = newName; // Update dropdown display text
        }
    }


    public void DeleteProfile(int index)
    {
        if (currentProfileIndex >= 0 && currentProfileIndex < profiles.Count)
        {
            profiles.RemoveAt(currentProfileIndex);
            SaveProfiles();

            // Reset UI components after deleting a profile
            nameText.text = ""; // Clear the displayed name
            profileImage.sprite = null; // Clear the displayed image or set to a default image
            if (profiles.Count > 0 && currentProfileIndex < profiles.Count) {
                // Select next available profile or the last one if it was the last in list
                SelectProfile(currentProfileIndex);
            } else {
                // No profiles left or no valid selection possible
                currentProfileIndex = -1; // Reset current profile index
                profileDropdown.captionText.text = "Select Profile"; // Reset dropdown default text
                profileDropdown.ClearOptions(); // Clear dropdown options
            }
        }
    }

    private void SaveProfiles()
    {
        string json = JsonUtility.ToJson(new Serialization<UserProfile>(profiles), true);
        File.WriteAllText(profilesFilePath, json);
        UpdateDropdown(); // Ensure dropdown is updated whenever profiles are saved
    }

    private void LoadProfiles()
    {
        if (File.Exists(profilesFilePath))
        {
            string json = File.ReadAllText(profilesFilePath);
            profiles = JsonUtility.FromJson<Serialization<UserProfile>>(json).ToList();
            UpdateDropdown(); // Update the dropdown after loading profiles
        }
    }

    [System.Serializable]
    private class Serialization<T>
    {
        public List<T> items;
        public Serialization(List<T> items) { this.items = items; }
        public List<T> ToList() { return items; }
    }
}

