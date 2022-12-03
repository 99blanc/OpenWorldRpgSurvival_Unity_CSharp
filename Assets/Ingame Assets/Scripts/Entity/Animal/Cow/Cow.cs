using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : Animal
{
    protected override void Reset()
    {
        base.Reset();

        RandomAction();
    }

    private void RandomAction()
    {
        RandomSound();

        int _random = Random.Range(0, 4);

        if (_random == 0)
            Wait();
        else if (_random == 1)
            Eat();
        else if (_random == 2)
            Peek();
        else if (_random == 3)
            TryWalk();
    }

    private void Wait()
    {
        currentTime = waitTime;
    }

    private void Eat()
    {
        currentTime = waitTime;
        eAnimator.SetTrigger("doEat");
    }

    private void Peek()
    {
        currentTime = waitTime;
        eAnimator.SetTrigger("doPeek");
    }
}
