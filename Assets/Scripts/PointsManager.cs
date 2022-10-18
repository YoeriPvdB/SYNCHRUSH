using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointsManager : MonoBehaviour
{
    TimeScript _timeScript;
    UIScript _uiScript;
    MenuScript _menuScript;

    int totalPoints, checkpointCount, bonusCount, bonusScore;

    string[] scenes = { "TandemModeScene", "BlastModeScene", "NormalModeScene", "EndlessModeScene" };

    private void Start()
    {
        _menuScript = GetComponent<MenuScript>();
        _timeScript = GetComponent<TimeScript>();
        _uiScript = GameObject.Find("UI Handler").GetComponent<UIScript>();
        totalPoints = 0;

        if (SceneManager.GetActiveScene().name == "EndlessModeScene" || SceneManager.GetActiveScene().buildIndex == 11)
        {
            checkpointCount = 0;
            bonusCount = 100;
            StartCoroutine("ChallengeBonus", 33f);
        }
    }

    
    IEnumerator ChallengeBonus(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        checkpointCount++;

        bonusCount += 50;

        _uiScript.StartCoroutine("TextPopUp", "BONUS");
        StartCoroutine("ChallengeBonus", 30f);

    }

    public void CalculatePoints(float EndScore)
    {
       float pointPercent = 0f;

        int points = 200;

        for (int i = 0; i < _timeScript.levelTimes.Length; i++)
        {
            if (EndScore <= _timeScript.levelTimes[i])
            {
                if (pointPercent < 1)
                {
                    pointPercent += 0.25f;
                }
            }
        }

        points = Mathf.RoundToInt(points * pointPercent);

        GetPoints(points);
    }

    public void EndlessPoints(float EndScore)
    {

        float endScore = EndScore;

        
        bonusScore = bonusCount * checkpointCount;

       

        int basePoints = Mathf.RoundToInt(endScore * 5);

        int points =  basePoints + bonusScore;

        GetPoints(points);

    }

    public void GetPoints(int points)
    {
        totalPoints += points;

        int currentPlayerPoints = PlayerPrefs.GetInt("Points");

        

        _menuScript.ShowPoints(totalPoints, currentPlayerPoints);

        PlayerPrefs.SetInt("Points", currentPlayerPoints + totalPoints);


    }

    public void SetDaily()
    {
        int bonusPoints = 300;

        for (int i = 0; i < 2; i++)
        {
            if (SceneManager.GetActiveScene().name == scenes[i])
            {
                if (PlayerPrefs.GetInt("LvlChallenge " + i) == 1)
                {
                    

                    PlayerPrefs.SetInt("LvlChallenge " + i, 0);

                    GetPoints(bonusPoints);
                }
            }
        }
    }

    
}
