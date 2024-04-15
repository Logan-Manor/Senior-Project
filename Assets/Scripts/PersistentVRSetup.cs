using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentVRSetup : MonoBehaviour
{
    private static PersistentVRSetup instance;
    public string[] scenesToExclude; // Add scene names here via the Inspector

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(gameObject); // Make the whole prefab persistent
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Ensure only one instance of the VR setup exists
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the current scene should exclude the persistent VR setup
        foreach (var sceneName in scenesToExclude)
        {
            if (scene.name == sceneName)
            {
                gameObject.SetActive(false); // Disable the VR setup for this scene
                return; // Exit the loop and method early
            }
        }

        // If the scene is not in the exclusion list, make sure the VR setup is active
        gameObject.SetActive(true);
    }

    public Transform GetCameraTransform()
    {
        // Assuming the camera is directly attached to this GameObject or one of its children
        return GetComponentInChildren<Camera>(true).transform;
    }

    public static PersistentVRSetup Instance { get { return instance; } }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
