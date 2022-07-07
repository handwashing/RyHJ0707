using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 키보드, 마우스, 터치를 이벤트로 오브젝트에 보낼 수 있는 기능을 지원

public class VirtualJoystick01 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // [SerializeField] : private 변수를 인스펙터에서 접근가능하게 해주는 기능으로 원하는 변수 앞에 '[SerializeField]'를 붙인다.
    // 심화설명 : 'Serialize'는 직렬화라는 작업을 의미한다. 직렬화는 추상적인 데이터를 전송 가능하고 저장 가능한 형태로 바꾸는 것을 의미한다.
    // 유니티는 Public 데이터만 직렬화하지만 'Serialize'를 선언함으로써 private 데이터도 직렬화 처리가 되며, 우리가 인스펙터창에서
    // 수정한 데이터가 저장되어 남아있다고 보면 된다. 
    [SerializeField] private Canvas joystickCanvas; // 조이스틱이 표현된 캔버스
    [SerializeField] private RectTransform leverTransform; // 조이스틱 내, 빨간 레버의 위치값을 담아둘 변수
    private RectTransform joystickTransform; // 조이스틱 내, 하얀 배경의 위치값을 담아둘 변수


    // [Range(0, 12)] : 인스펙터에서 적용가능한 수를 최소값(0)과 최대값(12)으로 제한해놓을 수 있다.
    [SerializeField, Range(10, 300)] private float leverRange; // 레버가 움직일 수 있는 거리를 담아둘 변수


    private Vector2 inputDirection;
    private bool isInput;

    [SerializeField] private TPSCharacterController controller;

    private void Awake()
    {
        joystickTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData) // 드래그를 시작할 때,
    {
        Debug.Log("드래그 시작 합니다.");

        controller.Move(Vector2.zero);
        ControlJoystickLever(eventData);
        isInput = true;
    }

    // 오브젝트를 클릭해서 드래그 하는 도중에 들어오는 이벤트로써,
    // 클릭을 유지한 상태로 마우스 이동을 멈추면 이벤트가 들어오지 않음
    public void OnDrag(PointerEventData eventData) // 드래그 중 일 때,
    {
        Debug.Log("드래그 중 입니다.");

        ControlJoystickLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData) // 드래그를 끝 냈을 때,
    {
        Debug.Log("드래그를 끝 냈습니다.");

        // 가상 조이스틱에서 손을 땠을 때, 빨간 레버의 위치가 조이스틱 하얀 배경의 정중앙에 위치하도록 처리
        leverTransform.anchoredPosition = Vector2.zero;
        isInput = false;
        controller.Move(Vector2.zero);
    }

    private void ControlJoystickLever(PointerEventData eventData)
    {
        // 캔버스 스케일 모드를 'Scale with screen size'로 변경하면 화면 크기에 따라 변경되는 입력 위치와 조이스틱이 가지는
        // AnchoredPosition 위치에 차이가 발생된다. 하여, 캔버스의 크기와 곱해서 적용한다.
        Vector2 scaledAnchoredPosition = joystickTransform.anchoredPosition * joystickCanvas.transform.localScale.x;
        // 레버가 위치해야 될 위치를 구하기 : 터치한 위치 - 조이스틱 하얀 배경의 위치
        // eventData.position : 터치한 위치를 가지고 있는 매개변수
        // joystickTransform.anchoredPosition : 조이스틱 하얀 배경의 위치 값
        Vector2 inputPos = eventData.position - scaledAnchoredPosition;
        // magnitude : 벡터의 길이를 반환
        // normalized : 정규화 벡터 : 순수 방향을 표시하기 위한 벡터
        Vector2 inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        leverTransform.anchoredPosition = inputVector;

        // inputVector는 해상도를 기반으로 만들어진 값으로 캐릭터의 이동속도로 사용하기에는 너무 큰 값을 가지고 있다.
        // 해당 값을 그대로 사용하면 캐릭터의 이동속도가 너무 빠르게 적용된다.
        // 하여, 입력방향의 값을 방향벡터(0~1의 값)로 정규화된 값으로 캐릭터에 전달하기 위함.
        inputDirection = inputVector / leverRange;
    }

    private void InputContolVector() // 처리된 입력방향을 캐릭터에 전달하기위한 메서드
    {
        Debug.Log($"InputDirection : X ({inputDirection.x}) / Y ({inputDirection.y})");
        controller.Move(inputDirection);
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (isInput)
        {
            InputContolVector();
        }
    }
}
