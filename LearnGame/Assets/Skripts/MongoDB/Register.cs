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
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (WorkMogoDb.Connection())
            {
                if(WorkMogoDb.CheckIn(login.text, password.text))
                {
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
