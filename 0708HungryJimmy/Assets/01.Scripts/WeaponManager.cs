using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //무기 중복 교체 실행 방지
    public static bool isChangeWeapon = false; //true => ChangeWeapon x

    //현재 무기와 현재 무기의 애니메이션
    public static Transform currentWeapon; //기존의 무기를 껐다 켜는 기능뿐...가장 기본적인 컴퍼넌트 트랜스폼 사용
    public static Animator currentWeaponAnim;

    //현재 무기의 타입
    [SerializeField]
    private string currentWeaponType;

    //무기 교체 딜레이(총을 꺼내려고 손을 집어넣는 시간)
    [SerializeField]
    private float changeWeaponDelayTime;
    //무기 교체가 완전히 끝난 시점
    [SerializeField]
    private float changeWeaponEndDelayTime;

    //무기 종류들 전부 관리
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private CloseWeapon[] hands;
    [SerializeField]
    private CloseWeapon[] axes;
    [SerializeField]
    private CloseWeapon[] pickaxes;

    //관리 차원에서 쉽게 무기 접근이 가능하도록 만들기
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();

    //필요한 컴퍼넌트
    [SerializeField]
    private GunController theGunController; //건이나 핸드 등.. 하나를 비활성화하고 다른 하나를 실행할 수 있도록...
    [SerializeField]
    private HandController theHandController;
    [SerializeField]
    private AxeController theAxeController;
    [SerializeField]
    private PickaxeController thePickaxeController;


    void Start()
    {
        for (int i = 0; i < guns.Length; i ++)
        { //gunDictionary에 gunName이 key값으로 들어가고, value로 guns[i]가(자신이) 들어간다.
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        }  
        for (int i = 0; i < axes.Length; i++)
        {
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
        }  
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDictionary.Add(pickaxes[i].closeWeaponName, pickaxes[i]);
        }  
    }

    void Update()
    {
        if (! isChangeWeapon)
        {//숫자 1이 눌렸을 경우 / 무기 교체 실행(서브머신건)
            if (Input.GetKeyDown(KeyCode.Alpha1))
                StartCoroutine(ChangeWeaponCoroutine("HAND", "BareHand"));
        //숫자 2가 눌렸을 경우 / 무기 교체 실행(맨손) 
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
        //숫자 3이 눌렸을 경우 / 무기 교체 실행(도끼) 
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));
        //숫자 4가 눌렸을 경우 / 무기 교체 실행(곡괭이) 
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "Pickaxe"));
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true; //무기 교체가 중복으로 실행되지 않도록
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime); //무기를 넣을때까지 대기

        //바뀐 무기를 새로 꺼내야함 정조준 상태를 하고 있다면 먼저 해제해야함
        CancelPreWeaponAction(); //이전의 무기 취소
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime); //무기를 꺼내는 애니메이션이 끝날때까지 대기

        currentWeaponType = _type; //바꾸고자 할 타임_type을 현재 타입에 넣기
        isChangeWeapon = false; //무기 교체가 가능하도록...
    }

    //무기 취소 함수
    private void CancelPreWeaponAction()
    {//현재 타입에 따라서...
        switch(currentWeaponType)
        {
            case "GUN":
                theGunController.CancelFineSight();
                theGunController.CancelReload();
                GunController.isActivate = false; //이전의 것을 적용이 안되게 취소시키는 처리
                break;
            case "HAND":
                HandController.isActivate = false; //맨손상태일때 우클릭해도 정조준이 안될 것
                break;
            case "AXE":
                AxeController.isActivate = false; //Axe상태일때 우클릭해도 정조준이 안될 것
                break;
            case "PICKAXE":
                PickaxeController.isActivate = false; //Axe상태일때 우클릭해도 정조준이 안될 것
                break;
        }
    }

    //무기 교체 함수
    private void WeaponChange(string _type, string _name)
    {
        if (_type == "GUN")   
            theGunController.GunChange(gunDictionary[_name]);
        else if (_type == "HAND")
            theHandController.CloseWeaponChange(handDictionary[_name]);
        else if (_type == "AXE")
            theAxeController.CloseWeaponChange(axeDictionary[_name]);
        else if (_type == "PICKAXE")
            thePickaxeController.CloseWeaponChange(pickaxeDictionary[_name]);
    }
}
