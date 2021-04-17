using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Linq;
using Photon.Pun;
using System.Text.Json;

public class Window : MonoBehaviourPunCallbacks
{
    public static Window Instance;

    protected JSONNode windows;
    protected int curent = -1;
    protected bool oneStep = true;
    protected int result = 0;
    private int count;
    private const string playerNamePrefKey = "PlayerName";
    private bool first = true;

    private Dictionary<string, int> regionsInfo = new Dictionary<string, int>
    {
        {"Comunication", 0 },
        {"Creation", 0 },
        {"Defence", 0 },
        {"Problems", 0 },
        {"Information", 0 },
        {"Curent", 0 }
    };
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path">путь до файла без расширения</param>
    public void StartPlay(string path)
    {
        WorkMogoDb.Connection();
        var dataInDatabase = WorkMogoDb.GetData(path, "");
        var jsonFile = dataInDatabase is null ? Resources.Load("Jsons\\" + path).ToString() : dataInDatabase;
        var info = JSON.Parse(jsonFile);
        windows = info["windows"];

        var playerName = PlayerPrefs.GetString(playerNamePrefKey);
        regionsInfo = WorkMogoDb.GetPlayerData(playerName, regionsInfo);
        curent = regionsInfo["Curent"];
    }

    private void Update()
    {
        if (isActiveWindow()) oneStep = true;
        if (windows != null && !isActiveWindow() && oneStep)
        {
            first = true;
            count = windows.Count;
            curent += 1;
            if (curent < windows.Count)
            {
                var window = windows[curent]["window_" + curent];
                var type = RemoveQuote(window["type"]);
                var prefab = (GameObject)Resources.Load("Windows\\" + type);
                Instantiate(prefab, transform);
                oneStep = false;
            }
            else
            {
                windows = null;
                curent = -1;
                regionsInfo["Curent"] = curent;
                SaveInfo();
            }
        }
    }

    protected static string RemoveQuote(JSONNode value)
    {
        return value
            .ToString()
            .Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries)
            .First();
    }

    public string GetValue(string key)
    {
        var window = windows[curent]["window_" + curent];
        var data = window[key];
        return RemoveQuote(data);
    }

    protected bool isActiveWindow()
    {
        return transform.childCount != 0;
    }

    public virtual void CloseWindow(bool next = true)
    {
        if (transform.childCount != 0)
            Destroy(transform.GetChild(0).gameObject);
        if (!next)
        {
            windows = null;
            regionsInfo["Curent"] = curent;
            SaveInfo();
            curent = -1;
        }
    }

    public virtual void RegisterResult(bool res)
    {
        if (res == true)
        {
            result += 1;
            UpdateRegion();
        }
    }

    private void SaveInfo()
    {
        var playerName = PlayerPrefs.GetString(playerNamePrefKey);
        WorkMogoDb.SetPlayerData(playerName, regionsInfo);
    }

    private void UpdateRegion()
    {
        if (!first) return;
        var window = windows[curent]["window_" + curent];
        var region = RemoveQuote(window["region"]);
        if (region != null)
        {
            var percent = regionsInfo[region];
            var allCount = GetCountRegionItem(region);
            var realCount = (int)Math.Round(allCount / 100.0 * percent, 0);
            realCount += 1;
            var newPercent = realCount * 100 / allCount ;
            regionsInfo[region] = newPercent;
        }
        first = false;
    }

    public int GetCountRegionItem(string region)
    {
        var count = 0;
        for(int i = 0; i < windows.Count; i++)
        {
            var window = windows[i]["window_" + i];
            var reg = RemoveQuote(window["region"]);
            if(reg != null)
            {
                if (region == reg) count++;
            }
        }
        return count;
    }
}
