using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;
    private float moveSpeed;
    private Vector3 dir;
    private void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

    }


    public void Move(float horizontal, float vertical)
    {
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
    }

    private void Update()
    {



        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        dir = new Vector3(horizontal, 0, vertical).normalized;

        Move(horizontal, vertical);
        if(Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        
        if (dir.magnitude > 0)
        {
            animator.SetBool("isMoving", true);

        
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        

        //    Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        //    if (Input.GetKey(KeyCode.LeftShift) && (horizontal != 0 || vertical > 0))
        //    {
        //        animator.SetBool("isRunning", true);
        //        //moveSpeed = direction.magnitude;
        //        //animator.SetFloat("Speed", moveSpeed);
        //        animator.SetBool("isMoving", false);
        //    }
        //    else 
        //    {
        //        animator.SetBool("isRunning", false); // øÚ¡˜¿” ∏ÿ√„
        //    }


            //    if (!Input.GetKey(KeyCode.LeftShift) &&( horizontal != 0 || vertical > 0))
            //    {
            //        animator.SetBool("isMoving", true); // øÚ¡˜¿” Ω√¿€
            //        animator.SetBool("isRunning", false);
            //    }
            //    else
            //    {
            //        animator.SetBool("isMoving", false); // øÚ¡˜¿” ∏ÿ√„
            //    }

            //    if (vertical < 0)
            //    {

            //        animator.SetBool("isWalkBack", true); // øÚ¡˜¿” Ω√¿€
            //    }
            //    else
            //    {
            //        animator.SetBool("isWalkBack", false); // øÚ¡˜¿” ∏ÿ√„
            //    }



            //    animator.SetFloat("Direction", horizontal);
            //    if (moveSpeed > 0)
            //    {
            //        transform.Translate(direction * moveSpeed * Time.deltaTime);
            //    }
            //}


    }
}
