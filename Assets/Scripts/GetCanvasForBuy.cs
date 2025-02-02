using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCanvasForBuy : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera camera;
    public Canvas canvas;

    void Start()
    {
        // Get the main camera
        camera = Camera.main;

        // Get the Canvas component
        canvas = transform.GetComponent<Canvas>();

        // Set the world camera for the Canvas
        if (canvas != null)
        {
            canvas.worldCamera = camera;
        }
        else
        {
            Debug.LogError("Canvas component not found on this GameObject.");
        }
    }
}
