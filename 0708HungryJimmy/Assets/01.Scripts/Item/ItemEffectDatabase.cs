using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName;    // 아이템의 이름 (키값으로 사용) 
    [Tooltip("HP, HUNGRY, THIRSTY 만 가능합니다")]
    public string[] part;     // 효과가 적용될 부분
    public int[] num;     // 효과가 적용될 수치

}

public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;

    // 필요한 컴포넌트
    [SerializeField]
    private StatusController thePlayerStatus;
    // [SerializeField]
    // private WeaponManager theWeaponManager;
    [SerializeField]
    private SlotToolTip theSlotToolTip;
    [SerializeField]
    private QuickSlotController theQuickSlotController;

    private const string HP = "HP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY";

    // QuickSlotController 징검다리
    public void isActivatedQuickSlot(int _num)
    {
        theQuickSlotController.isActivatedQuickSlot(_num);
    }

    // SlotToolTip 징검다리
    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        theSlotToolTip.ShowToolTip(_item, _pos);
    }

    // SlotToolTip 징검다리
    public void HideToolTip()
    {
        theSlotToolTip.HideToolTip();
    }

    public void UseItem(Item _item)
    {
        if (_item.itemType == Item.ItemType.Equipment)      // 장비아이템일때 장비교체 시도
        {
            // ##### ChangeWeaponCoroutine이 오류나서 주석처리~
            // StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName));
        }
        else if (_item.itemType == Item.ItemType.Used)       // ItemType이 Used일때
        {
            for (int x = 0; x < itemEffects.Length; x++)
            {
                if (itemEffects[x].itemName == _item.itemName)
                {
                    for (int y = 0; y < itemEffects[x].part.Length; y++)        //itemEffects[x].part의 길이만큼 반복돌림
                    {
                        switch (itemEffects[x].part[y])
                        {
                            case HP:
                                thePlayerStatus.IncreaseHP(itemEffects[x].num[y]);
                                break;
                            case HUNGRY:
                                thePlayerStatus.IncreaseHungry(itemEffects[x].num[y]);
                                break;
                            case THIRSTY:
                                thePlayerStatus.IncreaseThirsty(itemEffects[x].num[y]);
                                break;
                            default:
                                Debug.Log("잘못된 Status 부위 HP, SP, DP, HUNGRY, THIRSTY, SATISFY만 가능합니다");
                                break;
                        }
                        Debug.Log(_item.itemName + "을 사용했습니다.");
                    }
                    return;
                }
            }
            Debug.Log("ItemEffectDatabase에 일치하는 itemNmae 없습니다");
        }
    }

}
