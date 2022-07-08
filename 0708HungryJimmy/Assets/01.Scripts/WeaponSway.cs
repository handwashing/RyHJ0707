using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    //기존 위치(가만히 있을 때 원래 위치로 돌아가야함)
    private Vector3 originPos;

    //(계산에 필요한)현재 위치
    private Vector3 currentPos;

    //sway 한계 (무기가 흔들릴 때 최대 얼마만큼 흔들릴지...)
    [SerializeField]
    private Vector3 limitPos;

    //정조준 sway 한계
    [SerializeField]
    private Vector3 fineSightLimitPos;

    //움직임의 부드러움 정도 (총이 얼마나 부드럽게 흔들릴지...)
    [SerializeField]
    private Vector3 smoothSway;

    //필요한 컴퍼넌트
    [SerializeField]
    private GunController theGunController;   //(총의 경우)정조준 상태를 받아올 수 있는 값이 필요


    void Start()
    {
        originPos = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.canPlayerMove && !Inventory.inventoryActivated /* && isActivated */)
            {
                TrySway();
            }
    }

    private void TrySway()
    {//상하좌우가 움직였을 떄
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
            Swaying();
        else
            BackToOriginPos();
    }

    private void Swaying()
    {//임시변수
     //정조준 상태가 아닐때의 무기 흔들림 / 마우스의 움직임값을 각각 변수에 대입
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");

        if (!theGunController.isFineSightMode)
        {
            //부드럽게 움직이도록value에 lerp 값 주고 currentPos에 넣어주기!
            //화면 밖으로 벗어나지 않게 Mathf.Clamp로 가두기  
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -limitPos.x, limitPos.x),
                Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.x), -limitPos.y, limitPos.y),
                originPos.z);
        }
        else
        {   //정조준 상태일때는 살짝 흔들리게 처리..
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.y), -fineSightLimitPos.x, fineSightLimitPos.x),
                Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
                originPos.z);
        }
        transform.localPosition = currentPos;
    }

    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;
    }
}
