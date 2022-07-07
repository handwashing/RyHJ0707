using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCharacterController2 : MonoBehaviour
{
    [SerializeField]
    private Transform characterBody;
    [SerializeField]
    private Transform cameraArm;
    [SerializeField]
    private VirtualJoystick02 moveJoystick;
    [SerializeField]
    private VirtualJoystick02 cameraJoystick;

    private CapsuleCollider capsuleCollider; //캡슐 콜라이더가 Mesh콜라이더와 맞닿아 있을 경우가 true임(지상)...

    //상태 변수
    // private bool isRun = false; //걷기인지 달리기인지 (false가 기본값)
    // private bool isCrouch = false; //앉아있는지 아닌지
    // private bool isGround = true; //땅인지 아닌지
    private bool isActivated = true;
    private bool isDead = false;
    private bool isMove = false;

    public Animator animator;
    private Rigidbody myRigid;
    private GunController theGunController;
    private Crosshair theCrosshair;
    private StatusController theStatusController;
    private Inventory theInven;

    void Start()
    {
        animator = characterBody.GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        //플레이어가 캡슐 콜라이더를 통제할 수 있도록 가져오기...
        //리지드바디 컴퍼넌트를 마이리지드 변수에 넣겠다
        myRigid = GetComponent<Rigidbody>();
        // applySpeed = walkSpeed;  //달리기 전까지 기본속도는 걷기

        theStatusController = FindObjectOfType<StatusController>();
        theGunController = FindObjectOfType<GunController>();
        theCrosshair = FindObjectOfType<Crosshair>(); //(?)
        theInven = FindObjectOfType<Inventory>();
    }

    void Update()
    {
        if (isActivated && GameManager.canPlayerMove)
        {
            // IsGround();// isGround가 true인지 false인지 확인하는 함수 
            // TryJump();// 점프중인지 확인하는 함수  
            // TryRun(); //뛰거나 걷는것을 구분하는 함수(판단 후 움직임 제어 / 순서주의)
            // TryCouch(); //앉으려고 시도
            Move(); //키입력에 따라 움직임이 실시간으로 이루어지게하는 처리
            // CameraRotation(); // 상하 카메라 회전
            // CharacterRotation();// 좌우 카메라 회전
            LookAround();
        }

        // if (theStatusController.currentHungry < 0 && !isDead)
        // {
        //     StartCoroutine(Dead());
        // }
    }

    private void Move()
    {
        //Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 moveInput = new Vector2(moveJoystick.horizontal, moveJoystick.vertical);
        bool isMove = moveInput.magnitude != 0;
        animator.SetBool("isMove", isMove);

        if (isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            //characterBody.forward = lookForward; // 캐릭터가 바라보는 정면은 카메라가 바라보는 정면
            characterBody.forward = moveDir; // 캐릭터가 바라보는 정면은 입력된 방향에 맞춰 바라본다. 
            transform.position += moveDir * Time.deltaTime * 5f;
        }
    }

    private void LookAround()
    {
        //Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector2 mouseDelta = new Vector2(cameraJoystick.horizontal * -1, cameraJoystick.vertical * -1);
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if (x < 180)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }

    private void Dead()
    {
        if (theStatusController.currentHungry <= 0 && !isDead)
        {
            animator.SetTrigger("isDead");
            isDead = true;
            myRigid.velocity = Vector3.zero;
            new WaitForSeconds(3f);
            gameObject.SetActive(false);
        }

    }
}
