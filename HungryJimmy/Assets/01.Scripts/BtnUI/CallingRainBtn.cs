using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CallingRainBtn : MonoBehaviour
{
    public void RainCallingButton()
    {
        SceneManager.LoadScene("Rhythm Game");
    }
}
