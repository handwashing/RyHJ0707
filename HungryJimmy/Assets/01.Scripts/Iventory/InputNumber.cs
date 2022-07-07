using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputNumber : MonoBehaviour
{
    private bool activated = false;
    [SerializeField]
    private Text text_Preview;      // 인벤토리에 있는 아이템의 갯수를 띄워줌
    [SerializeField]
    private Text text_Input;        // 버리고 싶은 갯수만큼 입력함
    [SerializeField]
    private InputField if_text;     // InputField의 텍스트

    [SerializeField]
    private GameObject go_Base;     // 필요할때만 활성화 시켜줄것

    [SerializeField]
    private ActionController thePlayer;     // 어떤 위치에서 생성될 것인가

    void Update()
    {
        if (activated)      // activated가 True일때만 실행
        {
            if (Input.GetKeyDown(KeyCode.Return))       // Enter키를 누르면
            {
                OK();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))      //Esc키를 누르면
            {
                Cancel();
            }
        }
    }

    public void Call()
    {
        go_Base.SetActive(true);
        activated = true;
        if_text.text = "";       // 호출할때마다 if_text 초기화
        text_Preview.text = DragSlot.instance.dragSlot.itemCount.ToString();    // 호출하자마자 text_Preview에 아이템의 소지갯수가 들어감 (즉 소지중인 모든아이템 갯수)
    }

    public void Cancel()
    {
        activated = false;
        go_Base.SetActive(false);
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;      // 드래그 끝났으니 null로 해줌
    }

    // 입력받았을 때 숫자인지 문자인지 먼저 확인
    public void OK()
    {
        DragSlot.instance.SetColor(0);      // 색 없애줌

        int num;
        if (text_Input.text != "")
        {
            if (CheckNumber(text_Input.text))
            {
                num = int.Parse(text_Input.text);       // int.Parse -> 문자열을 강제로 int로 형변환해줌
                if (num > DragSlot.instance.dragSlot.itemCount)     // 입력한 숫자가 아이템의 갯수보다 많을때 
                {
                    num = DragSlot.instance.dragSlot.itemCount;     // 입력한 숫자를 아이템 갯수로 맞춰줌
                }
            }
            else
            {
                num = 1;        // 숫자 외의 입력은 전부 1로 처리함
            }
        }
        else        // 아무것도 입력하지 않았을 때
        {
            num = int.Parse(text_Preview.text);     // text_Preview를 넘겨줄 것
        }
        StartCoroutine(DropItemCoroutine(num));
    }

    IEnumerator DropItemCoroutine(int _num)
    {
        for (int i = 0; i < _num; i++)
        {
            Instantiate(DragSlot.instance.dragSlot.item.itemPrefab, thePlayer.transform.position + thePlayer.transform.forward, Quaternion.identity);
            DragSlot.instance.dragSlot.SetSlotCount(-1);        // 하나씩 줄어들게 함
            yield return new WaitForSeconds(0.05f);
        }

        // 현재 아이템을 들고 있고, 그 아이템의 모든 갯수를 버릴 때 아이템 파괴
        if (int.Parse(text_Preview.text) == _num)       // 모두 버릴때
        {
            if (QuickSlotController.go_HandItem != null)        // 현재 들고있는 아이템이 있을경우
            {
                Destroy(QuickSlotController.go_HandItem);       // 아이템 파괴
            }
        }

        DragSlot.instance.dragSlot = null;      // 초기화해서 없애줌
        go_Base.SetActive(false);
        activated = false;
    }

    // 문자인지 숫자인지 구분
    private bool CheckNumber(string _argString)
    {
        char[] _tempCharArray = _argString.ToCharArray();
        // argString = "안녕하세요", _tempCharArray[0] = "안", [1] = "녕"...
        bool isNumber = true;
        for (int i = 0; i < _tempCharArray.Length; i++)
        {
            if (_tempCharArray[i] >= 48 && _tempCharArray[i] <= 57)     // 문자열을 char[]에 넣고 한글자씩 비교함
            {
                continue;
            }
            isNumber = false;       // 문자가 하나라도 있으면 조건을 만족하지 못해서 isNumber = false됨
        }

        return isNumber;
    }
}
