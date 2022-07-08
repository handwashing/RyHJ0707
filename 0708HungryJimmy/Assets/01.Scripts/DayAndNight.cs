using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] private float secondPerRealTimeSecond; //게임 세계의 100초 =  현실 세계의 1초

    [SerializeField] private bool isNight = false;

    [SerializeField] private float fogDensityCalc; //증감량 비율

    [SerializeField] private float nightFogDensity; //밤 상태의 Fog 밀도
    private float dayFogDensity; //낮 상태의 Fog 밀도
    private float currentFogDensity; //계산


    // Start is called before the first frame update
    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity; //dayFogDensity에 현재값 주기
    }

    void Update()
    {//태양의 엑스축을 증가시켜 낮,밤 바꾸기 / 태양이 특정 각도로 기울어지면 낮,밤이 되도록 조정
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);
        //Debug.Log(transform.eulerAngles.x);
        
        if (transform.eulerAngles.x >= 170 && !isNight)
            isNight = true;
        else if (transform.eulerAngles.x >= 10 && isNight)
        {
            isNight = false;
            GameManager.instance.AddDate(1);
        }

        if (isNight) //밤일 경우
        {
            if (currentFogDensity <= nightFogDensity) //밤이여도 적당히 보이도록 nightFogDensity 이하일때만 실행
            {
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime; //특정시간만큼 계속 증가시키기
                RenderSettings.fogDensity = currentFogDensity; //위에 계산한 값을 실제 반영    

            }
        }
        else //낮일 경우
        {

            if (currentFogDensity >= dayFogDensity)
            {
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime; //특정시간만큼 계속 감소시키기
                RenderSettings.fogDensity = currentFogDensity; //위에 계산한 값을 실제 반영  
            }


        }

    }

}