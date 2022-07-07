using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{//떨어지는 arrow들을 관리하는 스크립트
    public float beatTempo;  //arrow가 얼마나 빨리 떨어질지를 관리
    public bool hasStarted; //버튼을 누르면 arrow가 화면 아래로 떨어지도록...(버튼이 눌렸나 체크하는 것)

    void Start()
    {//우리가 설정한 비트 템포가 무엇이든 여기에 사용되어야 함
    //모든 비트를 60으로 나누어 -> 초당 얼마나 빨리 움직여야 하는지 알려줌
    //120에서는 초당 2단위로 움직여야 함
        beatTempo = beatTempo / 20f;
    }

    void Update()
    {
        //hasStarted가 되어있지 않다면...
        //상태가 없는지 확인하기
        
        if(!hasStarted)
        {//버튼이 눌렸는지 체크하고 없다면
            if(Input.anyKeyDown) //입력을 할 때 아무 키나 입력하면 아무 키나 입력할 수 있다
            {   
                hasStarted = true; //버튼이 눌려있다면 hasStarted상태인 것...
            }
        }
        else 
        //그렇지 않다면 비트에 따라 바를 아래로 내리기 
        //비트는 약 1분에 120비트가 적용되는 템포임
        //-> 분당 120 비트를 얻으려면 60으로 나누면 초당 2비트가 된다
        //유니티에서 초당 비트를 원하면 화살키를 움직일 것-> 초당 두 곳으로 움직여
        //화살키가 얼마나 빨리 움직일지를 나타낼 것
        {
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f); //모든 프레임으로 이동하지 않도록 비트 템포에 시간을 곱해줌
        }
        
    }
 }
