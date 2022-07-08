using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp; //바위의 체력(0이 되면 파괴)

    [SerializeField]
    private float destroyTime; //파괴 후 남은 파편 제거 시간

    [SerializeField]
    private SphereCollider col; //구체 콜라이더 -> 파괴후에 비활성화시켜서 없애버릴거임...

    
    //필요한 게임 오브젝트
    [SerializeField]
    private GameObject go_rock; //일반 바위
    [SerializeField]
    private GameObject go_debris; //깨진 바위
    [SerializeField]
    private GameObject go_effect_prefabs; //채굴 이펙트
    
    //필요한 사운드 이름
    [SerializeField]
    private string strike_Sound;
    [SerializeField]
    private string destroy_Sound;

    //채굴
    public void Mining()
    {
        SoundManager.instance.PlaySE(strike_Sound);

        //바위 콜라이더의 가운데에 파편 클론 생성
        var clone = Instantiate(go_effect_prefabs, col.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime); //일정 시간(destroyTime) 후 파편 클론 파괴

        hp--; //hp를 1씩 깎아서...
        if (hp <= 0) //hp가 0이하면 파괴
            Destruction();
    }

    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroy_Sound);

        //바위가 파괴 되었기에 비활성화하고 사라지게 하기 -> 잔해만 남도록
        col.enabled = false;
        Destroy(go_rock);

        go_debris.SetActive(true); //바위 잔해 활성화시켜 나타나게 하기...
        Destroy(go_debris, destroyTime); //일정 시간(destroyTime) 후 debris도 삭제
    }

   
}
