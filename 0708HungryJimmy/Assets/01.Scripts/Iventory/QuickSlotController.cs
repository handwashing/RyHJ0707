using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotController : MonoBehaviour
{
    [SerializeField] private Slot[] quickSlots;     // 퀵슬롯들
    [SerializeField] private Image[] img_CoolTime;      // 퀵슬롯 쿨타임 이미지
    [SerializeField] private Transform tf_parent;       // 퀵슬롯의 부모 객체

    [SerializeField] private Transform tf_ItemPos;      // 아이템이 위치할 손 끝
    public static GameObject go_HandItem;       // 손에 든 아이템

    // 쿨타임 내용
    [SerializeField]
    private float coolTime;
    private float currentCoolTime;
    private bool isCoolTime;        // CoolTime인지

    // 퀵슬롯 보이기
    [SerializeField]
    private float appearTime;
    private float currentAppearTime;
    private bool isAppear;

    private int selectedSlot;       // 선택된 퀵슬롯

    // 필요한 컴포넌트
    [SerializeField]
    private GameObject go_Selectedimage;        // 선택된 퀵슬롯의 이미지
    [SerializeField]
    private WeaponManager theWeaponManager;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        quickSlots = tf_parent.GetComponentsInChildren<Slot>();
        anim = GetComponent<Animator>();
        selectedSlot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        TryInputNumber();
        CoolTimeCalc();
        AppearCalc();
    }

    private void AppearReset()
    {
        currentAppearTime = appearTime;
        isAppear = true;
        anim.SetBool("Appear", isAppear);
    }

    private void AppearCalc()
    {
        if (Inventory.inventoryActivated)       // 인벤토리 창이 나오면 무조건 퀵슬롯 나오게
        {
            AppearReset();
        }
        else
        {
            if (isAppear)
            {
                currentAppearTime -= Time.deltaTime;
                if (currentAppearTime <= 0)
                {
                    isAppear = false;
                    anim.SetBool("Appear", isAppear);
                }
            }
        }
    }

    private void CoolTimeReset()
    {
        currentCoolTime = coolTime;
        isCoolTime = true;
    }

    private void CoolTimeCalc()     // 쿨타임 계산
    {
        if (isCoolTime)
        {
            currentCoolTime -= Time.deltaTime;
            for (int i = 0; i < img_CoolTime.Length; i++)       // 쿨타임 이미지 구현
            {
                img_CoolTime[i].fillAmount = currentCoolTime / coolTime;
            }
            if (currentCoolTime <= 0)
            {
                isCoolTime = false;
            }
        }
    }

    private void TryInputNumber()
    {
        if (!isCoolTime)        // isCoolTime이 false일때만 실행
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangeSlot(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeSlot(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ChangeSlot(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ChangeSlot(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                ChangeSlot(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                ChangeSlot(5);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                ChangeSlot(6);
            }
        }

    }

    public void isActivatedQuickSlot(int _num)
    {
        if (selectedSlot == _num)
        {
            Execute();
            return;
        }
        if (DragSlot.instance != null)      // DragSlot.instance이 인벤토리 창을 열때만 활성화됨
        {
            if (DragSlot.instance.dragSlot != null)
            {
                if (DragSlot.instance.dragSlot.GetQuickSlotNumber() == selectedSlot)        // 퀵슬롯 다른 칸을 선택하고 아이템을 먹을때 오류 생기는데 그걸 방지함
                {
                    Execute();
                    return;
                }
            }
        }
    }

    private void ChangeSlot(int _num)
    {
        SelectedSlot(_num);

        Execute();
    }

    private void SelectedSlot(int _num)
    {
        // 선택된 슬롯
        selectedSlot = _num;
        // 선택된 슬롯으로 이미지 이동
        go_Selectedimage.transform.position = quickSlots[selectedSlot].transform.position;
    }

    private void Execute()
    {
        CoolTimeReset();
        AppearReset();

        if (quickSlots[selectedSlot].item != null)
        {
            if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Equipment)
            {
                StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(quickSlots[selectedSlot].item.weaponType, quickSlots[selectedSlot].item.itemName));
            }
            else if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Used)
            {
                ChangeHand(quickSlots[selectedSlot].item);
            }
            else
            {
                ChangeHand();
            }
        }
        else
        {
            ChangeHand();
        }
    }
    private void ChangeHand(Item _item = null)
    {
        StartCoroutine(theWeaponManager.ChangeWeaponCoroutine("HAND", "맨손"));
        if (_item != null)
        {
            StartCoroutine(HandItemCoroutine());
        }
    }

    IEnumerator HandItemCoroutine()
    {
        HandController.isActivate = false;
        yield return new WaitUntil(() => HandController.isActivate);     // 무기 교체의 마지막이 이뤄지면 람다식이 true가 됨
        go_HandItem = Instantiate(quickSlots[selectedSlot].item.itemPrefab, tf_ItemPos.position, tf_ItemPos.rotation);
        go_HandItem.GetComponent<Rigidbody>().isKinematic = true;       // 아이템이 중력의 영향 받지않게
        go_HandItem.GetComponent<BoxCollider>().enabled = false;        // 아이템 콜라이더 해제해서 플레이어와 부딪히지않음
        go_HandItem.tag = "Untagged";       // 태그되지 않은 상태로 바꿔줌
        go_HandItem.layer = 9;      //Weapon 레이어
        go_HandItem.transform.SetParent(tf_ItemPos);
    }

    public void DecreaseSelectedItem()
    {
        CoolTimeReset();
        AppearReset();

        quickSlots[selectedSlot].SetSlotCount(1);      // 활성화된 퀵슬롯 하나씩 차감

        if (quickSlots[selectedSlot].itemCount <= 0)
        {
            Destroy(go_HandItem);
        }
    }

    public bool GetIsCoolTime()     // 다른 스크립트에서 불러올수 있게하려고
    {
        return isCoolTime;
    }

    public Slot GetSelectedSlot()
    {
        return quickSlots[selectedSlot];
    }
}
