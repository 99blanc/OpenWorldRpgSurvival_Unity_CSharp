using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public bool Hand;
    public bool Weapon;
    public bool Rig;
    public bool Bow;
    public string weaponName;
    public int damage;
    public int stamina;
    public float range;
    public float attackCatch;
    public float attackDisable;
    public bool isHolding;
    public ParticleSystem weaponParticle;
    public AudioClip weaponSound;
}
