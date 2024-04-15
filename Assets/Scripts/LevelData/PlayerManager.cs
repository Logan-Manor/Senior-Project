using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI; // For buttons
using TMPro; // For TextMeshPro elements

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public List<Level> Levels;
    //public PlayerSettings playerSettings; // Assume this is a class you define

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLevelData(); // Load level data as soon as the LevelManager is initialized
            LoadCameraPosition();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }


    void LoadLevelData()
    {
        Levels = new List<Level>();

        string path = Path.Combine(Application.dataPath, "Scripts/LevelData/LevelDataFile.json");
        Debug.Log("Attempting to load level data from: " + path);
        string jsonStr = File.ReadAllText(path);
        Debug.Log(jsonStr);
        LevelList data = JsonUtility.FromJson<LevelList>(jsonStr);
        foreach (var level in data.levels)
        {
            Debug.Log($"Level: {level.LevelName}, BestTime immediately after deserialization: {level.BestTime}");
        }
        if (data == null)
        {
            Debug.LogError("Deserialization failed. Data is null.");
        }
        else if (data.levels == null)
        {
            Debug.LogError("Deserialization succeeded but levels list is null.");
        }
        else
        {
            foreach (var level in data.levels)
            {
                // Here you set a default value for BestTime if it is 0 (indicating it wasn't in the JSON)
                // Adjust this logic based on your needs, for example, using a specific indicator value
                if (level.BestTime == 0)
                {
                    // Set to a default value indicating absence, or keep it at 0 if that's your preference
                    level.BestTime = -1; // Example: using -1 to indicate "no time recorded"
                }
            }
            Debug.Log($"Successfully deserialized. Levels count: {data.levels.Count}");
        }
        Levels = data.levels;

        // Debug log to confirm list is populated
        Debug.Log("Levels loaded: " + Levels.Count);
    }

    public void LoadCameraPosition()
    {
        string path = Path.Combine(Application.dataPath, "Scripts/LevelData/CameraPosition.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            CameraPositionData data = JsonUtility.FromJson<CameraPositionData>(json);
            if (data != null) // Check if data is not null
            {
                // Apply the loaded camera position
                Camera.main.transform.position = new Vector3(data.x, data.y, data.z);
            }
            else
            {
                // Handle the case where data is null, e.g., set default values or log a message
                Debug.Log("No camera position data found. Using default camera position.");
            }
        }
    }
}