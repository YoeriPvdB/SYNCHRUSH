using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkinsScript : MonoBehaviour
{
    string[] skins = {"Normal", "Wizard", "Rock", "Unicorn", "Skin 5" };
    public List<string> unlockedSkins = new List<string>();

    public bool[] skinStatus = { false, false, false, false, false };

    int[] requiredPoints = { 100, 100, 100, 100, 100 };

    GameObject[] skinButtons;

    

    [SerializeField] GameObject[] teamSprites, skinImages, teamButtons, Arrows;

    public int currentPoints, currentTeam;

    GameObject promptGo;

    [SerializeField] AudioScript _audioScript;

    

    public Text pointsText, denyText;

    public Text[] requirementTexts;

    public int skinChoice;

    private void Awake()
    {
        GetSkins();

        promptGo = GameObject.Find("Prompt");
        pointsText = GameObject.Find("Points Text").GetComponent<Text>();
        denyText = GameObject.Find("Deny Text").GetComponent<Text>();
        skinButtons = GameObject.FindGameObjectsWithTag("Skin");

        promptGo.SetActive(false);
        denyText.gameObject.SetActive(false);

        currentPoints = PlayerPrefs.GetInt("Points");
        SetPointText();
        CheckUnlocks();

        //currentPoints = 600;
        skinChoice = 0;
    }

    void GetSkins()
    {
        for(int i = 0; i < skins.Length; i++)
        {
            if(PlayerPrefs.HasKey("Skin " + i))
            {
                int skinNum = PlayerPrefs.GetInt("Skin " + i);

                skinStatus[skinNum] = true;
            }
        }
    }
    
    void CheckUnlocks()
    {
        for(int i = 0; i < skins.Length - 1; i++)
        {
            if (skinStatus[i] == false)
            {
                requirementTexts[i].text = "" + requiredPoints[i];
                //requirementTexts[i].text = "";
                skinButtons[i].GetComponent<Image>().color = Color.gray;
            }
            else
            {
                skinButtons[i].GetComponent<Image>().color = Color.white;
                requirementTexts[i].enabled = false;
            }
        }
    }

    void SetPointText()
    {
        pointsText.text = "POINTS: " + currentPoints;
    }

    public void SelectSkin (int skinChoice)
    {
        _audioScript.PlayMenuAudio(3);

        if(currentTeam == 0)
        {
            PlayerPrefs.SetString("skin", skins[skinChoice]);
            print("T1Skin: " + skins[skinChoice]);
        } else
        {
            PlayerPrefs.SetString("T2Skin", skins[skinChoice]);
            print("T2Skin: " + skins[skinChoice]);
        }
        
        //skinButtons[skinChoice].transform.Find("Skin Image").gameObject.SetActive(true);
    }

    public void UnlockSkin()
    {
        if(currentPoints >= requiredPoints[skinChoice])
        {
            _audioScript.PlayMenuAudio(3);
            skinStatus[skinChoice] = true;
            currentPoints -= requiredPoints[skinChoice];
            unlockedSkins.Add(skins[skinChoice]);
            PlayerPrefs.SetInt("Points", currentPoints);

            SetPointText();
            promptGo.SetActive(false);
            CheckUnlocks();
            print("choice: " + skinChoice);
            print("Reamining points: " + currentPoints);
            EventSystem.current.SetSelectedGameObject(skinButtons[skinChoice]);
            PlayerPrefs.SetInt("Skin " + skinChoice, skinChoice);

            SelectSkin(skinChoice);
        } else
        {
            //print("Not enough points");
            StartCoroutine("Deny");
            return;
        }
    }

    IEnumerator Deny()
    {
        _audioScript.PlayMenuAudio(2);
        denyText.gameObject.SetActive(true);
        promptGo.SetActive(false);


        yield return new WaitForSecondsRealtime(1.5f);

        denyText.gameObject.SetActive(false);
    }

    public void PromptUnlock()
    {
        _audioScript.PlayMenuAudio(1);
        promptGo.SetActive(true);
        promptGo.transform.Find("Prompt Text").GetComponent<Text>().text = "Spend " + requiredPoints[skinChoice] + " points?";
        EventSystem.current.SetSelectedGameObject(GameObject.Find("YAS"));
    }

    public void Cancel()
    {

        EventSystem.current.SetSelectedGameObject(skinButtons[skinChoice]);
    }
    
    public void CheckSkin(int skin)
    {
        skinChoice = skin;

        if(skinStatus[skin] == false)
        {
            PromptUnlock();
        } else
        {
            SelectSkin(skinChoice);
        }
    }

    public void TeamSelect(int team)
    {
        currentTeam = team;

        _audioScript.PlayMenuAudio(1);

        if(team == 0)
        {
            GameObject.Find("Player").GetComponent<MenuMove>().Sprites[0] = teamSprites[0];
            GameObject.Find("Player").GetComponent<MenuMove>().Sprites[1] = teamSprites[1];

            teamSprites[2].SetActive(false);
            teamSprites[3].SetActive(false);

            teamButtons[1].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            teamButtons[0].GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        } else
        {
            GameObject.Find("Player").GetComponent<MenuMove>().Sprites[0] = teamSprites[2];
            GameObject.Find("Player").GetComponent<MenuMove>().Sprites[1] = teamSprites[3];

            teamSprites[0].SetActive(false);
            teamSprites[1].SetActive(false);

            teamButtons[0].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            teamButtons[1].GetComponent<Image>().color = new Color(1, 1, 1, 1f);


        }
    }

    public void Hover(int currentButton)
    {
        _audioScript.PlayMenuAudio(0);

        //skinImages[currentButton].SetActive(true);

        for(int i = 0; i < skinButtons.Length - 1; i++)
        {
            if(i == currentButton)
            {
                Arrows[i].gameObject.SetActive(true);
                //skinImages[currentButton].SetActive(true);
            } else
            {
                Arrows[i].gameObject.SetActive(false);
               // skinImages[currentButton].SetActive(false);
            }
        }

        foreach(GameObject skin in skinImages)
        {
            if(skin == skinImages[currentButton])
            {
                skin.SetActive(true);
            } else
            {
                skin.SetActive(false);
            }
        }
    }

   


}
