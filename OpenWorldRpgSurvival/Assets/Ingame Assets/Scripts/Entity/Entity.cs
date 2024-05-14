using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    [SerializeField] protected string entityName;
    public int setHealth;
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;
    [SerializeField] protected float turnSpeed;

    protected float applySpeed;
    protected Vector3 _dir;
    protected bool isWalking, isRunning, isFollowing, isAttacking, action, dead = false;

    [SerializeField] protected float walkTime;
    [SerializeField] protected float waitTime;
    [SerializeField] protected float runTime;
    [SerializeField] protected int destroyTime;
    protected float currentTime;

    protected Animator eAnimator;
    protected Rigidbody rigid;
    protected BoxCollider bCollider;
    protected AudioSource audioSource;
    protected FieldOfView FieldOfView;
    protected NavMeshAgent NavMeshAgent;

    [SerializeField] protected int itemCount;
    [SerializeField] protected GameObject[] itemPrefabs;

    [SerializeField] protected AudioClip[] idleSound;
    [SerializeField] protected AudioClip hurtSound;
    [SerializeField] protected AudioClip deadSound;

    protected virtual void Start()
    {
        currentTime = waitTime;
        action = true;
        audioSource = GetComponent<AudioSource>();
        eAnimator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        bCollider = GetComponent<BoxCollider>();
        FieldOfView = GetComponent<FieldOfView>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {
        if (!dead)
        {
            Move();
            ElapseTime();
            CheckPos();
        }
    }

    protected void Move()
    {
        if (isWalking || isRunning)
        {
            NavMeshAgent.SetDestination(transform.position + _dir * 5f);
        }
    }

    protected void ElapseTime()
    {
        if (action)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0 && !isFollowing && !isAttacking)
            {
                Reset();
            }
        }
    }

    protected void CheckPos()
    {
        if (!Physics.Raycast(transform.position, Vector3.down, 8f))
        {
            Spawner spawn = FindObjectOfType<Spawner>();
            spawn.CheckObject(entityName);
            Destroy(gameObject);
        }
    }

    protected virtual void Reset()
    {
        action = true;

        NavMeshAgent.ResetPath();

        isWalking = false;
        eAnimator.SetBool("IsWalking", isWalking);
        isRunning = false;
        eAnimator.SetBool("IsRunning", isRunning);
        NavMeshAgent.speed = walkSpeed;

        _dir.Set(UnityEngine.Random.Range(-0.2f, 0.2f), 0f, UnityEngine.Random.Range(0.5f, 1f));
    }

    protected void TryWalk()
    {
        currentTime = walkTime;
        isWalking = true;
        eAnimator.SetBool("IsWalking", isWalking);
        NavMeshAgent.speed = walkSpeed;
    }

    public virtual void GetDamage(int damage)
    {
        if (!dead)
        {
            setHealth -= damage;

            if (setHealth <= 0)
            {
                Dead();
                Destroy(gameObject, destroyTime);
                return;
            }

            PlaySE(hurtSound);
            eAnimator.SetTrigger("doHurt");
        }
    }

    protected virtual void Dead()
    {
        PlaySE(deadSound);

        isWalking = false;
        isRunning = false;
        dead = true;

        int _random = UnityEngine.Random.Range(0, itemCount);

        try
        {
            for (int i = 0; i < itemPrefabs.Length; i++)
            {
                for (int j = 0; j < _random; j++)
                {
                    Instantiate(itemPrefabs[i], transform.position + transform.forward + transform.up, Quaternion.identity);
                }
            }
        }
        catch (NullReferenceException)
        {

        }

        eAnimator.SetTrigger("doDead");
    }

    protected void RandomSound()
    {
        int _random = UnityEngine.Random.Range(0, idleSound.Length);

        PlaySE(idleSound[_random]);
    }

    protected void PlaySE(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
