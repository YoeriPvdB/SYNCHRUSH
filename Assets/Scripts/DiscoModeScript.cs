using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DiscoModeScript : MonoBehaviour
{
    public List<GameObject> Bloks = new List<GameObject>();

    public Color[] colours;

    [SerializeField] string[] colourNames = {"Yellow", "Red", "Green", "Blue" };

    [SerializeField] GameObject[] obstacleParents;

    [SerializeField] GameObject[] shockwaves;

    [SerializeField] int[] bloksPerChonk, totalColours;

    public  bool[] teamBlast = { false, false };

    GameObject BonusStarter, wall1, wall2;

    public GameObject[] Players;

    public GameObject wall;

    int startBloks = 0, bloksQuarter, currentChonk;

    public int teamNum;

    public List<int> colourChoices;

    Transform firstTop, firstBot;

    
    private void Start()
    {
        wall1 = GameObject.FindGameObjectWithTag("D1");
        wall2 = GameObject.FindGameObjectWithTag("D2");

        Bloks.AddRange(GameObject.FindGameObjectsWithTag("Smol Block"));

        //bloksQuarter = Bloks.Count / 4;

        currentChonk = 0;
        startBloks = 0;

        obstacleParents = GameObject.FindGameObjectsWithTag("Obstacle");

        FindBloks();

        firstTop = Players[0].GetComponent<LevelBuilder>().chonks[0].transform.Find("Top").transform;
        firstBot = Players[0].GetComponent<LevelBuilder>().chonks[0].transform.Find("Ground").transform;

        Players[0].transform.position = new Vector2(2, firstBot.position.y + 0.7f);
        Players[1].transform.position = new Vector2(Players[1].transform.position.x, firstTop.position.y - 0.7f);

        
        //wall.SetActive(false);
    }

    void FindBloks()
    {

        foreach (GameObject blok in Bloks)
        {
            for (int i = 0; i < obstacleParents.Length; i++)
            {
                if (blok.gameObject.transform.parent == obstacleParents[i].transform)
                {
                    bloksPerChonk[i]++;
                }
            }
        }

        FindCurrent();
    }

    void FindCurrent()
    {
        bloksQuarter = Mathf.CeilToInt(bloksPerChonk[currentChonk] / 4);

        GetColours(0);
        
    }

    void GetColours(int colour)
    {
        for (int i = 0; i < bloksQuarter; i++)
        {
            colourChoices.Add(colour);
        }

        if (colour < colours.Length - 1)
        {
            colour++;
            GetColours(colour);
        } else
        {
            
            DiscoSetup();
        }
    }

    void DiscoSetup()
    {

        for (int i = startBloks; i < (startBloks + bloksQuarter); i++)
        {
            int colourChoice;
            int colourCount = 2;

            

            if (Bloks[i].transform.position.y <= 0)
            {
                if(colourChoices.Count > 2)
                {
                    colourChoice = Mathf.CeilToInt(Random.Range((colourChoices.Count / colourCount), colourChoices.Count));
                } else
                {
                    
                    colourChoice = colourChoices.Count - 1;
                    //print(colourChoices[0] + " " + colourChoices[1]);
                }
               
            } else
            {
                if(colourChoices.Count > 2)
                {
                    colourChoice = Mathf.RoundToInt(Random.Range(0, colourChoices.Count / colourCount));
                } else
                {
                   
                    colourChoice = 0;
                    //print(colourChoices[0] + " " + colourChoices[1]);
                }
               
            }

            Bloks[i].GetComponent<SpriteRenderer>().color = colours[colourChoices[colourChoice]];

            colourChoices.Remove(colourChoices[colourChoice]);

            
        }

        startBloks += bloksQuarter;

        if (colourChoices.Count > 0)
        {
            //colourChoice++;
            DiscoSetup();
            
        } else
        {
            //colourChoice = 0;
            print("Count: " + colourChoices.Count);
            if (currentChonk < obstacleParents.Length -1)
            {
                currentChonk++;
                FindCurrent();
            } else
            {
                DoubleCheck();
            }
        }
    }

    void DoubleCheck()
    {
        for(int i = 0; i < Bloks.Count; i++)
        {
            if(Bloks[i].transform.position.y > 0)
            {
                if(Bloks[i].GetComponent<SpriteRenderer>().color == colours[2] || Bloks[i].GetComponent<SpriteRenderer>().color == colours[3])
                {
                    int colour = Random.Range(0, 2);
                    Bloks[i].GetComponent<SpriteRenderer>().color = colours[colour];
                }
            } else
            {
                if (Bloks[i].GetComponent<SpriteRenderer>().color == colours[0] || Bloks[i].GetComponent<SpriteRenderer>().color == colours[1])
                {
                    int colour = Random.Range(2, 4);
                    Bloks[i].GetComponent<SpriteRenderer>().color = colours[colour];
                }
            }
        }
    }

    public void BlastCheck(int team)
    {
        if(team == 0)
        {
            GetComponent<VersusCheckScript>().t1Status = VersusCheckScript.T1Status.Check;
        } else
        {
            GetComponent<VersusCheckScript>().t2Status = VersusCheckScript.T2Status.Check;
        }
    }

    public void BlastPrep(int team)
    {
        if (team == 1)
        {
            GetComponent<VersusCheckScript>().t2Status = VersusCheckScript.T2Status.Blast;
            wall = wall2;
            Players[1].GetComponent<Player2Script>().StartCoroutine("WallTimer", wall);
        }
        else
        {
            GetComponent<VersusCheckScript>().t1Status = VersusCheckScript.T1Status.Blast;
            wall = wall1;
            Players[0].GetComponent<PlayerSwitch>().StartCoroutine("WallTimer", wall);
        }

        teamNum = team;

        wall.SetActive(true);
        
        wall.transform.position = new Vector2(Players[team].transform.position.x + 100f, 0);


        GetComponent<VersusCheckScript>().wall = wall;

        CheckNearbyObst(wall);

        
    }

    void CheckNearbyObst(GameObject wall)
    {
        Collider2D[] blokCols = Physics2D.OverlapCircleAll(wall.transform.position, 5f);

        foreach(Collider2D blok in blokCols)
        {
            if(blok.gameObject.tag == "Smol Block")
            {
                blok.gameObject.SetActive(false);
            }
        }
    }


    public void EnableShockwave(bool status, int team)
    {
        Players[team].GetComponent<BlastScript>().shockWave.SetActive(status);
        Players[team].GetComponent<BlastScript>().blastRadius = 1.2f;
        teamBlast[team] = false;
    }

    public void StartShockwave(int team)
    {
        if (Players[team].GetComponent<BlastScript>().shockWave.activeSelf && teamBlast[team] == false)
        {
            if(team == 0)
            {
                GameObject.Find("SoundController").GetComponent<AudioScript>().T1BlastAudio();
            } else
            {
                GameObject.Find("SoundController").GetComponent<AudioScript>().T2BlastAudio();
            }
            Players[team].GetComponent<BlastScript>().StartCoroutine("Blast");
            //Players[team].GetComponent<BlastScript>().canBlast = false;
            teamBlast[team] = true;
        }
    }

}
