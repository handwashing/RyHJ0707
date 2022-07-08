using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //스피드 조정 변수
    [SerializeField]
    private float walkSpeed; //걷기 속도
    [SerializeField]
    private float runSpeed; //달리기 속도
    [SerializeField]
    private float crouchSpeed; //앉을 때 속도
    private float applySpeed; //현재 적용중인 속도 / 이것 하나만 있어도 대입만 하면 됨...(여러개의 함수 쓸 필요없다)

    [SerializeField]
    private float jumpForce; //얼마만큼의 힘으로 위로 올라갈지

    //상태 변수
    private bool isRun = false; //걷기인지 달리기인지 (false가 기본값)
    private bool isCrouch = false; //앉아있는지 아닌지
    private bool isGround = true; //땅인지 아닌지
    private bool isActivated = true;

    //앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY; //앉을 때 이동할 위치값 (y값을 감소시켜 숙인것처럼...)
    private float originPosY; //원래높이값(숙였다가 돌아갈...)
    private float applyCrouchPosY; //적용될 위치값

    //땅 착지 여부 /바닥에 닿았는지 여부를  확인할 콜라이더
    private CapsuleCollider capsuleCollider; //캡슐 콜라이더가 Mesh콜라이더와 맞닿아 있을 경우가 true임(지상)...

    //필요한 컴퍼넌트
    [SerializeField]
    private Camera theCamera; //camera component
    //플레이어의 실제 육체(몸) / 콜라이더로 충돌 영역 설정, 리지드바디로 콜라이더에 물리적 기능 추가
    private Rigidbody myRigid;
    // private GunController theGunController;
    // private Crosshair theCrosshair;
    private StatusController theStatusController;

    // 조이스틱 가져오기
    [SerializeField] private VirtualJoystick02 moveJoystick;
    [SerializeField] private VirtualJoystick02 cameraJoystick;

    public Animator animator; // 애니메이션
    public Button Runbtn; // 달리기 버튼


    void Start()
    {
        //하이어라키의 객체를 뒤져서 카메라 컴퍼넌트가 있다면 theCamera에 
        //찾아서 넣어주기 -> theCamera = FindObjectOfType<Camera>(); 
        //카메라가 여래개일 수 있으니 프로젝트창에 직접 드래그했음...

        capsuleCollider = GetComponent<CapsuleCollider>();
        //플레이어가 캡슐 콜라이더를 통제할 수 있도록 가져오기...
        //리지드바디 컴퍼넌트를 마이리지드 변수에 넣겠다
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;  //달리기 전까지 기본속도는 걷기
        animator = gameObject.GetComponent<Animator>();

        theStatusController = FindObjectOfType<StatusController>();
        // theGunController = FindObjectOfType<GunController>();


        //초기화
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY; //기본 서있는 상태로 초기화
        // theCrosshair = FindObjectOfType<Crosshair>(); //(?)


    }

    void Update()
    {
        if (isActivated && GameManager.canPlayerMove)
        {
            IsGround();// isGround가 true인지 false인지 확인하는 함수 
            TryJump();// 점프중인지 확인하는 함수  
            TryRun(); //뛰거나 걷는것을 구분하는 함수(판단 후 움직임 제어 / 순서주의)
            TryCouch(); //앉으려고 시도
            Move(); //키입력에 따라 움직임이 실시간으로 이루어지게하는 처리
            CameraRotation(); // 상하 카메라 회전
        }

    }

    private void TryCouch() //앉기 시도
    {
        //좌측에 있는 crtl키를 눌러야 발동
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }

    }

    private void Crouch() //앉기 동작
    {//isCrouch가 실행될때마다 반전시키기
        isCrouch = !isCrouch;
        // 상태전환

        //if (isCrouch) //isCrouch가 true면 false로 바꿔주기
        //     isCrouch = false;
        // else
        //     isCrouch = true; //그렇지 않으면 true 
        //이렇게도 쓸 수 있다!

        if (isCrouch) //isCrouch가 트루면 앉는 모션으로...
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {//아니라면...
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        // 카메라 이동 (카메라의 현재 x 값, 바뀔 y 값 , 카메라의 현재 z 값)
        //theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applyCrouchPosY, theCamera.transform.localPosition.z);
        // 위의 코드를 대기시간 줘서 자연스럽게 앉는 느낌을 주었다.
        StartCoroutine(CrouchCoroutine());
    }

    private void TryJump() //점프 시도
    {//스페이스바를 한 번 눌렀을 경우 / 땅위에 있을 경우에...
        if (Input.GetKeyDown(KeyCode.Space) && isGround/*  && theStatusController.GetCurrentSP() > 0 */)
        {
            Jump();
        }
    }

    private void Jump() //점프
    {
        if (isCrouch)  //앉은 상태에서 점프시 앉은 상태 해제
            Crouch(); //앉아있다 점프 했을 때...  플레이어를 일어난 상태로...
                      //벨로서티 (현재 어느방향, 속도로 움직이는지...)를 변경해
                      //jumpForce만큼 순간적으로 위로 향하게 만들기...
        // theStatusController.DecreaseStamina(100);       // 점프 시 특정 값만큼 스테미나를 깎아줌
        myRigid.velocity = transform.up * jumpForce;
    }

    IEnumerator CrouchCoroutine() //부드러운 앉기 동작(카메라 이동 처리...)
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while (_posY != applyCrouchPosY) //posY가 원하는 값이 되면 벗어나도록(아니면 반복...)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f); //보관하기 _posY부터applyCrouchPosY까지 0.3f씩 증가
            theCamera.transform.localPosition = new Vector3(0, _posY, 0); //변경된 포스값을 카메라에 적용...
            if (count > 15)
                break; //무한반복 방지위해 보관 범위를 지정해줌...
            yield return null; //1프레임마다 쉬기 / _posY가 목적지까지 가면 while문에서 빠져 나옴...
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);
    }

    private void IsGround() //지면 체크
    {//고정된 좌표를 향해 y 반절의 거리만큼 (아래방향으로) 레이저 쏘기
     //-> 지면과 닿게 됨...isGround는 true를 반환해 점프할 수 있는 상태가 됨...
     //지면의 경사에 따라 오차가 생기는 것을 방지하기 위해 여유주기 /+0.1f/
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        // theCrosshair.JumpingAnimation(!isGround); //(?)
    }

    private void TryRun() //달리기 시도
    {//shitf키를 누르면 달릴 수 있도록...
        if (Input.GetKey(KeyCode.LeftShift)) //LeftShift 를 누르게 되면
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) //LeftShift에서 손을 떼면
        {
            RunningCancel();
        }
    }

    private void Running() //달리기 실행
    {
        if (isCrouch)  //앉은 상태에서 달릴때 앉은 상태 해제
            Crouch();
        // theGunController.CancelFineSight(); //정조준 모드 해제
        // theStatusController.DecreaseStamina(10);    // 달리는 중일때 지속적으로 값 깎음
       
        Vector2 moveInput = new Vector2(moveJoystick.horizontal, moveJoystick.vertical);
        bool isRun = moveInput.magnitude != 0;
        applySpeed = runSpeed; //스피드가 RunSpeed로 바뀜

        animator.SetBool("isRun", isRun);       
    }

    private void RunningCancel() //달리기 취소
    {
        isRun = false;
        applySpeed = walkSpeed; //걷는 속도
        animator.SetBool("isRun", isRun); 
    }

    private void Move() //움직임 실행
    {
        Vector2 moveInput = new Vector2(moveJoystick.horizontal, moveJoystick.vertical);
        bool isMove = moveInput.magnitude != 0;
        animator.SetBool("isMove", isMove);

        if (isMove)
        {
            Vector3 lookForward = new Vector3(theCamera.transform.forward.x, 0f, theCamera.transform.forward.z).normalized;
            Vector3 lookRight = new Vector3(theCamera.transform.right.x, 0f, theCamera.transform.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            gameObject.transform.forward = moveDir; // 캐릭터가 바라보는 정면은 입력된 방향에 맞춰 바라본다. 
            transform.position += moveDir * Time.deltaTime * applySpeed;
        }
    }

    private void CameraRotation()
    {  
        Vector2 mouseDelta = new Vector2(cameraJoystick.horizontal*0.5f , cameraJoystick.vertical*0.5f);
        Vector3 camAngle = theCamera.transform.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if (x < 180) // 위아래 영역
        {
            x = Mathf.Clamp(x, -1f, 50f);
        }
        else // 좌우 영역
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        theCamera.transform.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }


}
