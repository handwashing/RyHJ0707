using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    //풀 체력
    [SerializeField]
    private int hp;

    //이펙트 제거 시간
    [SerializeField]
    private float destroyTime;

    //폭발력 세기
    [SerializeField]
    private float force;

    //타격 효과
    [SerializeField]
    private GameObject go_hit_effect_prefab;

    [SerializeField]
    private Rigidbody[] rigidbodies;
    private BoxCollider[] boxColliders;

    [SerializeField]
    private string hit_Sound;

    void Start()
    {
        rigidbodies = this.transform.GetComponentsInChildren<Rigidbody>();
        boxColliders = transform.GetComponentsInChildren<BoxCollider>();
    }

   public void Damage()
   {


        hp--;

        Hit();

        if(hp <= 0)
        {
            Destruction();
        }


   }

   private void Hit()
    {
        SoundManager.instance.PlaySE(hit_Sound);

        var clone = Instantiate(go_hit_effect_prefab, transform.position + Vector3.up, Quaternion.identity); //풀에서 약간 위의 ㅇ위치에서 프리팹 생성!
        Destroy(clone, destroyTime); //destroyTime후에 클론 삭제
    }

    private void Destruction()
    {
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].useGravity = true;
            rigidbodies[i].AddExplosionForce(1f, transform.position, 1f); //폭발세기, 폭발위치(자기 자신의 위치), 폭발반경
            boxColliders[i].enabled = true;
        }

        Destroy(this.gameObject, destroyTime);
    }
}
