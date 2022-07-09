using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warning : MonoBehaviour
{

    [SerializeField] private GameObject hungry_yellow_Panel;
    [SerializeField] private GameObject hungry_red_Panel;
    [SerializeField] private GameObject thirsty_yellow_Panel;
    [SerializeField] private GameObject thirsty_red_Panel;


    bool h_yellowUI = false;      //아이템으로 회복할때마다 이거 처리 해줘야 계속 경고창 나타남
    bool h_redUI = false;
    bool t_yellowUI = false;
    bool t_redUI = false;


    public StatusController theStatus;
    public GameObject rgstButton; //리듬게임(기우제) 버튼


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (theStatus.currentHungry < 50)
        {
            if (theStatus.currentHungry > 30 && !h_yellowUI)
            {
                h_yellowUI = true;
                StartCoroutine(ShowHYellowPanel());
            }
            if (theStatus.currentHungry < 15)
            {
                if (!h_redUI)
                {
                    h_redUI = true;
                    StartCoroutine(ShowHRedPanel());
                }

            }
        }

        if (theStatus.currentThirsty < 50)
        {
            if (theStatus.currentThirsty > 30 && !t_yellowUI)
            {
                t_yellowUI = true;
                StartCoroutine(ShowTYellowPanel());
            }
            if (theStatus.currentThirsty < 15) //목마름 지수가 15보다 낮을 때
            {
                if(!rgstButton.activeInHierarchy)
                {
                    rgstButton.SetActive(true); //리듬게임 버튼 활성화

                    if (!t_redUI)
                    {
                        t_redUI = true;
                        StartCoroutine(ShowTRedPanel());
                    }
                }
            }
        }
    }

    IEnumerator ShowHYellowPanel()
    {
        hungry_yellow_Panel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hungry_yellow_Panel.SetActive(false);
        //yellowUI = false;
    }

    IEnumerator ShowHRedPanel()
    {
        hungry_red_Panel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hungry_red_Panel.SetActive(false);
        //yellowUI = false;
    }

    IEnumerator ShowTYellowPanel()
    {
        thirsty_yellow_Panel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        thirsty_yellow_Panel.SetActive(false);
        //redUI = false;
    }

    IEnumerator ShowTRedPanel()
    {
        thirsty_red_Panel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        thirsty_red_Panel.SetActive(false);
        //redUI = false;
    }


}
