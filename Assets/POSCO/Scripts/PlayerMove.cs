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
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        if (Input.GetKey(KeyCode.LeftShift) && (horizontal != 0 || vertical > 0))
        {
            animator.SetBool("isRunning", true);
            //moveSpeed = direction.magnitude;
            //animator.SetFloat("Speed", moveSpeed);
            animator.SetBool("isMoving", false);
        }
        else 
        {
            animator.SetBool("isRunning", false); // øÚ¡˜¿” ∏ÿ√„
        }

        dir = 0;
        if (!Input.GetKey(KeyCode.LeftShift) &&( horizontal != 0 || vertical > 0))
        {
            animator.SetBool("isMoving", true); // øÚ¡˜¿” Ω√¿€
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isMoving", false); // øÚ¡˜¿” ∏ÿ√„
        }

        if (vertical < 0)
        {
     
            animator.SetBool("isWalkBack", true); // øÚ¡˜¿” Ω√¿€
        }
        else
        {
            animator.SetBool("isWalkBack", false); // øÚ¡˜¿” ∏ÿ√„
        }


                
        animator.SetFloat("Direction", horizontal);
        if (moveSpeed > 0)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }


}
