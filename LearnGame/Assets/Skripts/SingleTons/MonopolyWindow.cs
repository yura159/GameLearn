using Photon.Pun;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonopolyWindow : Window
{
    private List<string> namesPlayers;

    public void StartPlay(string path, List<string> players)
    {
        this.namesPlayers = players;
        var jsonFile =  Resources.Load("Jsons\\" + path).ToString();
        var info = JSON.Parse(jsonFile);
        windows = info["windows"];

        curent = Random.Range(0, windows.Count);
        var result = "";
        foreach (var name in namesPlayers) result += name + "+";

        GetComponent<PhotonView>().RPC("CreateWindow", RpcTarget.All, windows.ToString(), curent, result);
    }

    //public static new MonopolyWindow Instance;
    protected void Awake()
    {
        Instance = this;
    }

    [PunRPC]
    private void CreateWindow(string strWindows, int curent, string strNamesPlayers)
    {
        windows = JSON.Parse(strWindows);
        this.curent = curent;
        namesPlayers = strNamesPlayers.Split(new char[] { '+' }, System.StringSplitOptions.RemoveEmptyEntries).ToList();

        if (strNamesPlayers.Contains(PhotonNetwork.NickName)) Game.Instance.isFirst = true;

        var window = windows[curent]["window_" + curent];
        var type = RemoveQuote(window["type"]);
        var prefab = (GameObject)Resources.Load("Windows\\" + type);
        var wind = Instantiate(prefab, transform);
        var manager = wind.GetComponentInChildren<Manager>();
        if (!namesPlayers.Contains(PhotonNetwork.NickName))
        {
            manager.Active = false;
        }
    }

    [PunRPC]
    private void DestroyWindow()
    {
        Game.Instance.Cube.GetComponent<ButtonCube>().isActive = true;
        if (transform.childCount != 0)
            Destroy(transform.GetChild(0).gameObject);
    }

    private void Update()
    {
        
    }

    [PunRPC]
    private void NotFirst()
    {
        Game.Instance.isFirst = false;
    }

    public override void RegisterResult(bool res)
    {
        if (res == true)
        {
            var name = PhotonNetwork.NickName;
            var player = namesPlayers.Where(x => x == name).First();
            Game.Instance.RegisterAnswer(player);
            GetComponent<PhotonView>().RPC("NotFirst", RpcTarget.All);
        }
    }

    public override void CloseWindow()
    {
        if (namesPlayers[0] == PhotonNetwork.NickName)
        {
            Game.Instance.currentStepPlayer.NextStep();
            GetComponent<PhotonView>().RPC("DestroyWindow", RpcTarget.Others);
        }
        DestroyWindow();
    }
}
