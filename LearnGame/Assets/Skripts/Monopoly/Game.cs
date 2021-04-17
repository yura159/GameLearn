using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviourPunCallbacks, IPunObservable
{
    public Text Win;
    public GameObject Cube;
    public static Game Instance;
    [NonSerialized]
    public bool isFirst = false;

    private List<MonopolyPlayer> players;
    private List<MonopolyItem> items;
    public Step currentStepPlayer;
    private Dictionary<MonopolyPlayer, Step> StepsInfo = new Dictionary<MonopolyPlayer, Step>();


    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        
        players = getPlayers();
        currentStepPlayer = new Step(players.Count);
        items = GetComponentsInChildren<MonopolyItem>()
            .ToList();
        for(int i = 0; i < players.Count; i++)
            StepsInfo.Add(players[i], new Step(items.Count));
    }

    private void Update()
    {
        foreach(var player in players)
        {
            var countItems = items.Where(x => x.GetColor() == player.GetColor()).Count();
            var allItems = items.Count;
            var percent = countItems / (float)allItems;
            if(percent >= 0.8f)
            {
                GetComponent<PhotonView>().RPC("WinGame", RpcTarget.All, player.GetName());
            }
        }
    }

    [PunRPC]
    private void WinGame(string namePlayer)
    {
        Win.gameObject.SetActive(true);
        Win.text = "Выиграл " + namePlayer + " !!!";
        Cube.SetActive(false);
    }

    private List<MonopolyPlayer> getPlayers()
    {
        var countPlayer = GameManager.Instance.PlayersNames.Count;
        var allPlayers = GetComponentsInChildren<MonopolyPlayer>()
            .ToList();
        allPlayers
            .Skip(countPlayer)
            .ToList()
            .ForEach(p => p.gameObject.SetActive(false));
        var names = GameManager.Instance.PlayersNames;
        for (int i = 0; i < countPlayer; i++)
        {
            var name = names[i];
            var player = allPlayers[i];
            player.SetName(name);
        }
        return allPlayers
            .Take(countPlayer)
            .ToList();
    }

    public void DeletePlayer(string PlayerName)
    {
        var player = players.Where(x => x.GetName() == PlayerName).First();
        players.Remove(player);
        foreach(var item in items)
        {
            if (item.GetColor() == player.GetColor())
                item.GetComponent<Image>().color = item.StartColor;
        }
        currentStepPlayer.size -= 1;
        if(players.Count == 1)
        {
            WinGame(players[0].GetName());
        }
        player.gameObject.SetActive(false);
    }

    public void MoveStep(int count)
    {
        var player = players[currentStepPlayer.currentStep];
        StartCoroutine(movePlayer(player, count));
    }

    private IEnumerator movePlayer(MonopolyPlayer player, int countStep)
    {
        var currentStep = StepsInfo[player];
        for (int i = 0; i < countStep; i++)
        {
            currentStep.NextStep();
            SetStep(player, currentStep);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);

        var onStart = items[currentStep.currentStep].GetComponent<MonopolyItem>().OnStart;
        if (onStart)
        {
            currentStepPlayer.NextStep();
            currentStep.currentStep = 0;
            SetStep(player, currentStep);
        }
        else
        {
            var monopolyWindow = (MonopolyWindow)Window.Instance;

            var indexItem = currentStep.currentStep;
            var item = items[indexItem];
            var itemColor = item.GetColor();
            var PlayerColor = player.GetColor();
            var playersForAnswer = new List<string>() { player.GetName() };
            if (itemColor == PlayerColor)
            {
                currentStepPlayer.NextStep();
                yield break;
            }

            if (itemColor != item.StartColor)
            {
                var otherPlayer = players.Where(x => x.GetColor() == itemColor).First();
                playersForAnswer.Add(otherPlayer.GetName());
            }
            monopolyWindow.StartPlay("Monopoly", playersForAnswer);
        }
    }

    public void TransferOnCurrentPlayer()
    {
        GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.PlayerList[currentStepPlayer.currentStep]);
    }

    private void SetStep(MonopolyPlayer player, Step currentStep)
    {
        var indexItem = currentStep.currentStep;
        var item = items[indexItem];
        var piece = player.GetPiece();
        piece.SetActive(true);
        piece.transform.position = item.transform.position;
    }

    public void RegisterAnswer(string namePlayer)
    {//вызывается только при правильном ответе
        if (!isFirst) return;
        var player = players.Where(x => x.GetName() == namePlayer).First();
        var indexItem = StepsInfo[player].currentStep;
        var item = items[indexItem];
        var itemColor = item.GetColor();
        var PlayerColor = player.GetColor();
        if (itemColor != item.StartColor)
        {
            if (itemColor != PlayerColor)
            {
                var otherPlayer = players.Where(x => x.GetColor() == itemColor).First();
                otherPlayer.GetScore().Decrement();
                player.GetScore().Increment();
                item.GetComponent<Image>().color = PlayerColor;
            }
        }
        else
        {
            player.GetScore().Increment();
            item.GetComponent<Image>().color = PlayerColor;
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentStepPlayer.currentStep);
            foreach (var step in StepsInfo)
            {
                stream.SendNext(step.Value.currentStep);
            }
        }
        else
        {
            this.currentStepPlayer.currentStep = (int)stream.ReceiveNext();
            foreach (var step in StepsInfo)
            {
                step.Value.currentStep = (int)stream.ReceiveNext();
            }
        }
    }

    public bool IsMyStep(string myName)
    {
        var idPlayer = currentStepPlayer.currentStep;
        var player = players[idPlayer];
        return player.GetName() == myName;
    }

    public class Step
    {
        public int currentStep { get; set; }

        public int size { get; set; }
        public Step(int size)
        {
            this.size = size;
            currentStep = 0;
        }

        public void NextStep()
        {
            currentStep += 1;
            if (currentStep == size) currentStep = 0;
        }

        public void DecrementSize()
        {
            size -= 1;
            if (currentStep == size) currentStep = 0;
        }
    }
}
