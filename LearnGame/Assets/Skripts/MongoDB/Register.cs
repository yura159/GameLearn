using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public InputField login;
    public InputField password;
    public Text massage;

    private const string playerNamePrefKey = "PlayerName";
    private Dictionary<string, int> regionsInfo = new Dictionary<string, int>
    {
        {"Comunication", 0 },
        {"Creation", 0 },
        {"Defence", 0 },
        {"Problems", 0 },
        {"Information", 0 },
        {"Curent", 0 }
    };
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (WorkMogoDb.Connection())
            {
                if(WorkMogoDb.CheckIn(login.text, password.text))
                {
                    WorkMogoDb.SetPlayerData(login.text, regionsInfo);
                    PlayerPrefs.SetString(playerNamePrefKey, login.text);
                    SceneManager.LoadScene("Main", LoadSceneMode.Single);
                }
                else
                {
                    massage.text = "такой пользователь уже зарегистрирован";
                }
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
