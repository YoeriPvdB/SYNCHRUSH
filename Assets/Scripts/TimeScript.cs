using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScript : MonoBehaviour
{
    LevelBuilder _buildScript;
    PlayerMovement _moveScript;
    UIScript _uiScript;

    float levelDist, completeSpeed, boostSpeed, completeTime;

    public string[] levelAwards = {"Bronze", "Silver", "Gold", "Platinum"};

    public float[] levelTimes;

    public float levelTime = 0;
 
    public GameObject[] speedBoosts;

    private void Start()
    {
        boostSpeed = 0;

        _buildScript = GetComponent<LevelBuilder>();
        _moveScript = GetComponent<PlayerMovement>();
        _uiScript = GameObject.Find("UI Handler").GetComponent<UIScript>();


        GetTimes();
    }

    void GetTimes()
    {
        levelDist = Vector2.Distance(new Vector2(gameObject.transform.position.x, 0), new Vector2(/*_buildScript.chonks[_buildScript.chonks.Count - 1].transform.Find("ModeSwitch").position.x*/
            GameObject.FindGameObjectWithTag("End").transform.position.x, 0));

        print("distance: " + levelDist);

        speedBoosts = GameObject.FindGameObjectsWithTag("SpeedBoost");

        float baseSpeed = 1300f * Time.fixedDeltaTime;

        float boostedSpeed = baseSpeed * 3f;

        float speedCheck = boostedSpeed * 1.5f;

        boostSpeed = speedCheck * speedBoosts.Length;

        completeSpeed = (1300f + boostSpeed) * Time.fixedDeltaTime;

        completeTime = levelDist / (completeSpeed);



        levelTimes[0] = completeTime + 50;
        levelTimes[1] = completeTime + 35;
        levelTimes[2] = completeTime + 15;
        levelTimes[3] = completeTime + 1;

        _uiScript.SetGoals(levelAwards, levelTimes);
    }

    

    

    


}
