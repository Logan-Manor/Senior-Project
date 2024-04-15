using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class CameraPositionAdjuster : MonoBehaviour
{
    public TMP_InputField xInputField, yInputField, zInputField;
    public Button leftButton, rightButton, upButton, downButton, forwardButton, backButton;
    public float adjustmentStep = 0.1f;

    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform; // Retrieve the main camera's transform

        // Setup button click listeners
        leftButton.onClick.AddListener(() => AdjustPosition(-adjustmentStep, 0, 0));
        rightButton.onClick.AddListener(() => AdjustPosition(adjustmentStep, 0, 0));
        upButton.onClick.AddListener(() => AdjustPosition(0, adjustmentStep, 0));
        downButton.onClick.AddListener(() => AdjustPosition(0, -adjustmentStep, 0));
        forwardButton.onClick.AddListener(() => AdjustPosition(0, 0, adjustmentStep));
        backButton.onClick.AddListener(() => AdjustPosition(0, 0, -adjustmentStep));

        // Setup input field listeners
        xInputField.onEndEdit.AddListener(delegate { UpdatePositionFromInput('x'); });
        yInputField.onEndEdit.AddListener(delegate { UpdatePositionFromInput('y'); });
        zInputField.onEndEdit.AddListener(delegate { UpdatePositionFromInput('z'); });

        // Initialize input fields based on the current camera position
        UpdateInputFieldsFromPosition();
    }

    void AdjustPosition(float xAmount, float yAmount, float zAmount)
    {
        cameraTransform.position += new Vector3(xAmount, yAmount, zAmount);
        UpdateInputFieldsFromPosition();
        SaveCameraPosition(); // Save after adjustment
    }

    void UpdatePositionFromInput(char axis)
    {
        float value = 0;
        bool valueParsed = false;
        switch (axis)
        {
            case 'x':
                valueParsed = float.TryParse(xInputField.text, out value);
                if (valueParsed)
                    cameraTransform.position = new Vector3(value, cameraTransform.position.y, cameraTransform.position.z);
                break;
            case 'y':
                valueParsed = float.TryParse(yInputField.text, out value);
                if (valueParsed)
                    cameraTransform.position = new Vector3(cameraTransform.position.x, value, cameraTransform.position.z);
                break;
            case 'z':
                valueParsed = float.TryParse(zInputField.text, out value);
                if (valueParsed)
                    cameraTransform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, value);
                break;
        }
        if (valueParsed) SaveCameraPosition(); // Save after manual input adjustment
    }

    void UpdateInputFieldsFromPosition()
    {
        xInputField.text = cameraTransform.position.x.ToString("F2");
        yInputField.text = cameraTransform.position.y.ToString("F2");
        zInputField.text = cameraTransform.position.z.ToString("F2");
    }

    public void SaveCameraPosition()
    {
        CameraPositionData data = new CameraPositionData()
        {
            x = cameraTransform.position.x,
            y = cameraTransform.position.y,
            z = cameraTransform.position.z
        };

        string path = Path.Combine(Application.dataPath, "Scripts/LevelData/CameraPosition.json");
        File.WriteAllText(path, JsonUtility.ToJson(data));
        Debug.Log($"Saved Camera Position to {path}");
    }
}

