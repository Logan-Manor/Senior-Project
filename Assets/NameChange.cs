using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameChange : MonoBehaviour
{
     public TMP_InputField nameInputField;
    public TextMeshProUGUI displayNameText;
    // void Start()
    // {
    //     nameInputField = GetComponent<InputField>();
    // }

    public void OnNameButtonClick()
    {
        // Activate the input field's GameObject.
        nameInputField.gameObject.SetActive(true);
        nameInputField.ActivateInputField();
        nameInputField.Select(); // This focuses the input field for typing.
        nameInputField.onEndEdit.AddListener(SubmitName);
    }

    private void SubmitName(string submittedName)
    {
        displayNameText.text = submittedName;
        PlayerPrefs.SetString("PlayerName", submittedName); // Save the name
        PlayerPrefs.Save(); // Make sure to save PlayerPrefs
        HideInputField();
    }

    private void Start()
    {
        // Initialize the input field to be hidden at start.
        HideInputField();
         if (PlayerPrefs.HasKey("PlayerName"))
    {
        string playerName = PlayerPrefs.GetString("PlayerName");
        displayNameText.text = playerName; // Set the saved name
    }
    }

    private void HideInputField()
    {
        // Deactivate the input field's GameObject.
        nameInputField.gameObject.SetActive(false);
        nameInputField.onEndEdit.RemoveListener(SubmitName); // Ensure the listener is removed.
    }
        
    
}
