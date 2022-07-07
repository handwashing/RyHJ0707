using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    // 필요한 컴포넌트
    [SerializeField]
    private GameObject go_Base;     // 필요할때만 호출할 것

    [SerializeField]
    private Text txt_ItemName;
    [SerializeField]
    private Text txt_ItemDesc;
    [SerializeField]
    private Text txt_HowToUsed;


    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        go_Base.SetActive(true);
        // SlotToolTip 너비의 반, 높이의 반만큼 위치 조정함
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f, -go_Base.GetComponent<RectTransform>().rect.height * 0.5f, 0f);
        go_Base.transform.position = _pos;      // ToolTip을 Slot 오른쪽으로 위치하게 하고싶음

        txt_ItemName.text = _item.itemName;
        txt_ItemDesc.text = _item.itemDesc;

        if (_item.itemType == Item.ItemType.Equipment)      // ItemType이 Equipment일때
        {
            txt_HowToUsed.text = "우클릭 - 장착";       // txt_HowToUsed에 뜨게 할 설명
        }
        else if (_item.itemType == Item.ItemType.Used)
        {
            txt_HowToUsed.text = "우클릭 - 먹기";
        }
        else
        {
            txt_HowToUsed.text = "";
        }
    }

    public void HideToolTip()       // ToolTip 사라지게 함
    {
        go_Base.SetActive(false);
    }
}
