using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMODUnity;

public class UIScript : MonoBehaviour
{
    public Text modeText, timeText, addTimeText, rushText, R, U, S, H, startText, goalText, awardText, popupText;

    public Slider swapSlider, botSlider;

    public PlayerSwitch _playerSwitch;

    PlayerMovement _moveScript;

    public float cdNum, leveltime = 0, textOpacity = 1f, rushOpacity = 0f, textScale = 1, popUpOpacity;

    public bool isCounting;

    Color timeAlpha;

    public Color uiColour;

    Transform timeTextTransform;

    public AudioScript _audioScript;

    PauseScript _pauseScript;

    GameObject Player;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        _moveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        _pauseScript = GameObject.Find("Menu Canvas").GetComponent<PauseScript>();

        timeTextTransform = addTimeText.gameObject.transform;

        timeTextTransform.position = addTimeText.gameObject.transform.position;

        rushOpacity = 0;

        textOpacity = 0;

        popUpOpacity = 0;

        timeText.enabled = false;

        StartCoroutine("Countdown");
    }

    IEnumerator Countdown()
    {
        

        float startTimer = 3f;

        _audioScript.PlayP1Audio(4);

        while (startTimer > 0.8f)
        {

            startTimer -= Time.deltaTime;

            startText.text = "" + Mathf.RoundToInt(startTimer);

            yield return null;
        }

        if(startTimer <= 0.8f)
        {

            if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "VersusModeScene")
            {

                _audioScript.musicEmitter1.enabled = true;
                startText.text = "SYNCH/RUSH";
                StartCoroutine("Pop", startText.gameObject);
                yield return new WaitForSecondsRealtime(1f);


                timeText.enabled = true;

                GameObject.Find("Ambience 1").GetComponent<StudioEventEmitter>().enabled = true;

                // _moveScript.speed = 1300f;
                _moveScript.speed = _moveScript.normalSpeed;

                if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "DiscoModeScene")
                {
                    GameObject.FindGameObjectWithTag("Player 2").GetComponent<Player2Script>().p2Speed = 1150f;
                    GameObject.Find("Follow Object").GetComponent<VSCamScript>().canMove = true;
                }
                _audioScript.StartCoroutine("PlayMusic");
            } 

            startText.enabled = false;
        }

    }

    public void  SetGoals(string[] levelAwards, float[] levelTimes)
    {
        goalText.text = levelAwards[0] + " : " + Mathf.RoundToInt(levelTimes[0]) + "s \n \n" +
            levelAwards[1] + " : " + Mathf.RoundToInt(levelTimes[1]) + "s \n\n" +
            levelAwards[2] + " : " + Mathf.RoundToInt(levelTimes[2]) + "s \n\n" +
            levelAwards[3] + " : " + Mathf.RoundToInt(levelTimes[3]) + "s";
    }

    private void Update()
    {
        Timer();

        addTimeText.color = new Color(addTimeText.color.r, addTimeText.color.g, addTimeText.color.b, textOpacity);

        if(popupText != null)
        {
           popupText.color = new Color(popupText.color.r, popupText.color.g, popupText.color.b, popUpOpacity);
        }

        uiColour = new Color(uiColour.r, uiColour.g, uiColour.b, rushOpacity);

        if(timeText.enabled == false && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "VersusModeScene")
        {
            _moveScript.speed = 1;
        }
    }

    public void Timer()
    {
        if (_pauseScript.isPaused == false && startText.enabled == false)
        {
            leveltime += Time.deltaTime;
        } 
        
        timeText.text = "" + Mathf.RoundToInt(leveltime);
    }

    public void Counter()
    {
        if (isCounting)
        {            
            swapSlider.gameObject.SetActive(true);
            //swapSlider.maxValue = cdNum;
            cdNum -= Time.deltaTime;
            swapSlider.value = cdNum;
            
        }
        else
        {
            swapSlider.gameObject.SetActive(false);
            
        }
    }

    public IEnumerator ModeSwap()
    {
        modeText.enabled = true;

        if(_playerSwitch.inTandem)
        {
            modeText.text = "TANDEM " + " ON";

        } else
        {
            modeText.text = "TANDEM " + " OFF";
        }

        

        yield return new WaitForSecondsRealtime(2f);

        modeText.enabled = false;
    }

    public IEnumerator AddTimeText(int num)
    {
        addTimeText.gameObject.transform.position = new Vector2(timeText.gameObject.transform.position.x + 0.5f, timeText.gameObject.transform.position.y - 1f);
        addTimeText.enabled = true;
        textOpacity = 1f;

        addTimeText.text = "+ " + num;

        leveltime += num;

        while (textOpacity > 0)
        {
            addTimeText.gameObject.transform.Translate(transform.up * Time.deltaTime);
            textOpacity -= Time.deltaTime;
            yield return null;

            //
        }

        if(textOpacity<= 0)
        {
            
            addTimeText.gameObject.transform.position = new Vector2(timeText.gameObject.transform.position.x + 0.5f, timeText.gameObject.transform.position.y - 1f);
            addTimeText.enabled = false;
            
        }
    }

    public IEnumerator TextPopUp(string text)
    {
        popupText.gameObject.transform.position = new Vector2(Player.transform.position.x + 0.5f, Player.transform.position.y);
        popupText.enabled = true;
        popUpOpacity = 1f;

        popupText.text = text;

        while (popUpOpacity > 0)
        {
            popupText.gameObject.transform.Translate(transform.up * Time.deltaTime);
            popUpOpacity -= Time.deltaTime;
            yield return null;

            //
        }

        if (popUpOpacity <= 0)
        {

            popupText.gameObject.transform.position = new Vector2(timeText.gameObject.transform.position.x + 0.5f, timeText.gameObject.transform.position.y - 1f);
            popupText.enabled = false;

        }
    }

    IEnumerator Rush(Text rushText)
    {
        //rushText.color = new Color(rushText.color.r, rushText.color.g, rushText.color.g, rushOpacity);

        //rushOpacity = 0;

        while (rushOpacity < 1)
        {
            rushText.color = uiColour;
            rushText.gameObject.transform.Translate(Vector2.up * Time.deltaTime);
            rushOpacity += Time.deltaTime;

            yield return null;

            
        }

        rushOpacity = 0;
    }

    IEnumerator Pop (GameObject ting)
    {
        float popTime = 1;

        
        while (popTime > 0)
        {
            textScale += (0.001f * 360f) * Time.deltaTime;

            //ting.transform.localScale = new Vector2(textScale, textScale);
            ting.transform.position -= new Vector3(0, 0, 0.05f);

            popTime -= Time.deltaTime;

            yield return null;
        }
    }

    

    
}
