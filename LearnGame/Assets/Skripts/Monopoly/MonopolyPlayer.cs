using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonopolyPlayer : MonoBehaviour
{
    public Color GetColor()
    {
        return GetComponentInChildren<ColorPlayer>().GetColor();
    }

    public Score GetScore()
    {
        return GetComponentInChildren<Score>();
    }

    public GameObject GetPiece()
    {
        return GetComponentInChildren<PlayerPiece>().gameObject;
    }

    public string GetName()
    {
        return GetComponentInChildren<NamePlayer>().GetComponent<Text>().text;
    }

    public void SetName(string name)
    {
        GetComponentInChildren<NamePlayer>().GetComponent<Text>().text = name;
    }
}
