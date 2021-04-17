using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseVariant : MonoBehaviour
{
    private bool flag = false;
    private Color startColor;
    // Start is called before the first frame update
    void Start()
    {
        startColor = GetComponent<Image>().color;
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (!flag)
            {
                GetComponent<Image>().color = Color.yellow;
                flag = true;
            }
            else
            {
                GetComponent<Image>().color = startColor;
                flag = false;
            }
        });
    }
}
