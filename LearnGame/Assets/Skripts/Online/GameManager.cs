using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameManager Instance;

    
    public List<string> PlayersNames = new List<string>();
    private bool game = false;
    private void Awake()
    {
        Instance = this;   
    }

    public void StartGame()
    {
        GetComponent<PhotonView>().RPC("SetGameOnTrue", RpcTarget.All);
    }

    [PunRPC]
    private void SetGameOnTrue()
    {
        game = true;
    }

    private void Start()
    {
        var photonView = GetComponent<PhotonView>();
        PlayersNames.Add(photonView.Owner.NickName);
        DontDestroyOnLoad(gameObject);
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Main");
        Destroy(gameObject);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        //if(PlayersNames.Contains(other.NickName)) other.
        PlayersNames.Add(other.NickName);
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        if (game && other.NickName != PhotonNetwork.NickName)
        {
            Game.Instance.DeletePlayer(other.NickName);
        }
        PlayersNames.Remove(other.NickName);
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && PhotonNetwork.IsMasterClient)
        {
            var result = "";
            foreach(var name in PlayersNames)
            {
                result += name + "+";
            }
            stream.SendNext(result);
        }
        else if(stream.IsReading && !PhotonNetwork.IsMasterClient)
        {
            var result = (string)stream.ReceiveNext();
            PlayersNames = result.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
