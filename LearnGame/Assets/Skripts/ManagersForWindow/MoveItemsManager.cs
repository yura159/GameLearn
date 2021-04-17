using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MoveItemsManager : Manager
{
    public Text Counter;
    public Text Question;
    public List<GameObject> Items;
    public List<GameObject> Boxes;
    public List<Text> Texts;

    private string rightAnswer;

    private Transform startParent;
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
        Counter.text = Window.Instance.GetValue("number");
        rightAnswer = Window.Instance.GetValue("rightAnswer");
        for (int i = 0; i < Items.Count; i++)
            FillData(i);

        startParent = Items[0].transform.parent;
    }

    private void FillData(int index)
    {
        Texts[index].text = Window.Instance.GetValue("text_" + (index + 1));
        Items[index].GetComponentInChildren<Text>().text = Window.Instance.GetValue("item_" + (index + 1));
    }

    // Update is called once per frame
    void Update()
    {
        if (!Active) return;
        var countMoveItems = Items.Where(x => x.transform.parent == startParent).Count();
        if(countMoveItems == 0)
        {
            var tuples = rightAnswer.Split(' ').Select(x => parsePare(x)).ToList();
            foreach(var tup in tuples)
            {
                var indexItem = tup.Item1 - 1;
                var indexBox = tup.Item2 - 1;
                var realParent = Items[indexItem].transform.parent.gameObject;
                var rightParent = Boxes[indexBox];
                var item = Items[indexItem];
                if (realParent == rightParent)
                {
                    item.GetComponent<Image>().color = Color.green;
                }
                else
                {
                    item.GetComponent<Image>().color = Color.red;
                }
            }
            Items.ForEach(x => x.GetComponent<ItemMove>().enabled = false);
            var sucess = Items.Where(x => x.GetComponent<Image>().color == Color.red).Count() == 0;
            Window.Instance.RegisterResult(sucess);
            StartCoroutine(exit());
        }
    }

    private Tuple<int, int> parsePare(string pare)
    {
        var parse = pare.Split('-');
        var first = int.Parse(parse[0]);
        var second = int.Parse(parse[1]);
        return Tuple.Create(first, second);
    }

    private IEnumerator exit()
    {
        yield return new WaitForSeconds(2);
        Window.Instance.CloseWindow();
    }
}
