using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;      // 인벤토리 활성화 되면 다른 움직임들 막을 때 사용할것

    // 필요한 컴포넌트 
    [SerializeField]
    private GameObject go_InventoryBase;

    [SerializeField]
    private GameObject go_SlotsParent;

    [SerializeField]
    private GameObject go_QuickSlotsParent;


    private Slot[] slots;       // 인벤토리 슬롯들
    private Slot[] quickslots;      // 퀵슬롯들
    private bool isNotPut;
    private int slotNumber;

    public Slot[] GetSlots() { return slots; }        // 슬롯에 있는 값 전부 반환

    [SerializeField] private Item[] items;      // 아이템 정보 받아올 배열 for문 돌려서 받아올것

    public void LoadToInven(int _arrayNum, string _itemName, int _itemNum)      // 아이템 이름, 번호만 가지고 넣기
    {
        for (int i = 0; i < items.Length; i++)      // 아이템 갯수만큼 돌림
        {
            if (items[i].itemName == _itemName)     // 넘어온 아이템의 이름이 같다면
            {
                slots[_arrayNum].AddItem(items[i], _itemNum);       // 아이템 추가
            }
        }
    }

    private void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
        quickslots = go_QuickSlotsParent.GetComponentsInChildren<Slot>();

    }

    private void Update()
    {

    }

    // private void TryOpenInventory()
    // {
    //     // if (!(Input.mousePosition.x > panelRect.rect.xMin && Input.mousePosition.x < panelRect.rect.xMax
    //     // && Input.mousePosition.y > panelRect.rect.yMin && Input.mousePosition.y < panelRect.rect.yMax))
    //     // {
    //         if (Input.GetMouseButtonDown(0))
    //         {
    //             inventoryActivated = !inventoryActivated;   // True <-> false로 바꿔줌

    //             if (inventoryActivated)     // True일 때
    //             {
    //                 OpenInventory();
    //             }
    //             else                        // False일 때
    //             {
    //                 CloseInventory();
    //             }
    //         }
    //     // }

    // }

    public void OpenInventory()
    {
        GameManager.isOpenInventory = true;
        go_InventoryBase.SetActive(true);
    }

    public void CloseInventory()
    {
        GameManager.isOpenInventory = false;
        go_InventoryBase.SetActive(false);
    }

    // 아이템을 먹으면 바로 퀵슬롯에 반영됨
    public void AcquireItem(Item _item, int _count = 1)
    {
        PutSlot(quickslots, _item, _count);     // 퀵슬롯이 가득찼다
        if (isNotPut)
        {
            PutSlot(slots, _item, _count);      // 인벤토리도 가득찼다
        }
        // PutSlot(quickslots, _item, _count);              ##### 없는 내용있어서 일단 주석처리
        // if (!isNotPut)
        //     theQuickSlot.isActivateQuickSlot(slotNumber);

        // if (isNotPut)
        //     PutSlot(slots, _item, _count);

        if (isNotPut)
            Debug.Log("퀵슬롯과 인벤토리가 꽉찼습니다");
    }

    // ##### 원래 AcquireItem 였는데 PutSlot으로 변경한듯.
    public void PutSlot(Slot[] _slots, Item _item, int _count = 1)
    {

        if (Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] != null)
                {
                    if (_slots[i].item.itemName == _item.itemName)
                    {
                        slotNumber = i;
                        _slots[i].SetSlotCount(_count);
                        isNotPut = false;
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item == null)
            {
                _slots[i].AddItem(_item, _count);
                return;
            }
        }

        isNotPut = true;
    }
}