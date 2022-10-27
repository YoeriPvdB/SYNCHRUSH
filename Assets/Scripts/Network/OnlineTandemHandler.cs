using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineTandemHandler : MonoBehaviourPun
{

    private void Awake()
    {
        if(!photonView.IsMine)
        {

        }
    }

    public void CheckOwner()
    {
        if(!photonView.IsMine)
        {
            SwitchOwner();
        }
    }

    public void SwitchOwner()
    {
        base.photonView.RequestOwnership();

        
    }
    
}
