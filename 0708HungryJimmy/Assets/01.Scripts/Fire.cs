using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private string fireName;      // 불의 이름 : 난로, 화롯불, 모닥불

    [SerializeField] private int damage;     // 불의 데미지

    [SerializeField] private float damageTime;       // 데미지가 들어갈 딜레이
    private float currentDamageTime;

    [SerializeField] private float durationTime;     // 불의 지속시간
    private float currentDurationTime;

    [SerializeField]
    private ParticleSystem ps_Flame;     // 파티클 시스템

    // 필요한 컴포넌트
    private StatusController thePlayerStatus;       // 플레이어의 상태

    // 상태변수
    private bool isFire = true;

    void Start()
    {
        thePlayerStatus = FindObjectOfType<StatusController>();
        currentDurationTime = durationTime;
    }

    void Update()
    {
        if (isFire)
        {
            ElapseTime();
        }
    }

    private void ElapseTime()       // 데미지 입는데 딜레이
    {
        currentDurationTime -= Time.deltaTime;

        if (currentDamageTime > 0)
        {
            currentDamageTime -= Time.deltaTime;
        }

        if (currentDurationTime <= 0)
        {
            Off();
        }
    }

    private void Off()
    {
        ps_Flame.Stop();
        isFire = false;
    }

    private void OnTriggerStay(Collider other)      // 불에 닿으면 데미지 입음
    {
        if (isFire && other.transform.tag == "Player")
        {
            if (currentDamageTime <= 0)
            {
                thePlayerStatus.DecreaseHP(damage);     // 데미지만큼 체력 감소
                currentDamageTime = damageTime;     // 데미지를 입었으니 damageTime으로 딜레이 걸어줌
            }
        }
    }

    public bool GetIsFire()
    {
        return isFire;
    }
}
