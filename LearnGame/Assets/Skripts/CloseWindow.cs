using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseWindow : MonoBehaviour
{
    public bool next = false;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Window.Instance.CloseWindow(next);
        });
    }
}
