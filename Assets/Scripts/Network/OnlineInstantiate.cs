using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class OnlineInstantiate : MonoBehaviour
{
    PhotonView myPV;
    GameObject playerAvatar, zuum;


    private void Awake()
    {
        myPV = GetComponent<PhotonView>();
        Debug.Log(myPV.ViewID);
        if(!PhotonNetwork.IsMasterClient)
        {
            playerAvatar = PhotonNetwork.InstantiateRoomObject(Path.Combine("Prefabs", "TestSquare"), Vector3.zero, Quaternion.identity);
        }
        zuum = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        playerAvatar.transform.position = zuum.transform.position;
    }
}
