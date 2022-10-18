using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject tutorialMenu, EndMenu, EndBackground;

    public MenuState menuState;

    public Text playerScoreText, bestScoreText, endRushText, endR, endU, endS, endH, pointsText;

    public List<Text> gameRushText = new List<Text>();

    UIScript uiScript;

    TimeScript _timeScript;

    string[] highscores = { "TandemHighScore", "BlastHighScore", "NormalHighScore", "ChallengeHighScore" };
    string[] scenes = { "TandemModeScene", "BlastModeScene", "NormalModeScene", "EndlessModeScene" };

    public float EndScore;

    public int HighScore;

    int totalPoints;

    string currentSceneScore;

    bool endState = false;

    public AudioScript _audioScript;

    PointsManager _pointsScript;

    private void Start()
    {
        totalPoints = 0;
        menuState = MenuState.Off;

        uiScript = GameObject.Find("UI Handler").GetComponent<UIScript>();
        _timeScript = GameObject.FindGameObjectWithTag("Player").GetComponent<TimeScript>();
        _pointsScript = GetComponent<PointsManager>();

        pointsText = GameObject.Find("Points Text").GetComponent<Text>();

        EndMenu.SetActive(false);
        GetHighScores();
    }

    void GetHighScores()
    {
        for(int i = 0; i < highscores.Length; i++)
        {
            if(SceneManager.GetActiveScene().name == scenes[i])
            {
                if(PlayerPrefs.HasKey(highscores[i]))
                {
                    HighScore = PlayerPrefs.GetInt(highscores[i]);
                } else
                {
                    HighScore = 0;                    
                }
                
                currentSceneScore = highscores[i];
                
            }
        }
    }

    private void Update()
    {
        switch (menuState)
        {
            case MenuState.Off:

                tutorialMenu.SetActive(false);

                

                /*if(Input.GetKeyDown(KeyCode.F))
                {
                    
                    menuState = MenuState.On;
                }*/

                break;

            case MenuState.On:

                tutorialMenu.SetActive(true);

                

                /*if (Input.GetKeyDown(KeyCode.F))
                {
                    menuState = MenuState.Off;
                }*/

                break;
        }
        
    }

    public void MenuChange()
    {
        if(menuState == MenuState.Off)
        {
            menuState = MenuState.On;
        } else
        {
            menuState = MenuState.Off;
        }
    }

    public IEnumerator End()
    {
        //_audioScript.Music2();
        _audioScript.ambienceEmitter1.enabled = false;
        _audioScript.ambienceEmitter2.enabled = false;


        yield return new WaitForSecondsRealtime(0.1f);

        
        endState = true;
        EndMenu.SetActive(true);
        EndBackground.SetActive(true);
        GetComponent<PlayerMovement>().playerRB.velocity = Vector2.zero;
        GetComponent<PlayerMovement>().enabled = false;


        foreach (Text textObj in gameRushText)
        {

             if(textObj.gameObject.name == "R")
                {
                    endR.color = new Color(endR.color.r, endR.color.g, endR.color.b, 1f);
                }

                if (textObj.gameObject.name == "U")
                {
                    endU.color = new Color(endU.color.r, endU.color.g, endU.color.b, 1f);
                }

                if (textObj.gameObject.name == "S")
                {
                    endS.color = new Color(endS.color.r, endS.color.g, endS.color.b, 1f);
                }

                if (textObj.gameObject.name == "H")
                {
                    endH.color = new Color(endH.color.r, endH.color.g, endH.color.b, 1f);
                }
            
        }

        if(SceneManager.GetActiveScene().name != "VersusModeScene" && SceneManager.GetActiveScene().name != "DiscoModeScene" && SceneManager.GetActiveScene().name != "EndlessModeScene")
        {
            playerScoreText.text = "YOUR SCORE: " + Mathf.RoundToInt(EndScore);
            bestScoreText.text = "YOUR BEST SCORE: " + HighScore;

            _pointsScript.CalculatePoints(EndScore);

            if(EndScore <= _timeScript.levelTimes[3] + 3)
            {
                _pointsScript.SetDaily();
            }
        }

        if(SceneManager.GetActiveScene().name == "EndlessModeScene")
        {
            playerScoreText.text = "YOUR SCORE: " + Mathf.RoundToInt(EndScore);
            bestScoreText.text = "YOUR BEST SCORE: " + HighScore;
            _pointsScript.EndlessPoints(EndScore);
        }

        if(SceneManager.GetActiveScene().name == "VersusModeScene")
        {
            playerScoreText.text = "WINNER: " + GetComponent<P1ObstaclesScript>().winner;
            //bestScoreText.enabled = false;

            _pointsScript.GetPoints(100);
        } 

        if(SceneManager.GetActiveScene().name == "DiscoModeScene")
        {
            playerScoreText.text = "TEAM 1: " + GetComponent<T1DiscoObstScript>().T1Points + " POINTS";
            bestScoreText.text = "TEAM 2: " + GameObject.FindGameObjectWithTag("Player 2").GetComponent<T2DiscoObstacleScript>().T2Points + " POINTS";

            _pointsScript.GetPoints(100);
        }



        yield return new WaitForSecondsRealtime(5f);



        SceneManager.LoadScene("Menu Scene");
    }

    public void ShowPoints(int gainedPoints, int totalPoints)
    {
        pointsText.enabled = true;
        
        pointsText.text = "POINTS EARNED: " + gainedPoints + " \n\n" + "TOTAL POINTS: " + totalPoints;

        

    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "End")
        {
            
            GetEndScore();
            StartCoroutine("End");
        }

        
    }

    public void GetEndScore()
    {
        EndScore = uiScript.leveltime;

        SetHighScore();
    }

    void SetHighScore()
    {
        if(SceneManager.GetActiveScene().name == scenes[3])
        {
            if ((EndScore > HighScore && HighScore > 0) || (HighScore <= 0))
            {
                HighScore = Mathf.RoundToInt(EndScore);
                PlayerPrefs.SetInt(currentSceneScore, HighScore);
            }
        } else
        {
            if ((EndScore < HighScore && HighScore > 0) || (HighScore <= 0))
            {
                HighScore = Mathf.RoundToInt(EndScore);
                PlayerPrefs.SetInt(currentSceneScore, HighScore);
            }
        }
        
    }

    

    public enum MenuState
    {
        On,
        Off
    }
}
