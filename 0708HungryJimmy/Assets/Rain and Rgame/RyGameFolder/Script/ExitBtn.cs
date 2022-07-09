using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExitBtn : MonoBehaviour
{

    // public GameObject gamePanel;
    public AudioSource theMusic; //오디오 소스
    public BeatScroller theBS;

    public void Pause() 
    {
            theMusic.Pause();
            theBS.beatTempo = 0f;
    }
}
