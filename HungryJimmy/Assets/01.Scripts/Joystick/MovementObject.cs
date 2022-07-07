using UnityEngine;

public class MovementObject : MonoBehaviour
{
    [SerializeField] private VirtualJoystick02 virtualJoystick;
    private float moveSpeed = 10;

    void Start()
    {
        
    }

    void Update()
    {
        float x = virtualJoystick.horizontal; // Left & Right
        float y = virtualJoystick.vertical; // Up & Down

        if(x != 0 || y != 0)
        {
            transform.position += new Vector3(x, 0, y) * moveSpeed * Time.deltaTime;
        }
    }
}
