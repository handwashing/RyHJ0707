using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameStage";      // GameStage로 씬 이동하기

    public static Title instance;       // 싱글턴화 해주기

    private SaveNLoad theSaveNLoad;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;        // 자기자신 넣어주고
            // ### 이거 했더니 게임 시작해도 타이틀 패널이 유지되길래 일단 주석처리해둠
            // DontDestroyOnLoad(gameObject);      // 씬이동해도 파괴되지 않게하기
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ClickStart()
    {
        Debug.Log("로딩");
        SceneManager.LoadScene(sceneName);
    }

    public void ClickLoad()
    {
        Debug.Log("로드");

        StartCoroutine(LoadCoroutine());

    }

    IEnumerator LoadCoroutine()     // 씬 로드하는 코루틴
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // ## 여기 while문에 로딩화면 만들어줘도 됨. operation.process 이용해서
        while (!operation.isDone)
        {
            yield return null;      // 로딩이 끝날때까지 대기시킴
        }
        
        theSaveNLoad = FindObjectOfType<SaveNLoad>();       // 다음씬으로 넘어가고 그 씬에 있는 SaveNLoad를 찾음
        theSaveNLoad.LoadData();        // 로드해봤자 의미없음 없는 플레이어 찾는거라
        this.gameObject.SetActive(false);        // DontDestroyOnLoad해놨기 때문에 그냥 SetActive로 감춰줌
    }

    public void ClickExit()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}
