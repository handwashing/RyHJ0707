using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodOnFire : MonoBehaviour
{
    [SerializeField]

    private float time;     // 익히거나 타는데 걸리는 시간
    private float currentTime;      // 굽는중인 시간

    private bool done;      // 끝났으면, 더이상 불에 있어도 계산 무시할 수 있게 함

    [SerializeField]
    private GameObject go_CookedItemPrefab;      // 익혀지거나 탄 아이템 교체


    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Fire" && !done)     // "Fire" tag에 닿으면
        {
            currentTime += Time.deltaTime;      // currentTime에 Time을 더해줌

            if (currentTime >= time)        // currentTime이 time 이상이면
            {
                done = true;        // 익힘을 끝냄
                Instantiate(go_CookedItemPrefab, transform.position, Quaternion.Euler(transform.eulerAngles));       // go_CookedItemPrefab (구워졌거나 탄 고기 획득) 활성화
            }
        }
    }
}
