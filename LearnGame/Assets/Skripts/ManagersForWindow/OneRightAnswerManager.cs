using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OneRightAnswerManager : Manager
{
    public Text Counter;
    public Text Question;
    public List<Text> Variants;
    public int rightAnswer;

    // Start is called before the first frame update
    void Start()
    {
        try {
            GetData();
        }
        catch
        {

        }
    }

    public new void GetData()
    {
        Question.text = Window.Instance.GetValue("question");
        for (int i = 0; i < Variants.Count; i++)
            Variants[i].text = Window.Instance.GetValue("variant_" + (i + 1));
        Counter.text = Window.Instance.GetValue("number");
        rightAnswer = int.Parse(Window.Instance.GetValue("rightAnswer"));
    }

    public void RegisterAnswer(int answer)
    {
        if (!Active) return;
        var images = Variants.Select(x => x.transform.parent.GetComponent<Image>()).ToList();
        images[answer - 1].color = Color.red;
        images[rightAnswer - 1].color = Color.green;
        Window.Instance.RegisterResult(answer == rightAnswer);
        StartCoroutine(exit());
        Active = false;
    }

    private IEnumerator exit()
    {
        yield return new WaitForSeconds(2);
        Window.Instance.CloseWindow();
    }
}
