using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [HideInInspector] public Animator pAnimator;

    private const int Hand = 0, Weapon = 1, Bow = 2, Rig = 3;

    private void Start()
    {
        pAnimator = GetComponentInChildren<Animator>();
    }

    public void LoadRunAnimation(bool b, float x, float y)
    {
        if (b)
        {
            pAnimator.SetBool("IsRunning", b);
            pAnimator.SetFloat("Xcos", x);
            pAnimator.SetFloat("Ycos", y);
        }
        else
        {
            pAnimator.SetBool("IsRunning", b);
        }
    }

    public void LoadSprintAnimation(bool b)
    {
        if (b)
        {
            pAnimator.SetBool("IsSprinting", b);
        }
        else
        {
            pAnimator.SetBool("IsSprinting", b);
        }
    }

    public void LoadDodgeAnimation(bool b, float x, float y)
    {
        if (b)
        {
            pAnimator.SetTrigger("doDodge");
            pAnimator.SetFloat("Xcos", x);
            pAnimator.SetFloat("Ycos", y);
        }
        else
        {
            pAnimator.SetBool("IsDodging", b);
        }
    }

    public void LoadFallAnimation(bool b)
    {
        if (b)
        {
            pAnimator.SetTrigger("doFall");
            pAnimator.SetBool("IsFalling", b);
        }
        else
        {
            pAnimator.SetBool("IsFalling", b);
        }
    }


    public void LoadJumpAnimation(bool b, float x, float y)
    {
        if (b)
        {
            pAnimator.SetBool("IsJumping", b);
            pAnimator.SetFloat("Xcos", x);
            pAnimator.SetFloat("Ycos", y);
        }
        else
        {
            pAnimator.SetBool("IsJumping", b);
        }
    }

    public void LoadAttackAnimation(bool b, int t)
    {
        if (b && t == Hand)
        {
            pAnimator.SetTrigger("doAttack");
            pAnimator.SetInteger("AttackType", Hand);
            pAnimator.SetBool("IsAttacking", b);
        }
        else if (b && t == Weapon)
        {
            pAnimator.SetTrigger("doAttack");
            pAnimator.SetInteger("AttackType", Weapon);
            pAnimator.SetBool("IsAttacking", b);
        }
        else if (b && t == Bow)
        {
            pAnimator.SetTrigger("doAttack");
            pAnimator.SetInteger("AttackType", Bow);
            pAnimator.SetBool("IsAttacking", b);
        }
        else if (b && t == Rig)
        {
            pAnimator.SetTrigger("doAttack");
            pAnimator.SetInteger("AttackType", Rig);
            pAnimator.SetBool("IsAttacking", b);
        }
        else
        {
            pAnimator.SetBool("IsAttacking", b);
        }
    }

    public void LoadBlockAnimation(bool b)
    {
        if (b)
        {
            pAnimator.SetTrigger("doBlock");
            pAnimator.SetBool("IsBlocking", b);
        }
        else
        {
            pAnimator.SetBool("IsBlocking", b);
        }
    }

    public void LoadPickUpAnimation(bool b)
    {
        if (b)
        {
            pAnimator.SetTrigger("doPickUp");
        }
        else
        {
            pAnimator.SetBool("IsPickUping", b);
        }
    }

    public void LoadHurtAnimation(bool b)
    {
        if (b)
        {
            pAnimator.SetTrigger("doHurt");
        }
        else
        {
            pAnimator.SetBool("IsHurting", b);
        }
    }

    public void LoadDeadAnimation(bool b)
    {
        if (b)
        {
            pAnimator.SetTrigger("doDead");
        }
        else
        {
            pAnimator.SetBool("IsDeading", b);
        }
    }
}
