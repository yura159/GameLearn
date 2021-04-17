using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MultiAnswersManager : Manager
{
    public Text Counter;
    public Text Question;
    public List<Text> Variants;

    private List<int> rightAnswers;
    // Start is called before the first frame update
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
        Question.text = Window.Instance.GetValue("question");
        for (int i = 0; i < Variants.Count; i++)
            Variants[i].text = Window.Instance.GetValue("variant_" + (i + 1));
        Counter.text = Window.Instance.GetValue("number");
        rightAnswers = Window.Instance.GetValue("rightAnswer")
            .Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(x => int.Parse(x))
            .ToList();
    }

    public void RegisterAnswer()
    {
        if (!Active) return;
        var images = Variants.Select(x => x.transform.parent.GetComponent<Image>()).ToList();
        rightAnswers
            .Where(x => images[x - 1].color == Color.yellow)
            .ToList()
            .ForEach(x => images[x - 1].color = Color.green);
        images
             .Where(x => x.color == Color.yellow)
            .ToList()
            .ForEach(x => x.color = Color.red);
        rightAnswers
            .Where(x => images[x - 1].color != Color.green)
            .ToList()
            .ForEach(x => images[x - 1].color = Color.red);
        var sucess = images.Where(x => x.color == Color.red).Count() == 0;
        Window.Instance.RegisterResult(sucess);
        StartCoroutine(exit());
    }

    private IEnumerator exit()
    {
        yield return new WaitForSeconds(2);
        Window.Instance.CloseWindow();
    }
}
