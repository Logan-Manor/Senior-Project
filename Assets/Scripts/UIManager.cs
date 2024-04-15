using UnityEngine;

public class UIManager : MonoBehaviour
{
    public CanvasGroup[] canvasGroups; // Array to hold all canvases
    private int currentCanvasIndex = 0; // Track the current visible canvas

    private void Start()
    {
        // Initialize visibility of canvases
        ShowOnlyCurrentCanvas();
    }

    // Method to show only the current canvas based on the index
    private void ShowOnlyCurrentCanvas()
    {
        for (int i = 0; i < canvasGroups.Length; i++)
        {
            if (i == currentCanvasIndex)
            {
                canvasGroups[i].alpha = 1;
                canvasGroups[i].blocksRaycasts = true;
                canvasGroups[i].interactable = true;
            }
            else
            {
                canvasGroups[i].alpha = 0;
                canvasGroups[i].blocksRaycasts = false;
                canvasGroups[i].interactable = false;
            }
        }
    }

    // Public method to be called by UI buttons to cycle through canvases
    public void NextCanvas()
    {
        currentCanvasIndex = (currentCanvasIndex + 1) % canvasGroups.Length;
        ShowOnlyCurrentCanvas();
    }

    // Optional: Add method to go to a specific canvas directly
    public void GoToCanvas(int index)
    {
        if (index >= 0 && index < canvasGroups.Length)
        {
            currentCanvasIndex = index;
            ShowOnlyCurrentCanvas();
        }
    }
}


