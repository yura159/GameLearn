using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConversationScene : MonoBehaviour
{
    public string Scene;
    void Start()
    {
        var btn = GetComponent<Button>();
        if (btn == null) Debug.Log(name + "требует наличие кнопки для скрипта");
        btn.onClick.AddListener(() =>
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(Scene, LoadSceneMode.Additive);
        });
    }
}
