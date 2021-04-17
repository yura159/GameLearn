using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
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
        if (PlayerPrefs.HasKey(playerNamePrefKey)) login.text = PlayerPrefs.GetString(playerNamePrefKey);
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (WorkMogoDb.Connection())
            {
                if(WorkMogoDb.SigIn(login.text, password.text))
                {
                    try
                    {
                        WorkMogoDb.GetPlayerData(login.text, regionsInfo);
                    }
                    catch
                    {
                        WorkMogoDb.SetPlayerData(login.text, regionsInfo);
                    }
                    PlayerPrefs.SetString(playerNamePrefKey, login.text);
                    SceneManager.LoadScene("Main", LoadSceneMode.Single);
                }
                else
                {
                    massage.text = "неверный логин или пароль";
                }
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
