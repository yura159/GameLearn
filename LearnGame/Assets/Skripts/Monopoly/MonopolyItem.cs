using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonopolyItem : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool OnStart = false;
    public Color StartColor;

    private void Start()
    {
        StartColor = GetColor();
    }

    public Color GetColor()
    {
        return GetComponent<Image>().color;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            var color = GetColor();
            stream.SendNext(color.r);
            stream.SendNext(color.g);
            stream.SendNext(color.b);
        }
        else
        {
            // Network player, receive data
            var image = this.GetComponent<Image>();
            var r = (float)stream.ReceiveNext();
            var g = (float)stream.ReceiveNext();
            var b = (float)stream.ReceiveNext();
            image.color = new Color(r, g, b);
        }
    }
}
