using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    public List<Text> progresses;
    public List<Image> images;

    private Dictionary<string, int> info = new Dictionary<string, int>
    {
        {"Comunication", 0 },
        {"Creation", 0 },
        {"Defence", 0 },
        {"Problems", 0 },
        {"Information", 0 }
        //{"Curent", 0 }
    };
    private const string playerNamePrefKey = "PlayerName";
    void Start()
    {
        if (WorkMogoDb.Connection())
        {
            var playerName = PlayerPrefs.GetString(playerNamePrefKey);
            var data = WorkMogoDb.GetPlayerData(playerName, info);
            var counter = 0;
            foreach(var val in data.Values)
            {
                progresses[counter].text += " " + val;
                calculateSizeForImage(images[counter], val);
                counter += 1;
            }
        }
    }

    private void calculateSizeForImage(Image image, int percent)
    {
        var trnsfrm = image.GetComponent<RectTransform>();
        var height = trnsfrm.rect.height;
        var newHeight = trnsfrm.rect.height * percent / 100.0f;
        var size = image.transform.localScale.y;
        var posY = image.transform.localPosition.y;
        var newSize = size * percent / 100.0f;
        var newPosY = (height - newHeight) / 2.0f;
        image.transform.localPosition -= new Vector3(0, newPosY);
        image.transform.localScale -= new Vector3(0, size - newSize);
    }
}
