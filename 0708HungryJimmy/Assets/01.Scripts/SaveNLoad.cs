using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;        // Input, Output 출력

[System.Serializable]
public class SaveData       // 데이터를 직렬화하면 한줄로 데이터들이 나열되어 저장 장치에 읽고 쓰기가 쉬워짐
{
    public Vector3 playerPos;       // 저장할 플레이어의 위치
    public Vector3 playerRot;       // 저장할 플레이어가 바라보는 방향

    public List<int> invenArrayNumber = new List<int>();        // 슬롯 순서
    public List<string> invenItemName = new List<string>();     // 슬롯은 데이터 직렬화 불가능
    public List<int> invenItemNumber = new List<int>();     // 아이템 갯수
}

public class SaveNLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData();

    private string SAVE_DATA_DIRECTORY;     // 파일이 저장될 폴더 이름
    private string SAVE_FILENAME = "/SaveFile.txt";     // 저장될 파일 이름

    private PlayerController thePlayer;     // 플레이어 위치 가져옴

    private Inventory theInven;


    // ##### 안드로이드로 빌드할 경우 dataPath말고 persistentDataPath를 쓰니까 잘 되네요. 저 같은 문제 겪은 신 분 있을까봐 남겨놉니다.
    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves";      // Saves 폴더안에 생성

        if (!Directory.Exists(SAVE_DATA_DIRECTORY))     // SAVE_DATA_DIRECTORY가 없으면
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);     // SAVE_DATA_DIRECTORY 생성해줌
        }
    }

    public void SaveData()      // 데이터 저장
    {
        thePlayer = FindObjectOfType<PlayerController>();
        theInven = FindObjectOfType<Inventory>();

        saveData.playerPos = thePlayer.transform.position;      // 플레이어 위치 받아옴
        saveData.playerRot = thePlayer.transform.eulerAngles;       // vector3라 eulerAngles로 받음

        Slot[] slots = theInven.GetSlots();     
        for (int i = 0; i < slots.Length; i++)      // 슬롯의 갯수만큼 반복
        {
            if (slots[i].item != null){
                saveData.invenArrayNumber.Add(i);
                saveData.invenItemName.Add(slots[i].item.itemName);
                saveData.invenItemNumber.Add(slots[i].itemCount);
            }
        }

        string json = JsonUtility.ToJson(saveData);     // Json 파일로 저장

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);       // 텍스트들을 저 경로에 파일이름으로 저장할것

        Debug.Log("저장 완료");
        Debug.Log(json);
    }

    public void LoadData()      // 데이터 불러오기
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))       //SAVE_DATA_DIRECTORY에 SAVE_FILENAME이 있으면 조건문 실행
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);        // Json파일 다시 데이터로 풀어줌

            thePlayer = FindObjectOfType<PlayerController>();
            theInven = FindObjectOfType<Inventory>();

            thePlayer.transform.position = saveData.playerPos;
            thePlayer.transform.eulerAngles = saveData.playerRot;

            for (int i = 0; i < saveData.invenItemName.Count; i++)      // 리스트기때문에 Count
            {
                // 기억시킨 리스트를 for문 돌려서 다시 정보 받아옴
                theInven.LoadToInven(saveData.invenArrayNumber[i], saveData.invenItemName[i], saveData.invenItemNumber[i]);
            }

            Debug.Log("로드 완료");
        }
        else
        {
            Debug.Log("세이브 파일이 없습니다");
        }
    }
}

// ##### 파괴된 아이템은 bool 줘서 세이브데이터에 넣어주든지 하면됨
