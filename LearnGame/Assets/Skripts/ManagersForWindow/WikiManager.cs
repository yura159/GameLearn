using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WikiManager : Manager
{
    public Text Question;
    public Text Link;
    public Button Url;

    private string result;
    void Start()
    {
        try
        {
            GetData();
        }
        catch
        {

        }
    }

    public new void GetData()
    {
        var question = Window.Instance.GetValue("question");
        var link = Window.Instance.GetValue("link");
        result = Window.Instance.GetValue("answer");
        Question.text = question;
        Link.text = link;
        Url.onClick.AddListener(() =>
        {
            Application.OpenURL(link);
        });
    }

    public void CheckResult(string str)
    {
        if (!Active) return;
        if(str == result)
        {
            Window.Instance.RegisterResult(true);
            Question.text = "Правильно!";
            StartCoroutine(exit());
        }
    }

    private IEnumerator exit()
    {
        yield return new WaitForSeconds(2);
        Window.Instance.CloseWindow();
    }
}
