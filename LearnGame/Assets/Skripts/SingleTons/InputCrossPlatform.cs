using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCrossPlatform : MonoBehaviour
{
    public static InputCrossPlatform Instance;
    private Camera cam;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    public bool IsCursorClickOnCanvas(out Vector2 output)
    {
        output = new Vector2();
        if (Input.touchCount == 1) output = Input.GetTouch(0).position;
        if (Input.GetMouseButton(0)) output = Input.mousePosition;
        return output != new Vector2();
    }
}
