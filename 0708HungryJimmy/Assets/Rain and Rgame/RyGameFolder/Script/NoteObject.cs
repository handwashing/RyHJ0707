using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteObject : MonoBehaviour
{//Arrow가 떨어질 때 옳은 위치의 버튼을 누르면 Arrow가 사라짐 / Note 의 Hit과 Miss를 체크해 저장
    public bool canBePressed; //버튼이 눌렸는지?

   // public bool isImgOn; //이미지 활성화 여부
   // public Image img;

    public KeyCode keyToPress; //키 정보

    public GameObject hitEffect, goodEffect, perfectEffect, missEffect; //일치하는 노트 효과 가져오기

    // Start is called before the first frame update
    void Start()
    {
        //img.enabled = true;
        //isImgOn = true; //default = true;
    }

    // Update is called once per fra
    public void Update()
    {//만약 keyToPress상태이면 canBePressed상태가 true일지 체크하겠다...
        if(Input.GetMouseButtonDown(0)) 
        {
            if(canBePressed)
            {
                //img.enabled = false; //?
                //isImgOn = false; //?
                gameObject.SetActive(false); //true상태면 오브젝트가 옳은 위치에 있다는 것 /디폴트로 돌리는 것

                RhythmGameManager.instance.NoteHit(); //RhythmGameManager에 있는 NoteHit함수를 가져다 쓴다
            
                if(Mathf.Abs(transform.position.y) > 0.25) //버튼의 y축에서 0.25이상일 때 Hit 처리가 되었다면...
                {
                    Debug.Log("Hit");
                    RhythmGameManager.instance.NormalHit();
                    Instantiate(hitEffect,transform.position, hitEffect.transform.rotation); //hitEffect의 포지션 값 => hitEffect의 rotation 값임
                }
            
            }
        }
    }

    private void OnTriggerEnter(Collider other) //영상에서는 Collider2D 사용 / 다른 콜라이더를 가진(태그 된)물체와 닿았을 때 / 딱 한 번만 감지
    {   //만약 다른 콜라이더의 태그가 "Activator"라면...
        //canBePressed 할 수 있는 상태인지 체크
        if(other.tag == "Activator")
        {
            canBePressed = true; //위의 상태면 버튼을 누를 수 있는 
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.tag == "Activator")
        {
            canBePressed = false;

            RhythmGameManager.instance.NoteMissed();
            Instantiate(missEffect,transform.position, missEffect.transform.rotation); 
            
        }
    }
}
