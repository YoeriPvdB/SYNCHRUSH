using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject startButton;
    [SerializeField] int roomSize;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        startButton.SetActive(true);
    }

    public  void QuickStart()
    {
        PhotonNetwork.JoinRandomRoom();

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to Join room, will create one");
        CreateRoom();
    }

    public void CreateRoom()
    {
        Debug.Log("Creating room");
        int randomRoomNum = Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom("Room " + randomRoomNum, roomOptions);
        Debug.Log(randomRoomNum);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room.");
        CreateRoom();
    }


}
