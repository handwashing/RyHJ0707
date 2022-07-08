using UnityEngine;
using UnityEngine.EventSystems; // 오브젝트의 터치(상호작용)와 관련된 이름 공간(라이브러리)
using UnityEngine.UI;

public class VirtualJoystick02 : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public enum JoystickType { Move, Rotate }
    public JoystickType joystickType;
    public float sensitivity = 1f; // 조작 민감도

    private Image imageBackground; // 조이스틱 UI 중 배경 이미지 변수
    private Image imageController; // 조이스틱 UI 중 컨트롤러(핸들) 이미지 변수
    private Vector2 touchPosition; // 조이스틱의 방향정보를 외부 클래스에서 사용할 수 있도록 전역 변수 설정

    public float horizontal { get { return touchPosition.x * sensitivity; } }
    public float vertical { get { return touchPosition.y * sensitivity; } }

    private void Awake()
    {
        imageBackground = GetComponent<Image>();
        imageController = transform.GetChild(0).GetComponent<Image>();
    }

    /// <summary>
    /// 터치 시작 시 1회 실행
    /// </summary>
    public void OnPointerDown(PointerEventData eventData) 
        // IPointerDownHandler 인터페이스를 부모로 상속받을 경우 구현해야되는 메소드
        // 해당 스크립트를 가지고 있는 오브젝트를 터치했을 때 메소드가 1회 실행된다.
    {
        //Debug.Log("터치 시작 : " + eventData);
    }
    /// <summary>
    /// 터치 상태일 때 매 프레임 실행
    /// </summary>
    public void OnDrag(PointerEventData eventData)
        // IDragHandler 인터페이스를 부모로 상속받을 경우 구현해야되는 메소드
        // 해당 스크립트를 가지고 있는 오브젝트를 터치상태에서 드래그 했을 때 메소드가 매 프레임 실행된다.
    {
        touchPosition = Vector2.zero;

        // 조이스틱의 위치가 어디에 있든 동일한 값을 연산하기 위해
        // 'touchPosition'의 위치 값은 이미지의 현재 위치를 기준으로
        // 얼마나 떨어져 있는지에 따라 다르게 나온다.
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(imageBackground.rectTransform, eventData.position, eventData.pressEventCamera, out touchPosition))
        {
            // touchPosition 값의 정규화 [0 ~ 1]
            // touchPosition을 이미지 크기로 나눔
            touchPosition.x = (touchPosition.x / imageBackground.rectTransform.sizeDelta.x);
            touchPosition.y = (touchPosition.y / imageBackground.rectTransform.sizeDelta.y);

            // touchPosition 값의 정규화 [-n ~ n]
            // 왼쪽(-1), 중심(0), 오른쪽(1)로 변경하기 위해 touchPosition.x*2-1
            // 아래(-1), 중심(0), 위(1)로 변경하기 위해 touchPosition.y*2-1
            // 이 수식은 Pivot에 따라 달라진다. (좌하단 Pivot 기준)
            switch (joystickType)
            {
                case JoystickType.Move: // (좌하단 Pivot 기준)
                    touchPosition = new Vector2(touchPosition.x * 2 - 1, touchPosition.y * 2 - 1);
                    break;
                case JoystickType.Rotate: // (우하단 Pivot 기준)
                    touchPosition = new Vector2(touchPosition.x * 2 + 1, touchPosition.y * 2 - 1);
                    break;
            }

            // touchPosition 값의 정규화 [-1 ~ 1]
            // 가상 조이스틱 배경 이미지 밖으로 터치가 나가게 되면 -1 ~ 1보다 큰 값이 나올 수 있다.
            // 이 때 normailzed를 이용해 -1 ~ 1 사이의 값으로 정규화
            touchPosition = (touchPosition.magnitude > 1) ? touchPosition.normalized : touchPosition;
            
            

            // 가상 조이스틱 컨트롤러 이미지 이동
            // touchPosition은 -1 ~ 1 사이의 데이터이기 때문에 그대로 사용하게되면, 컨트롤러의 움직임을 보기 힘들다.
            // 하여, 배경 크기를 곱해서 사용한다.(단, 중심을 기준으로 왼쪽 -1, 오른쪽 1 이기 때문에 배경 크기의 절반을 곱함.)
            // TIP. 컨트롤러가 배경 이미지 바깥으로 튀어나가게 하고 싶지 않다면, 나눠주는 값을 더 크게 설정해야 한다.
            Vector2 controllerPosition = new Vector2(touchPosition.x * imageBackground.rectTransform.sizeDelta.x / 2,
                                                     touchPosition.y * imageBackground.rectTransform.sizeDelta.y / 2);
            imageController.rectTransform.anchoredPosition = controllerPosition;

            //Debug.Log("터치&드래그 : " + eventData);
        }
    }
    /// <summary>
    /// 터치 종료 시 1회 실행
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
        // IPointerUpHandler 인터페이스를 부모로 상속받을 경우 구현해야되는 메소드
        // 해당 스크립트를 가지고 있는 오브젝트를 터치하였다가 떼었을 때 메소드가 1회 실행된다.
    {
        // 터치 종료 시 이미지의 위치를 중앙으로 다시 옮긴다.
        imageController.rectTransform.anchoredPosition = Vector2.zero;
        // 다른 오브젝트에서 이동 방향으로 사용하기 때문에 이동 방향도 초기화
        touchPosition = Vector2.zero;

        //Debug.Log("터치 종료 : " + eventData);
    }
}
