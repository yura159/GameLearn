using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewPlayers : MonoBehaviourPun
{
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        var names = GameManager.Instance.PlayersNames;
        var result = "";
        foreach(var name in names)
        {
            result += name;
            result += '\n';
        }
        text.text = result;
    }
}
