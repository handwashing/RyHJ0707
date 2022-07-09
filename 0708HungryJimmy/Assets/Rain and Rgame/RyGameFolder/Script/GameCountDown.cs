using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCountDown : MonoBehaviour
{
    private int Timer = 0;
    public GameObject Num_A; //no 1
    public GameObject Num_B; //no 2
    public GameObject Num_C; //no 3
    public GameObject Num_GO; //Go image

    public GameObject gamePanel;



    void Start()
    {//시작할 때 카운트다운 초기화, 게임 시작 false 설정
        Timer = 0;

        Num_A.SetActive(false);
        Num_B.SetActive(false);
        Num_C.SetActive(false);
        Num_GO.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {//리듬 게임이 활성화 되어있다면...
        if (gamePanel.activeInHierarchy)
        {
            //게임 시작시 정지
            if (Timer == 0)
            {
                Time.timeScale = 0.0f;
            }
            //타이머가 150보다 작거나 같다면 타이머 계속 증가
            if (Timer <= 150)
            {
                Timer++;
                //타이머가 60보다 크다면 3 켜기
                if(Timer > 60)
                {
                    Num_C.SetActive(true);
                }
                // 3끄고 2 켜기
                if(Timer > 90)
                {
                    Num_C.SetActive(false);
                    Num_B.SetActive(true);
                }
                //2끄고 1켜기
                if(Timer > 120)
                {
                    Num_B.SetActive(false);
                    Num_A.SetActive(true);
                }
                //1끄고 GO이미지 켜기
                if(Timer > 150)
                {
                    Num_A.SetActive(false);
                    Num_GO.SetActive(true);
                    StartCoroutine(this.LoadingEnd());
                    Time.timeScale = 1.0f;
                }
            }
        }
    }

    IEnumerator LoadingEnd()
    {
        yield return new WaitForSeconds(1.0f);
        Num_GO.SetActive(false);
    }
}

