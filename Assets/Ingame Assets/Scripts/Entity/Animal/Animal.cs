using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Entity
{
    protected override void Update()
    {
        base.Update();

        if (FieldOfView.View() && !dead) 
        {
            Run(FieldOfView.GetTargetPosition());
        }
    }

    protected void Run(Vector3 targetPos)
    {
        _dir = new Vector3(transform.position.x - targetPos.x, 0f, transform.position.z - targetPos.z).normalized;

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        NavMeshAgent.speed = runSpeed;

        eAnimator.SetBool("IsRunning", isRunning);
    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);

        if (!dead)
        {
            Run(FieldOfView.GetTargetPosition());
        }
    }
}
