using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    [SerializeField] private string animalName;     // 동물의 이름
    [SerializeField] private int hp;        // 동물의 체력

    [SerializeField] private float walkSpeed;       // 걷기 스피드
    [SerializeField] private float runSpeed;        // 뛰기 스피드
    private float applySpeed;       // 이동시킬 스피드

    private Vector3 direction;      // 방향 설정

    private bool isWalking;     // 걷는지 안 걷는지 판별
    private bool isRunning;      // 뛰는지 판별
    private bool isDead;        // 죽었는지 판별

    // 상태변수
    // private bool isGround = true;       // 땅인지 아닌지

    [SerializeField] private float walkTime;    // 걷기 시간
    [SerializeField] private float runTime;     // 뛰기 시간
    private float currentTime;      // 여기에 대기 시간 넣고 1초에 1씩 감소시킬 것

    private BoxCollider boxCollider;

    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private BoxCollider boxCol;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Move();
        }
    }

    private void Move()
    {
        rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
        anim.SetBool("isWalking", isWalking);
        direction.Set(0f, Random.Range(0f, 360f), 0f);      // 방향 랜덤하게
    }

    private void Run(Vector3 _targetPos)        // 위협받거나 데미지 입었을 때 뛰게할것
    {
        // 플레이어와 반대방향으로 도망가게 할것
        direction = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;      // 이동시 runSpeed적용
        anim.SetBool("Running", isRunning);
    }

    public void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;

            if (hp <= 0)
            {
                Dead();
            }
            Run(_targetPos);        // _targetPos 방향으로 Run() 실행

        }
    }

    private void Dead()
    {
        isWalking = false;
        isDead = true;
        anim.SetTrigger("isDead");
        new WaitForSeconds(6f);
        gameObject.SetActive(false);
    }

}
