using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//각각의 무기마다 컨트롤러를 붙이면 매우 번거롭고 중복됨. -> 공통된, 비슷한 유형의 무기끼리 따로 컨트롤러를 만들어 통합시키기
//Hand 스크립트에서 가져와서 참조형식으로 작성

public class HandController : CloseWeaponController
{
     //활성화 여부
     public static bool isActivate = false;

    void Update()
    {
        if (isActivate)
            TryAttack();
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

      public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }
}

