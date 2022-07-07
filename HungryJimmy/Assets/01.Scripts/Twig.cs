using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twig : MonoBehaviour
{
    [SerializeField]
    private int hp; //체력
    [SerializeField]
    private float destroyTime; //이펙트 삭제 시간

    [SerializeField]
    private GameObject go_little_Twig; //작은 나뭇가지 조각들
    [SerializeField]
    private GameObject go_hit_effect_prefab; //타격 이펙트

    //회전값 변수
    private Vector3 originRot; 
    private Vector3 wantedRot;
    private Vector3 currentRot;

    //필요한 사운드 이름
    [SerializeField]
    private string hit_Sound;
    [SerializeField]
    private string broken_Sound;

    void Start()
    {//rotation값을 그대로 받아오려면 quaternion으로 선언해야 한다 but 보기 편하게 euler값을 토대로 Vector3로 표기한 것 / 그래서 아래의  rotation값은 (euler로 변환 시킨) quaternion 값임
        originRot = transform.rotation.eulerAngles; 
        currentRot = originRot; //현재 위치값은 지금 자기 자신임
    }

    //데미지 입히기
    public void Damage(Transform _playerTf)
    {//맞을 때 플레이어가 때린 반대 방향으로 나뭇가지를 휘게 만들기
        hp--;

        Hit();

        StartCoroutine(HitSwayCoroutine(_playerTf));

        if(hp <= 0)
        {
            Destruction();//Destroy
        }
    }

    private void Hit()
    {
        SoundManager.instance.PlaySE(hit_Sound);
        //프리팹을 생성해 클론에 넣기 / 나뭇가지의 정중앙에 생성 / 회접값 : 기본값
        GameObject clone = Instantiate(go_hit_effect_prefab, 
                                       gameObject.GetComponent<BoxCollider>().bounds.center + (Vector3.up * 0.5f),
                                       Quaternion.identity);

        Destroy(clone, destroyTime); //destroyTime 후에 클론 삭제
    }

    IEnumerator HitSwayCoroutine(Transform _target) // 이 타겟은 player 의 Transform
    {
        Vector3 direction = (_target.position - transform.position).normalized; //방향구하기/나뭇가지와 플레이어가 서로 바라보는 방향 
   
        Vector3 rotationDir = Quaternion.LookRotation(direction).eulerAngles; //방향에 따른 각도 구하기/direction으로 바라보게 만들기
    
        CheckDirection(rotationDir); //각도에 따라 어느쪽으로 눕히는지 체크 /rotationDir을 토대로 눕는 방향 체크
        

        //체크한 값으로 currentRot에서 wantedRot까지 Lerp를 반복
        while (!CheckThreshold()) //임계점에 반환하지 않았을 때만(false일때...) 반복
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, 0.25f); //자연스럽게 꺾일 수 있도록 Lerp 주기
            transform.rotation = Quaternion.Euler(currentRot); //벡터3(currentRot)를 Euler로 변환해 Quaternion으로 바꿔줌
            yield return null;
        }
        
        //다시 원래 위치로 돌아가게 만들기...
        wantedRot = originRot;

        while (!CheckThreshold()) 
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, 0.15f); 
            transform.rotation = Quaternion.Euler(currentRot); 
            yield return null;
        }
    }

    private bool CheckThreshold() //임계점 체크 (currentRot이 wantedRot과 가까워 졌는지...)
    {//두개의 값을 뺐을 때 오차가 적으면 가깝다는 것을 이용
        if(Mathf.Abs(wantedRot.x - currentRot.x) <= 0.5f && Mathf.Abs(wantedRot.z - currentRot.z) <= 0.5f) 
            return true; //임계점에 도달함
        return false; //임계점에 도달하지 못함
    }

    private void CheckDirection(Vector3 _rotationDir)
    {
        Debug.Log(_rotationDir);

        if(_rotationDir.y > 180)
        {
            if(_rotationDir.y > 300)
                wantedRot = new Vector3(-50f, 0f, -50f);
            else if(_rotationDir.y > 240)
                wantedRot = new Vector3(0f, 0f, -50f);
            else
                wantedRot = new Vector3(50f, 0f, -50f);
        }
        else if(_rotationDir.y <= 180)
        {
            if(_rotationDir.y < 60)
                wantedRot = new Vector3(-50f, 0f, -50f);
            else if(_rotationDir.y < 120)
                wantedRot = new Vector3(0f, 0f, 50f);
            else
                wantedRot = new Vector3(50f, 0f, 50f);
        }
    }

    private void Destruction()
    {
        SoundManager.instance.PlaySE(broken_Sound);
//잘린 나뭇가지 생성
        GameObject clone1 = Instantiate(go_little_Twig, 
                                gameObject.GetComponent<BoxCollider>().bounds.center + (Vector3.up * 0.5f),
                                Quaternion.identity);
        GameObject clone2 = Instantiate(go_little_Twig, 
                                gameObject.GetComponent<BoxCollider>().bounds.center - (Vector3.up * 0.5f),
                                Quaternion.identity);

        Destroy(clone1, destroyTime);
        Destroy(clone2, destroyTime);

        Destroy(gameObject);
    }
}
