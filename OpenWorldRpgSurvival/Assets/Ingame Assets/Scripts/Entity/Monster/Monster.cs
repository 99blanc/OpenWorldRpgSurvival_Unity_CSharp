using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity
{
    [SerializeField] protected int setDamage;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected float attackRange;
    [SerializeField] protected LayerMask targetMask;

    [SerializeField] protected float followTime;
    [SerializeField] protected float followDelayTime;

    [SerializeField] protected GameObject blood;
    protected float currentFollowTime;

    protected StatusController StatusController;

    protected override void Start()
    {
        base.Start();

        StatusController = FindObjectOfType<StatusController>();
    }

    protected override void Update()
    {
        base.Update();

        if (!dead)
        {
            CheckNight();
        }

        if (FieldOfView.View() && !dead && !isAttacking) 
        {
            StopAllCoroutines();
            StartCoroutine(FollowTargetCoroutine());
        }
    }

    protected void CheckNight()
    {
        if (!Day.isNight)
        {
            Spawner spawn = FindObjectOfType<Spawner>();
            spawn.CheckObject(entityName);
            Instantiate(blood, transform.position, Quaternion.identity);
            Destroy(gameObject, Random.Range(0, 100));
        }
    }

    protected void Follow(Vector3 targetPos)
    {
        _dir = targetPos;

        isFollowing = true;

        isWalking = false;
        isRunning = true;
        NavMeshAgent.speed = runSpeed;

        eAnimator.SetBool("IsRunning", isRunning);

        if (!dead && !ThirdPersonController.dead)
        {
            NavMeshAgent.SetDestination(_dir);
        }
        if (ThirdPersonController.dead)
        {
            Reset();
        }
    }

    protected IEnumerator FollowTargetCoroutine()
    {
        currentFollowTime = 0;
        Follow(FieldOfView.GetTargetPosition());

        while (currentFollowTime < followTime)
        {
            Follow(FieldOfView.GetTargetPosition()); 

            if (Vector3.Distance(transform.position, FieldOfView.GetTargetPosition()) <= attackRange)
            {
                if (FieldOfView.View() && !ThirdPersonController.dead)
                {
                    StartCoroutine(AttackCoroutine());
                }
            }

            yield return new WaitForSeconds(followDelayTime);

            currentFollowTime += followDelayTime;
        }

        isFollowing = false;
        isRunning = false;
        eAnimator.SetBool("IsRunning", isRunning);
        NavMeshAgent.ResetPath();
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        NavMeshAgent.ResetPath();
        currentFollowTime = followTime;

        yield return new WaitForSeconds(0.5f);
        transform.LookAt(FieldOfView.GetTargetPosition());

        eAnimator.SetTrigger("doAttack");
        yield return new WaitForSeconds(0.5f);

        RaycastHit _hit;
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out _hit, attackRange, targetMask))
        {
            if (ThirdPersonController.instance.GetBlock())
            {
                SoundController.instance.PlaySE(ThirdPersonController.instance.blockSound);
                StatusController.DecreaseHealth(5);
                StatusController.DecreaseStamina(setDamage);
            }
            else if (ThirdPersonController.instance.GetDodge())
            {
                
            }
            else
            {
                SoundController.instance.PlaySE(ThirdPersonController.instance.hurtSound);
                StatusController.DecreaseHealth(setDamage);
            }

            Instantiate(blood, _hit.point, Quaternion.identity);
        }

        yield return new WaitForSeconds(attackDelay);

        isAttacking = false;
        StartCoroutine(FollowTargetCoroutine());
    }

    protected override void Dead()
    {
        base.Dead();

        StopAllCoroutines();
        Spawner spawn = FindObjectOfType<Spawner>();
        spawn.CheckObject(entityName);
    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);

        if (!dead)
        {
            StopAllCoroutines();
            StartCoroutine(FollowTargetCoroutine());
        }
    }
}
