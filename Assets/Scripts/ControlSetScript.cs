using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ControlSetScript : MonoBehaviour
{
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    public InputActionReference[] Actions; //p1Action, p2Action;
    public InputActionReference[] menuActions;

    public InputAction inputAction;

    private PlayerInput playerInput;

    public Text[] inputTexts;

    string[] bindTexts = {"SPACE", "N", "K", "L", "ESC" };

    string[] scenes = { "TandemModeScene", "BlastModeScene", "NormalModeScene", "EndlessModeScene", "VersusModeScene", "DiscoModeScene" };

    public Text[] tutorialTexts;

    public Image[] tutorialImages;

    [SerializeField] GameObject[] tutorials;

    public GameObject rebindPrompt;

    AudioScript _audioScript;

    bool canRemap;

    string GameScene;

    int textChoice, tutorialNum;

    
    private void Start()
    {

        playerInput = GetComponent<PlayerInput>();
        _audioScript = GameObject.Find("Sound Controller").GetComponent<AudioScript>();

        GameScene = PlayerPrefs.GetString("GameScene");

        CheckScene();

        GetKeyBinds();

        LoadKeybinds();

        
    }

    void CheckScene()
    {
        for(int i = 0; i < scenes.Length; i++)
        {
            if(GameScene == scenes[i])
            {
                tutorials[i].SetActive(true);

                tutorialTexts = tutorials[i].GetComponentsInChildren<Text>();
                tutorialImages = tutorials[i].GetComponentsInChildren<Image>();
            }
        }
    }

    void GetKeyBinds()
    {

        for(int i =0; i < bindTexts.Length; i ++)
        {
            bindTexts[i] = Actions[i].action.GetBindingDisplayString();
            Actions[i].action.Enable();
        }
    }

    void LoadKeybinds()
    {
        for(int i = 0; i < inputTexts.Length - 1; i++)
        {
            inputTexts[i].text = bindTexts[i];
        }

        if(inputTexts.Length < 4)
        {
            inputTexts[2].text = bindTexts[4];
        }
    }

    private void Update()
    {
        


        for (int i = 0; i < Actions.Length - 1; i++)
        {
            if (Actions[i].action.triggered)
            {
                GetButton(i);
            }
        }

        if(menuActions[0].action.triggered)
        {
            CycleCheck(0);
        }

        if(menuActions[1].action.triggered)
        {
            CycleCheck(1);
        }



    }

    public void StartRebind()
    {
        
        rebindPrompt.SetActive(true);
        inputAction.Disable();

        rebindingOperation = inputAction.PerformInteractiveRebinding()
                    
                    .OnMatchWaitForAnother(0.1f)
                    .OnComplete(operation => RebindComplete())
                    .Start();

       rebindingOperation.Start();

    }

    void RebindComplete()
    {
        
        rebindPrompt.SetActive(false);
        rebindingOperation.Dispose();
        inputAction.Enable();

        inputTexts[textChoice].text = inputAction.GetBindingDisplayString();
        bindTexts[textChoice] = inputAction.GetBindingDisplayString();

        _audioScript.PlayMenuAudio(2);
        //canRemap = true;
    }

    public void GetButton(int reference)
    {
        _audioScript.PlayMenuAudio(1);
        inputAction = Actions[reference];
        textChoice = reference;
        StartRebind();
    }

    public void LoadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameScene);
        Destroy(GameObject.Find("Music").gameObject);
    }

    public void CanRemap(bool status)
    {
        canRemap = status;
    }

    public void CycleCheck(int dir)
    {
        if(dir == 0)
        {
            tutorialNum--;
        } else
        {
            tutorialNum++;
        }

        if(tutorialNum < 0)
        {
            tutorialNum = tutorialTexts.Length - 1;
        }

        if(tutorialNum > tutorialTexts.Length - 1)
        {
            tutorialNum = 0;
        }

        Cycle();
    }

    void Cycle()
    {
        _audioScript.PlayMenuAudio(1);

        for(int i = 0; i < tutorialTexts.Length; i++)
        {
            if(i == tutorialNum)
            {
                tutorialTexts[i].enabled = true;
                tutorialImages[i].enabled = true;
            } else
            {
                tutorialTexts[i].enabled = false;
                tutorialImages[i].enabled = false;
            }
        }
    }
}
