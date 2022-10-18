using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class LevelBuilder : MonoBehaviour
{
    public List<Object> loadedChonks = new List<Object>();

    GameObject tempChonk;

    GameObject[] boundaries;

    int previousChonk = -1, nextChonk = 1, modeSelect, chonkAmount = 3;

    //public int currentChonk = 0;

    string[] modes = { "ZoomModeTrigger", "NormalModeTrigger", "FlipModeTrigger", "SplitModeTrigger" };

    string[] scenes = { "TandemModeScene", "BlastModeScene", "NormalModeScene" };

    float space;

    int dailyChallenge, spriteNum;

    public List<GameObject> chonks = new List<GameObject>();

    public List<GameObject> backSprites = new List<GameObject>();

    public List<GameObject> powerUps = new List<GameObject>();

    Object backSprite;

    private void Awake()
    {
        boundaries = GameObject.FindGameObjectsWithTag("Bounds");

        dailyChallenge = PlayerPrefs.GetInt("dailyChallengeLvl");

        backSprite = Resources.Load("Prefabs/Background Stuff", typeof(GameObject));

        SceneCheck();

        ChonkShuffle();

        Setup();

        Label();

        Bounds();

       // BackgroundSet(0);

        if (SceneManager.GetActiveScene().name == "EndlessModeScene")
        {


            powerUps.AddRange(GameObject.FindGameObjectsWithTag("SpeedBoost"));

            foreach (GameObject boost in powerUps)
            {
                boost.gameObject.tag = "Life";
            }
        }

        /*if (SceneManager.GetActiveScene().name == "EndlessModeScene" || SceneManager.GetActiveScene().name == "NormalModeScene")
        {
            GetComponent<ModeSwitchScript>().CheckMode();
        }*/

    }

  
   

    public void BackgroundSet(int currentChonk)
    {
        if(currentChonk > 1)
        {
            Destroy(backSprites[0].gameObject);
            backSprites.Remove(backSprites[0]);

            Destroy(backSprites[1].gameObject);
            backSprites.Remove(backSprites[1]);
        }

        float dist = Vector2.Distance(new Vector2(chonks[currentChonk].transform.position.x, 0), new Vector2(chonks[currentChonk].transform.Find("ModeSwitch").position.x, 0));

        

        GameObject newSprites1 = Instantiate((GameObject)backSprite, new Vector2(chonks[currentChonk].transform.position.x, 0), transform.rotation);
        GameObject newSprites2 = Instantiate((GameObject)backSprite, new Vector2(chonks[currentChonk].transform.position.x + (dist / 2), 0), transform.rotation);

        backSprites.Add(newSprites1);
        backSprites.Add(newSprites2);
    }

    

    void SceneCheck()
    {
        if (SceneManager.GetActiveScene().name == "EndlessModeScene")
        {
            loadedChonks.AddRange(Resources.LoadAll("Prefabs/CHONKS", typeof(GameObject)));
            //GetChonks();
        }
        /*else
        {
            chonks.AddRange(GameObject.FindGameObjectsWithTag("Chonk"));
        }*/

        GetChonks();

        if(SceneManager.GetActiveScene().name == "BlastModeScene")
        {
            GetComponent<PlayerSwitch>().inTandem = false;
        }

        if(SceneManager.GetActiveScene().name == "TandemModeScene")
        {
            GetComponent<PlayerSwitch>().inTandem = true;
        }

        if(SceneManager.GetActiveScene().name == "VersusModeScene")
        {
            GetComponent<PlayerSwitch>().inTandem = true;
        }
    }

    void GetChonks()
    {
        /*for(int i = 0; i < chonkAmount; i++)
        {
            Instantiate((GameObject)loadedChonks[Random.Range(0, loadedChonks.Count + 1)]);
            
        }*/

        chonks.AddRange(GameObject.FindGameObjectsWithTag("Chonk"));
    }


    void ChonkShuffle()
    {

        if (SceneManager.GetActiveScene().name == scenes[0] || SceneManager.GetActiveScene().name == scenes[1] || SceneManager.GetActiveScene().name == scenes[2])
        {
            // print("dailyChallenge " + SceneManager.GetActiveScene().name);

            var today = System.DateTime.Today;
            var randomDaySeed = today.Day + today.Month * 10 + today.Year * 100;
            Random.InitState(randomDaySeed);
        }
        else
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
        }

        for (int i = 0; i < chonks.Count; i++)
        {
            int rnd = Random.Range(0, chonks.Count);
            tempChonk = chonks[rnd];
            chonks[rnd] = chonks[i];
            chonks[i] = tempChonk;

            
        }
        
    }

    void Setup()
    {
        for (int chonk = 0; chonk < chonks.Count; chonk++)
        {
            if (previousChonk < 0)
            {
                chonks[chonk].transform.position = new Vector2(0, 0);
            }
            else
            {
                chonks[chonk].transform.position = new Vector2(chonks[previousChonk].transform.Find("ModeSwitch").position.x, 0);
            }

            previousChonk++;
        }
    }

    void Label()
    {
        /*foreach (GameObject modeGo in chonks)
        {
            space = Vector2.Distance(new Vector2(0, modeGo.transform.Find("Top").transform.position.y), new Vector2(0, modeGo.transform.Find("Ground").transform.position.y));

            if (space <= 6f)
            {
                /*if(currentChonk <= 0)
                {
                    chonks[currentChonk].transform.Find("ModeSwitch").tag = modes[0];
                } else
                {
                    chonks[currentChonk - 1].transform.Find("ModeSwitch").tag = modes[0];
                }

                if(currentChonk > 0)
                {
                    chonks[currentChonk - 1].transform.Find("ModeSwitch").tag = modes[0];
                }
            }

            if (modeGo.transform.Find("ModeSwitch").tag != modes[0])
            {
                modeSelect = Random.Range(1, 3);
                modeGo.transform.Find("ModeSwitch").tag = modes[modeSelect];
            }

            if (currentChonk < chonks.Count - 1)
            {
                currentChonk++;
            }
        }*/

        for(int chonkNum = 0; chonkNum < chonks.Count - 1; chonkNum++)
        {
            space = Vector2.Distance(new Vector2(0, chonks[chonkNum + 1].transform.Find("Top").transform.position.y), 
                new Vector2(0, chonks[chonkNum + 1].transform.Find("Ground").transform.position.y));

            if(space <= 6f)
            {
                chonks[chonkNum].transform.Find("ModeSwitch").tag = modes[0];
            } else
            {
                modeSelect = Random.Range(1, 3);
                chonks[chonkNum].transform.Find("ModeSwitch").tag = modes[modeSelect];
            }
        }

        for(int i = 0; i < chonks.Count; i++)
        {
            chonks[i].transform.gameObject.name = "Chonk " + (i + 1);
        }

        if(SceneManager.GetActiveScene().name != "EndlessModeScene")
        {
            chonks[chonks.Count - 1].transform.Find("ModeSwitch").tag = "End";
        }

        
        
    }

    void Bounds()
    {
        foreach(GameObject boundGo in boundaries)
        {
            if(boundGo.transform.position.y > 3f )
            {
                boundGo.transform.position = new Vector2(boundGo.transform.position.x, 5f);
            }

            if(boundGo.transform.position.y < -3f)
            {
                boundGo.transform.position = new Vector2(boundGo.transform.position.x, -5f);
            }

            if(boundGo.transform.position.y > 0 && boundGo.transform.position.y < 3f)
            {
                boundGo.transform.position = new Vector2(boundGo.transform.position.x, 2.9f);
            }

            if(boundGo.transform.position.y < 0 && boundGo.transform.position.y > -3f)
            {
                boundGo.transform.position = new Vector2(boundGo.transform.position.x, -2.9f);
            }
        }
    }

    public void Infinte()
    {
        if(SceneManager.GetActiveScene().name == "EndlessModeScene")
        {
            Destroy(chonks[0].gameObject);
            chonks.Remove(chonks[0]);

            int selectedChonk = Random.Range(0, loadedChonks.Count);

            GameObject newChonk = Instantiate((GameObject)loadedChonks[selectedChonk], chonks[chonks.Count - 1].transform.Find("ModeSwitch").transform.position,
            chonks[chonks.Count - 1].transform.Find("ModeSwitch").transform.rotation);

            if (loadedChonks.Count > 1)
            {
                loadedChonks.Remove((GameObject)loadedChonks[selectedChonk]);
            }
            else
            {
                //GetChonks();
                loadedChonks.AddRange(Resources.LoadAll("Prefabs/CHONKS", typeof(GameObject)));
            }

            if(newChonk.transform.Find("SpeedBoost") != null)
            {
                newChonk.transform.Find("SpeedBoost").gameObject.tag = "Life";
            }

            chonks.Add(newChonk);

            //currentChonk = 0;

            Label();
        } 
    }
    
}
