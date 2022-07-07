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

    public GameObject rainCallingButton;


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

        if (theStatus.currentThirsty < 50) //목마름 상태가 50 이하라면... 
        {
            if (!rainCallingButton.activeInHierarchy) //이 상태에서부터 기우제를 시작할 수 있는 (로드씬) 버튼이 뜨도록... ( 그 전까지는 비활성화)
            {
                rainCallingButton.SetActive(true);

                if (theStatus.currentThirsty > 30 && !t_yellowUI)
                {
                    t_yellowUI = true;
                    StartCoroutine(ShowTYellowPanel()); 

                }
                if (theStatus.currentThirsty < 15)
                {
                    if (!t_redUI)
                    {
                        t_redUI = true;
                        StartCoroutine(ShowTRedPanel()); //경고창에 기우제를 시작하라는 메세지만 호출?
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
