using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

using System.Collections;
public class PlayerNameInputField : MonoBehaviour
{
    private const string playerNamePrefKey = "PlayerName";

    void Start()
    {
        string defaultName = string.Empty;
        var inputField = GetComponent<InputField>();
        if (inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                inputField.text = defaultName;
            }
        }
        PhotonNetwork.NickName = defaultName;
    }

    public void SetPlayerName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = value;
        //PlayerPrefs.SetString(playerNamePrefKey, value);
    }
}
