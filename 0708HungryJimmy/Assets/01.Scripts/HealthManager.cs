using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;

    [SerializeField] private StatusController theStatus;
    [SerializeField] private PlayerController thePlayer;
    public Animator animator;
    private bool isDead = false;



    // Start is called before the first frame update
    void Start()
    {
        animator = thePlayer.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Dead();
        }
    }

    // IEnumerator Dead()
    // {
    //     if (theStatus.currentHungry <= 0)
    //     {
    //         GameManager.isPause = true;
    //         animator.SetTrigger("isDead");
    //         isDead = true;
    //         yield return new WaitForSecondsRealtime(6f);
    //         Time.timeScale = 0f;
    //     }
    // }

    public void Dead()
    {
        if (theStatus.currentHungry <= 0)
        {
            isDead = true;
            animator.SetTrigger("isDead");
            GameManager.isPause = true;
        }
    }
}
