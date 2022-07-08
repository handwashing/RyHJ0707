using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 아이템 강의 26분에 Rock 스크립트에 내용 추가 필요 + 프리팹 할당 필요
public class ActionController : MonoBehaviour
{

    [SerializeField]
    private float range;    // 습득가능한 최대 거리 == ray 사정거리

    // 아이템 습득 가능하다면 true, default == false
    private bool pickupActivated = false;

    // 충돌체 정보 저장
    private RaycastHit hitInfo;     // 충돌체 정보 저장

    // 불을 근접해서 바라볼 시 true
    private bool fireLookActivated = false;

    // 특정 레이어를 가진 오브젝트에 대해서만 습득할 수 있게 함
    [SerializeField]
    private LayerMask layerMask;

    // 필요한 컴포넌트
    // 행동을 보여 줄 텍스트
    [SerializeField]
    private TextMeshProUGUI actionText;

    [SerializeField]
    private Inventory theInventory;

    [SerializeField]
    private QuickSlotController theQuickSlot;

    private void Start()
    {
        actionText = GetComponentInChildren<TextMeshProUGUI>();
    }

    // 매 프레임마다 키가 눌리고 있는지 확인
    private void Update()
    {
        // 매프레임마다 아이템이 있는지 확인
        CheckAction();
        TryAction();
    }

    // 아이템 먹을 때 E 키가 눌리는지 확인할 메서드
    private void TryAction()
    {
        // E키가 눌렸을 떄 아이템이 있는지 없는지 확인하는 메서드
        if (Input.GetMouseButtonDown(1))
        {
            CheckAction();
            CanPickUp();
            CanDropFire();
        }
    }

    // pickupActivated가 true라면 아이템을 주어라
    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                // 어떤 아이템을 획득했는지 확인
                // Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "획득했습니다.");
                // 인벤토리 스크립트 작성 후 추가
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                // 획득한 아이템 파괴
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }

    private void CanDropFire()
    {
        if (fireLookActivated)
        {
            if (hitInfo.transform.tag == "Fire" && hitInfo.transform.GetComponent<Fire>().GetIsFire())
            {
                // 손에 들고있는 아이템을 불에 넣음 == 선택된 퀵슬롯의 아이템을 넣는다 (Null).ItemName을 참조하면 오류나니까 이거부터 확인함

                Slot _selectedSlot = theQuickSlot.GetSelectedSlot();     // 이렇게 하면 null 일지라도 _selectedSlot에 값은 들어감
                if (_selectedSlot.item != null)      // 슬롯안에 아이템이 있는지 없는지 비교해야하니까 .item 넣어줌
                {
                    DropAnItem(_selectedSlot);
                }
            }
        }
    }

    private void DropAnItem(Slot _selectedSlot)
    {
        switch (_selectedSlot.item.itemType)
        {
            case Item.ItemType.Used:
                if (_selectedSlot.item.itemName.Contains("고기"))
                {
                    // // 조건을 만족하면 _selectedSlot에 아이템을 생성해줌 (불보다 조금 위의 위치에)
                    Instantiate(_selectedSlot.item.itemPrefab, hitInfo.transform.position + Vector3.up, Quaternion.identity);
                    theQuickSlot.DecreaseSelectedItem(); // 0개가 되면 슬롯에서 파괴됨
                }
                break;
            case Item.ItemType.Ingredient:
                break;
        }
    }

    private void CheckAction()
    {
        // 레이를 쏴서 플레이어가 바라보는 방향으로 충돌체의 정보를 확인하고 레이의 사정거리를 지정
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
            //else if (hitInfo.transform.tag == "WeakAnimal" || hitInfo.transform.tag == "StrongAnimail")
            //    MeatInfoAppear();
            else if (hitInfo.transform.tag == "Fire")       // 레이에 맞은 물체의 tag가 Fire면
                FireInfoAppear();       // Fire 정보 활성화
        }
        else // 아이템 획득하게 되면 정보 비활성화
        {
            InfoDisappear();        //### 원래 ItemInfoDisappear 였는데 나중에 바꾼거같아서 일단 바꿈###
        }
    }

    // ##### MeatInfoAppear()에도 Reset(); 해줘야함 #####
    private void Reset()    // 불 보다가 아이템 보는걸 반복하다보면 에러뜨는데 이를 막기위해 리셋함
    {
        pickupActivated = false;
        // dissolveActivated = false;       아직 작성안해서 에러뜨길래 주석처리해둠
        fireLookActivated = false;
    }


    // 아이템 정보가 보이는 메서드
    private void ItemInfoAppear()
    {
        Reset();
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "획득" + "<color=yellow>" + "(E)" + "</color>";
    }

    private void FireInfoAppear()
    {
        Reset();
        fireLookActivated = true;       // 켜진 상태의 불을 바라보면

        if (hitInfo.transform.GetComponent<Fire>().GetIsFire())
        {
            actionText.gameObject.SetActive(true);      // actionText도 활성화
            actionText.text = "선택된 아이템 불에 넣기" + "<color=yellow>" + "(E)" + "</color>";
        }
    }

    // 아이템 정보를 사라지게 하는 메서드
    private void InfoDisappear()
    {
        pickupActivated = false;
        // dissolveActivated = false;
        fireLookActivated = false;
        //
        actionText.gameObject.SetActive(false);
    }
}
