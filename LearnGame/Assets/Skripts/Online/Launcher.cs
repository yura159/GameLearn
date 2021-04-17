using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;
    #region Private Serializable Fields

    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 4;
    [SerializeField]
    private string levelName = "Lobby";

    #endregion


    #region Private Fields

    private string gameVersion = "1";
    private bool isConnecting;
    private string findNameGame ;
    private string createNameGame;
    #endregion

    public void SetFindNameGame(string name)
    {
        findNameGame = name;
    }

    public void SetCreateNameGame(string name)
    {
        createNameGame = name;
    }

    void Awake()
    {
        Instance = this;
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (string.IsNullOrEmpty(findNameGame))
                PhotonNetwork.JoinRandomRoom();
            else
                PhotonNetwork.JoinRoom(findNameGame);
        }
        else
        {
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public void CreateRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.CreateRoom(createNameGame, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }
        else
        {
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("We load the " + levelName);
            PhotonNetwork.LoadLevel(levelName);
        }
    }
}
