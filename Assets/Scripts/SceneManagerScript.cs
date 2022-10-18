using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;


public class SceneManagerScript : MonoBehaviour
{
    int dailyChallengeNum, level;

    private DateTime today, yesterday;
    private int randomDaySeed;

    [SerializeField] Vector2[] ogPos;
    [SerializeField] Vector2[] ogGroupPos;

    [SerializeField] AudioScript _audioScript;

    GameObject Music;

    public int[] challenges = {0,0,0};

    public Button[] Buttons;

    [SerializeField] bool[] buttonMoveStatus;

    [SerializeField] GameObject[] tutorials;

    string[] highscores = { "TandemHighScore", "BlastHighScore", "NormalHighScore", "ChallengeHighScore" };

    private void Awake()
    {
        Music = GameObject.Find("Music");
        _audioScript = GameObject.Find("Sound Controller").GetComponent<AudioScript>();

        if(SceneManager.GetActiveScene().name == "Menu Scene")
        {
            GetDate();
            GetScores();
            GetPoints();
        }

        if(PlayerPrefs.GetString("GameScene") == "VersusModeScene")
        {
            level = 0;
        }

        if(PlayerPrefs.GetString("GameScene") == "DiscoModeScene")
        {
            level = 1;
        }
    }

    private void Start()
    {
        for(int i = 0; i < Buttons.Length; i++)
        {
            ogPos[i] = Buttons[i].transform.position;
        }

        if(SceneManager.GetActiveScene().name == "Controls Scene" || SceneManager.GetActiveScene().name == "VS Controls Scene 1")
        {
            MoveGroup(GameObject.Find("Main"));
            MoveGroupLeft(GameObject.Find("Controls"));
        }

        if(SceneManager.GetActiveScene().name == "Cosmetics Scene")
        {
            MoveGroup(GameObject.Find("Skins"));
        }
    }

    void GetDate()
    {
        today = DateTime.Today;

        if(PlayerPrefs.HasKey("yesterday") == false)
        {
            yesterday = today;

            PlayerPrefs.SetString("yesterday", yesterday.ToString());
        } else
        {
            if(today.ToString() != PlayerPrefs.GetString("yesterday"))
            {
                

                for(int i = 0; i < challenges.Length; i++)
                {
                    challenges[i] = 1;
                    PlayerPrefs.SetInt("LvlChallenge " + i, challenges[i]);
                }

                yesterday = today;

                PlayerPrefs.SetString("yesterday", yesterday.ToString());
            } else
            {
                
                for (int i = 0; i < challenges.Length; i++)
                {
                    challenges[i] = PlayerPrefs.GetInt("LvlChallenge " + i);
                }
            }
        }
        
    }

    void GetScores()
    {
        //today = DateTime.Today;

        /*for (int i = 0; i < challenges.Length; i++)
        {
            if(PlayerPrefs.HasKey("LvlChallenge " + i))
            {
                print("yes key");
                challenges[i] = PlayerPrefs.GetInt("LvlChallenge " + i);
            } else
            {
                challenges[i] = 1;
                PlayerPrefs.SetInt("LvlChallenge " + i, challenges[i]);
            }
            
        }*/

        /*randomDaySeed = today.Day + today.Month * 10 + today.Year * 100;
        Random.InitState(randomDaySeed);

        dailyChallengeNum = (Random.Range(0, 3));

        PlayerPrefs.SetInt("dailyChallengeLvl", dailyChallengeNum);*/

        

        SetHighScores();

        for (int i = 0; i <= 2; i++)
        {
            if (challenges[i] == 1)
            {
                Buttons[i].transform.Find("DailyChallenge").gameObject.SetActive(true);

                
                    Buttons[i].transform.Find("DailyChallenge").GetComponent<Text>().text = "Daily Bonus";
            }
            else
            {
                //Buttons[i].transform.Find("DailyChallenge").GetComponent<Text>().text = "Complete";
                Buttons[i].transform.Find("DailyChallenge").gameObject.SetActive(false);
            }
        }
    }

    void GetPoints()
    {
        if (PlayerPrefs.HasKey("Points") == false)
        {
            PlayerPrefs.SetInt("Points", 0);
        } else
        {
            return;
        }
    }

    void SetHighScores()
    {
        for(int i = 0; i < highscores.Length; i++)
        {
            if (PlayerPrefs.HasKey(highscores[i]) == false)
            {
                PlayerPrefs.SetInt(highscores[i], 0);
            } else
            {
                print(highscores[i] + PlayerPrefs.GetInt(highscores[i]));
            }
        }
    }
    

    public void LoadScene(string scene)
    {

        if(scene == "Cosmetics Scene" || scene == "Menu Scene")
        {
            SceneManager.LoadScene(scene);
            
        } else
        {
            if(scene == "VersusModeScene" || scene == "DiscoModeScene")
            {
                SceneManager.LoadScene("VS Controls Scene 1");
            } else
            {
                SceneManager.LoadScene("Controls Scene");
            }
            
            PlayerPrefs.SetString("GameScene", scene);
        }

        DontDestroyOnLoad(Music.gameObject);
        
    }

    public void TutorialSelect()
    {
        tutorials[level].SetActive(true);
    }

    public void ResetPoints()
    {
        PlayerPrefs.SetInt("Points", 0);
        
    }

    public void ButtonSelect(int currentButton)
    {
        // ogPos = Buttons[currentButton].transform.position;
        
        if(buttonMoveStatus[currentButton] == false)
        {
            Buttons[currentButton].transform.DOMoveX(ogPos[currentButton].x + 50f, 0.3f);
            buttonMoveStatus[currentButton] = true;
        }
       
        _audioScript.PlayMenuAudio(0);
       
    }

    public void ButtonOff(int currentButton)
    {
        if(buttonMoveStatus[currentButton] == true)
        {
            Buttons[currentButton].transform.DOMoveX(ogPos[currentButton].x, 0.3f);
            buttonMoveStatus[currentButton] = false;
        }
        
    }

    public void MoveGroup(GameObject group)
    {
        group.transform.position = new Vector2(group.transform.position.x - 100f, group.transform.position.y);
        group.SetActive(true);
        group.transform.DOMoveX(group.transform.position.x + 100f, 0.5f);

        EventSystem.current.SetSelectedGameObject(group.transform.GetChild(0).gameObject);
    }

    public void MoveGroupLeft(GameObject group)
    {
        group.transform.position = new Vector2(group.transform.position.x + 100f, group.transform.position.y);
        group.SetActive(true);
        group.transform.DOMoveX(group.transform.position.x - 100f, 0.5f);

        EventSystem.current.SetSelectedGameObject(group.transform.GetChild(0).gameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
