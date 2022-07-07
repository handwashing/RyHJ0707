using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject go_BaseUI;      
    [SerializeField] private SaveNLoad theSaveNLoad;

    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!GameManager.isPause)       
            {
                CallMenu();     // 메뉴호출
            }
            else
            {
                CloseMenu();
            }
        }
    }

    private void CallMenu()     // 메뉴 호출
    {
        GameManager.isPause = true;
        go_BaseUI.SetActive(true);
        Time.timeScale = 0f;        // 시간흐름이 0이 되게 = 정지시킴
    }

    private void CloseMenu()        // 메뉴 닫기
    {
        GameManager.isPause = false;
        go_BaseUI.SetActive(false);
        Time.timeScale = 1f;        // 다시 정상속도로 시간흐르게
    }

    public void ClickSave()
    {
        Debug.Log("세이브 버튼 클릭");
        theSaveNLoad.SaveData();
    }

    public void ClickLoad()
    {
        Debug.Log("로드");
        theSaveNLoad.LoadData();
    }

    public void ClickExit()
    {
        Debug.Log("게임종료");
        Application.Quit();
    }
}
