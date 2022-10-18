using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;



public class AudioScript : MonoBehaviour
{
    public AudioSource audioSource, p1Source, p2Source, environmentSource, musicSource;

    public StudioEventEmitter jumpSound, blastSound, t1blast, t2blast, crashSound1, crashSound2, ambienceEmitter1, ambienceEmitter2, musicEmitter1, musicEmitter2, music3, music4;

    //public AudioClip jumpSound, blastSound, crashSound1, crashSound2, ambienceEmitter1, ambienceEmitter2, musicEmitter1, musicEmitter2, music3, music4;

    [SerializeField] AudioClip[] menuAudio, player1Audio, player2Audio, environmentAudio, musicAudio;
    
    

    public StudioGlobalParameterTrigger ambienceTrigger;

    int currentTrack;

    GameObject Player;




    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        
        
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == PlayerPrefs.GetString("GameScene"))
        {
           
            musicSource = GameObject.Find("Music").GetComponent<AudioSource>();
            currentTrack = 0;
            ShuffleTunes();
            audioSource = GameObject.Find("Menu Canvas").GetComponent<AudioSource>();

            blastSound = GameObject.Find("Shock Blast").GetComponent<StudioEventEmitter>();
        }
        
    }

    private void Update()
    {
        

        
    }

    void ShuffleTunes()
    {
        for (int i = 0; i < musicAudio.Length; i++)
        {
            int rnd = Random.Range(0, musicAudio.Length);
            var tempTrack = musicAudio[rnd];
            musicAudio[rnd] = musicAudio[i];
            musicAudio[i] = tempTrack;
        }

        
    }

    public IEnumerator PlayMusic()
    {
        musicSource.clip = musicAudio[currentTrack];

        musicSource.PlayDelayed(0.5f);

        yield return new WaitForSecondsRealtime(musicSource.clip.length);

       

        if(currentTrack < musicAudio.Length - 1 )
        {
            currentTrack++;
        } else
        {
            currentTrack = 0;
        }       
        StartCoroutine("PlayMusic");

    }

    public void PlayMenuAudio(int audioNum)
    {
        audioSource.clip = menuAudio[audioNum];

        audioSource.Play();
    }

    public void PlayP1Audio(int audioNum)
    {
        p1Source.clip = player1Audio[audioNum];
        p1Source.Play();

    }

    public void PlayEnvironmentAudio(int audioNum)
    {
       // environmentSource
    }


    /*public void Music3()
    {
        music3.Play();

        return;
    }

    public void Music4()
    {
        music4.Play();

        return;
    }

    public void Music2 ()
    {
        musicEmitter2.enabled = true;
        musicEmitter1.enabled = false;
    }*/

    public void SmallCrashAudio()
    {
        crashSound1.Play();
        return;
    }

    public void LargeCrashAudio()
    {
        crashSound2.Play();

        return;
    }

    public void JumpAudio()
    {
        jumpSound.Play();

        return;
    }


    public void BlastAudio()
    {
        /*audioSource.clip = blastSound;
        audioSource.Play();*/
        
        blastSound.Play();

        return;
    }

    public void T1BlastAudio()
    {
        t1blast.Play();
    }

    public void T2BlastAudio()
    {
        t2blast.Play();
    }

    /*public IEnumerator BlastAudio()
    {
        audioSource.clip = blastSound;
        audioSource.Play();

        yield return new WaitForSecondsRealtime(0.7f);

        audioSource.Stop();
    }

    public IEnumerator JumpAudio()
    {
        audioSource.clip = jumpSound;
        audioSource.Play();

        yield return null;
    }

    public IEnumerator CrashAudio()
    {
        audioSource.clip = crashSound;
        audioSource.Play();

        yield return new WaitForSecondsRealtime(1f);

        audioSource.Stop();
    }*/
}
