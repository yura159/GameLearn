using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPlayer : MonoBehaviour
{
    public Color GetColor()
    {
        return GetComponent<Image>().color;
    }
}
