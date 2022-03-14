using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class CharacterAnimations : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private Animator animator;
    private PC pc;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        pc = GetComponentInParent<PC>();
        navAgent = GetComponentInParent<NavMeshAgent>();
    }

    private void Update()
    {
        if (pc.isMoving)
        {
            WalkAnimation();
        }
        else
        {
            IdleAnimation();
        }
    }

    private void IdleAnimation()
    {
        animator.Play("Gerald-Idle");
    }

    private void WalkAnimation()
    {
        animator.Play("Gerald-Walk");
    }
}
