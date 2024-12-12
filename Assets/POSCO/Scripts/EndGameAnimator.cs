using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameAnimator : MonoBehaviour
{
  Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
       
    }
    private void Update()
    {
        animator.SetBool("IsMoving", true);
    }
}
