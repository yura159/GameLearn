using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCube : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    private List<State> states;

    [NonSerialized]
    public bool isActive = true;

    private State currentState;
    private Image image;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentState.countStep);
        }
        else
        {
            var countStep = (int)stream.ReceiveNext();
            currentState = states.Where(x => x.countStep == countStep).First();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = states[0];
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (Game.Instance.IsMyStep(PhotonNetwork.NickName) && isActive)
            {
                isActive = false;
                Game.Instance.TransferOnCurrentPlayer();
                StartCoroutine(StartCast());
            }
        });
        image = GetComponent<Image>();
    }

    private void Update()
    {
        image.sprite = currentState.SpriteState;
    }

    private IEnumerator StartCast()
    {
        for(int i = 0; i < 2000; i += 25)
        {
            var randIndex = UnityEngine.Random.Range(0, 6);
            currentState = states[randIndex];
            yield return new WaitForSeconds(i / 2000);
        }
        Game.Instance.MoveStep(currentState.countStep);
    }

    [Serializable]
    public class State
    {
        public Sprite SpriteState;
        public int countStep;
    }
}
