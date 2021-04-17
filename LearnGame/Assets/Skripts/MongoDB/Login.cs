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
    void Start()
    {
        if (PlayerPrefs.HasKey(playerNamePrefKey)) login.text = PlayerPrefs.GetString(playerNamePrefKey);
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (WorkMogoDb.Connection())
            {
                if(WorkMogoDb.SigIn(login.text, password.text))
                {
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
