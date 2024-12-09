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
        dir = 0;
        if (horizontal>0|| vertical>0)
        {
            animator.SetBool("isMoving", true); // ∞»±‚ Ω√¿€
        }
        else
        {
            animator.SetBool("isMoving", false); // ∞»±‚ ∏ÿ√„
        }

        

        moveSpeed = direction.magnitude;
        animator.SetFloat("Speed", moveSpeed);
       
        dir = horizontal;
        

      

        //if (dir<0)
        //{
        //    animator.SetBool("isLeft", true); // ∞»±‚ Ω√¿€
        //}
        //else
        //{
        //    animator.SetBool("isLeft", false); // ∞»±‚ ∏ÿ√„
        //}
        
        //if (dir>0)
        //{
        //    animator.SetBool("isRight", true); // ∞»±‚ Ω√¿€
        //}
        //else
        //{
        //    animator.SetBool("isRight", false); // ∞»±‚ ∏ÿ√„
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    animator.SetBool("isWalkBack", true); // ∞»±‚ Ω√¿€
        //}
        //else
        //{
        //    animator.SetBool("isWalkBack", false); // ∞»±‚ ∏ÿ√„
        //}
        animator.SetFloat("Direction", dir);
        if (moveSpeed > 0)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }


}
