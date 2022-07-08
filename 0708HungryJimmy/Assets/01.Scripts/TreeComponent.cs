using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeComponent : MonoBehaviour
{
    //깎일 나무 조각들
    [SerializeField]
    private GameObject[] go_treePieces;
    [SerializeField]
    private GameObject go_treeCenter; //중간에 있는 나무 piece

    //통나무
    [SerializeField]
    private GameObject go_Log_Prefabs;

    //쓰러질 때 랜덤으로 가해질 힘의 세기
    [SerializeField]
    private float force;

    //자식 트리
    [SerializeField]
    private GameObject go_ChildTree;

    //부모 트리 파괴되면, 캡슐 콜라이더 제거
    [SerializeField]
    private CapsuleCollider parentCol;

    //자식 트리 쓰러질 때 필요한 컴퍼넌트 활성화 및 중력 활성화
    [SerializeField]
    private CapsuleCollider childCol;
    [SerializeField]
    private Rigidbody childRigid;

    //파편
    [SerializeField]
    private GameObject go_hit_effect_prefab;

    //파편 제거 시간
    [SerializeField]
    private float debrisDestroyTime;

    //나무 제거 시간
    [SerializeField]
    private float destroyTime;


    //필요한 사운드
    [SerializeField]
    private string chop_sound; //나무 썰리는 소리
    [SerializeField]
    private string falldown_sound; //나무 떨어지는 소리
    [SerializeField]
    private string logChange_sound; //통나무로 바뀌는 소리


    public void Chop(Vector3 _pos, float angleY) // 도끼 부분에서 파편이 튀게 할 것 / (도끼랑 충돌한 지점의 위치, 플레이어가 서있는 위치)
    {
        Hit(_pos);

        AngleCalc(angleY); //플레이어가 어디서 도끼를 휘둘렀는지 각도 계산
   
        //piece가 남았는지 비교
        if (CheckTreePieces())
            return;

        FallDownTree(); //가운데 조각을 파괴해 나무 쓰러뜨리기
    }

    //적중 이펙트
    private void Hit(Vector3 _pos)
    {
        SoundManager.instance.PlaySE(chop_sound);


        GameObject clone = Instantiate(go_hit_effect_prefab, _pos, Quaternion.Euler(Vector3.zero));
        Destroy(clone, debrisDestroyTime);

    }

    private void AngleCalc(float _angleY)
    {
        Debug.Log(_angleY); //_angleY가 몇도인지에 따라서 몇번째 조각이 파괴되어야 하는지 결정
        if (0 <= _angleY && _angleY <= 70)
            DestroyPiece(2); //(2)번 나무 조각 파괴 => 003번 tree piece
        else if (70 <= _angleY && _angleY <= 140)
            DestroyPiece(3);
        else if (140 <= _angleY && _angleY <= 210)
            DestroyPiece(4);
        else if (210 <= _angleY && _angleY <= 280)
            DestroyPiece(0);
        else if (280 <= _angleY && _angleY <= 360)
            DestroyPiece(1);
    }
    
    private void DestroyPiece(int _num)
    {//피스가 있는 상태에서만 실행...(피스가 없는 상태에서 피스를 파괴하라고 하면 오류날 것)
        if(go_treePieces[_num].gameObject != null)
        {
            GameObject clone = Instantiate(go_hit_effect_prefab, go_treePieces[_num].transform.position, Quaternion.Euler(Vector3.zero));
            Destroy(clone, debrisDestroyTime);
            Destroy(go_treePieces[_num].gameObject);
        }
    }

    private bool CheckTreePieces()
    {
        for(int i = 0; i < go_treePieces.Length; i++)
        {//5개의 나무 피스가 남아있는지(파괴되지 않고...)
            if(go_treePieces[i].gameObject != null) //i번째 피스가 null이 아닌가...?
            {//null이 아니라면 아직 피스가 남아있는 것
                return true; //피스가 남아있다면 끝내버리기..
            }
        }
        return false; //위의 조건문에 걸리는 것이 없다면 남아있는 조각이 없는 것 -> false
    }

    private void FallDownTree()
    {
        SoundManager.instance.PlaySE(falldown_sound);
        Destroy(go_treeCenter);

        parentCol.enabled = false; //부모 트리의 콜라이더 비활성화 (콜라이더끼리의 충돌 방지)
        childCol.enabled = true; //자식 콜라이더 & 리지드바디 활성화
        childRigid.useGravity = true;

        childRigid.AddForce(Random.Range(-force,force), 0f, Random.Range(-force,force)); //나무가 기울어지도록 랜덤으로 힘주기
    
        StartCoroutine(LogCoroutine()); //나무가 쓰러지고 destroyTime후에 통나무 생성(코루틴)
    }

    IEnumerator LogCoroutine()
    {
        yield return new WaitForSeconds(destroyTime); //destroyTime만큼 대기

        SoundManager.instance.PlaySE(logChange_sound);

        Instantiate(go_Log_Prefabs, go_ChildTree.transform.position + (go_ChildTree.transform.up * 3f), Quaternion.LookRotation(go_ChildTree.transform.up)); //나무가 쓰러지는 위치에 통나무 생성 / go_ChildTree가 쓰러지는 방향의 위로 향하도록
        Instantiate(go_Log_Prefabs, go_ChildTree.transform.position + (go_ChildTree.transform.up * 6f), Quaternion.LookRotation(go_ChildTree.transform.up));
        Instantiate(go_Log_Prefabs, go_ChildTree.transform.position + (go_ChildTree.transform.up * 9f), Quaternion.LookRotation(go_ChildTree.transform.up));
        
        Destroy(go_ChildTree.gameObject);
    }
}
