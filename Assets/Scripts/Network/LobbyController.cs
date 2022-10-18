using Photon.Pun;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    [SerializeField] Image player1Img, player2Img;
    

    private void Start()
    {
        Debug.Log("Joined Room" + PhotonNetwork.CurrentRoom);
        GetPlayers();
    }

    void GetPlayers() {

        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            player1Img.enabled = true;
        } else
        {
            player1Img.enabled = true;
            player2Img.enabled = true;
        }
        
        
    }


    public void LoadLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel(11);
        }
    }
    
}
