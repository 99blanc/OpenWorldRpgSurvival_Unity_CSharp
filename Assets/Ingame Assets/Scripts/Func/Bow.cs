using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public string bowName;
    public int damage;
    public int stamina;
    public float accuracy;
    public float arrowRate;
    public float reloadDelay;
    public float range;
    public float attackCatch;
    public bool isHolding;
    public Vector3 bowSight;
    public ParticleSystem bowParticle;
    public AudioClip bowSound;
}
