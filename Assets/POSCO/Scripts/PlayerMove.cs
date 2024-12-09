using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Animator animator;
    private float moveSpeed;
    private float dir;
    private void Start()
    {
        animator = GetComponent<Animator>();

        
    }
    private void Update()
    {
        dir = 0;
        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("isMoving", true); // �ȱ� ����
        }
        else
        {
            animator.SetBool("isMoving", false); // �ȱ� ����
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        moveSpeed = direction.magnitude;
        animator.SetFloat("Speed", moveSpeed);
       
        dir = horizontal;
        

      

        if (dir<0)
        {
            animator.SetBool("isLeft", true); // �ȱ� ����
        }
        else
        {
            animator.SetBool("isLeft", false); // �ȱ� ����
        }
        
        if (dir>0)
        {
            animator.SetBool("isRight", true); // �ȱ� ����
        }
        else
        {
            animator.SetBool("isRight", false); // �ȱ� ����
        }
        if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("isWalkBack", true); // �ȱ� ����
        }
        else
        {
            animator.SetBool("isWalkBack", false); // �ȱ� ����
        }
        animator.SetFloat("Direction", dir);
        if (moveSpeed > 0)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }


}
