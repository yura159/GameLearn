using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviourPunCallbacks, IPunObservable
{
    private int count;
    private void Start()
    {
        var text = GetComponent<Text>().text;
        count = int.Parse(text);
    }

    public void Increment()
    {
        count += 1;
        WriteScore();
    }

    public void Decrement()
    {
        count -= 1;
        WriteScore();
    }

    private void WriteScore()
    {
        GetComponent<Text>().text = count.ToString();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(count);
        }
        else
        {
            // Network player, receive data
            this.count = (int)stream.ReceiveNext();
            WriteScore();
        }
    }
}
