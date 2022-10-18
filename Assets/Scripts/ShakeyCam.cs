using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ShakeyCam : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;

    CinemachineBasicMultiChannelPerlin shakeCam;
   

    public float shakeAmp, shakeFreq;

    float turnFreq = 1.5f, zoomSpace = 0.1f;

    int endChoice = 1;

    float[] turnEnd = {0, -90f};

    public bool isTurning, isZooming;

    private void Start()
    {
        vCam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        shakeCam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "VersusModeScene")
        {
            vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.5f;
        } else
        {
            if (GetComponent<LevelBuilder>().chonks[0].transform.Find("Top").position.y > 2.6f)
            {
                vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.65f;
            }
            else
            {
                vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.58f; //0.58
            }
        }
        
    }

    private void Update()
    {
        BoogieStatus();

        

        /*if (Input.GetKeyDown(KeyCode.H))
        {
            //StartCoroutine("TurnIt");
            isTurning = true; 

            
            
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            vCam.m_Lens.Dutch = 0;
            //vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.39f;
        }*/

        if(isTurning)
        {
            TurnIt();
            
        }

        if(isZooming)
        {
            Zoom();
            
        }
    }

    void BoogieStatus()
    {
        shakeCam.m_AmplitudeGain = shakeAmp;
        shakeCam.m_FrequencyGain = shakeFreq;
    }

    public void Zoom()
    {
        vCam.m_Lens.OrthographicSize -= zoomSpace;

        if (vCam.m_Lens.OrthographicSize >= 10f || vCam.m_Lens.OrthographicSize <= 6f)
        {
            zoomSpace *= -1;

            isZooming = false;

            return;
        }
    }

    public IEnumerator ShakeIt(float amp)
    {
        shakeAmp = amp;
        shakeFreq = 1;

        yield return new WaitForSecondsRealtime(0.15f);

        shakeAmp = 0f;
        shakeFreq = 0f;

    }

    public void TurnIt()
    {
        
        vCam.m_Lens.Dutch -= turnFreq;
        

        if (vCam.m_Lens.Dutch >= 0 || vCam.m_Lens.Dutch <= -90)
        {
            turnFreq *= -1;

            

            isTurning = false;

            return;
        }
    }

}
