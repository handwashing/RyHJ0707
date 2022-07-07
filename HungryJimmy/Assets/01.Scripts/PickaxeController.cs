using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
     //활성화 여부
     public static bool isActivate = true;

     private void Start()//임시
     {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
     }
    
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
                if(hitInfo.transform.tag == "Rock") //바위와 부딪혔을 경우
                {//Rock 클래스 안의 Mining을 호출
                    hitInfo.transform.GetComponent<Rock>().Mining();
                }
                else if(hitInfo.transform.tag == "Twig") //나뭇가지와 부딪혔을 경우
                {//Twig 클래스 안의 Damage 호출 / 플레이어 transform도 함께 가져오기 / FineObjectOfType을 통해 얻어와도 된다
                    hitInfo.transform.GetComponent<Twig>().Damage(this.transform); //도끼의 위치임
                }
                else if(hitInfo.transform.tag == "NPC") //나뭇가지와 부딪혔을 경우
                {//Twig 클래스 안의 Damage 호출 / 플레이어 transform도 함께 가져오기 / FineObjectOfType을 통해 얻어와도 된다
                    SoundManager.instance.PlaySE("Animal_Hit");
                    hitInfo.transform.GetComponent<Pig>().Damage(1 ,this.transform.position); //도끼의 위치임
                }
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
