using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            gameObject.SetActive(false);
        }
        GetComponent<Button>().onClick.AddListener(() =>
        {
            var countPlayer = GameManager.Instance.PlayersNames.Count;
            if(countPlayer >= 2)
            {
                PhotonNetwork.LoadLevel("Monopoly");
                PhotonNetwork.CurrentRoom.IsOpen = false;
                GameManager.Instance.StartGame();
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
