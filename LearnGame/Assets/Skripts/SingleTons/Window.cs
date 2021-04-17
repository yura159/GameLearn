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
        
    }

    private void Update()
    {
        if (isActiveWindow()) oneStep = true;
        if (windows != null && !isActiveWindow() && oneStep)
        {
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

    public virtual void CloseWindow()
    {
        if (transform.childCount != 0)
            Destroy(transform.GetChild(0).gameObject);
        windows = null;
        curent = -1;
    }

    public virtual void RegisterResult(bool res)
    {
        if (res == true) result += 1;
    }
}
