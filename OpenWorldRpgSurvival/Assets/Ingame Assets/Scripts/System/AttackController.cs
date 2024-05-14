using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [HideInInspector] public static AttackController instance;

    public bool isAttack = false;
    public bool isSwing = false;

    private Tool enableHand;
    private RaycastHit hitInfo;

    [SerializeField] private Transform player;
    [SerializeField] private Transform view;
    [SerializeField] private GameObject equipEvent;
    [SerializeField] private GameObject effect;
    [SerializeField] private LayerMask layerMask;

    private Camera target;
    private Transform Crosshair;
    private ThirdPersonController ThirdPersonController;
    private StatusController StatusController;
    private EquipController EquipController;

    private const int Hand = 0, Weapon = 1, Bow = 2, Rig = 3;

    private void Start()
    {
        EquipController = equipEvent.GetComponent<EquipController>();
        ThirdPersonController = player.GetComponentInParent<ThirdPersonController>();
        StatusController = FindObjectOfType<StatusController>();
        target = view.GetComponentInChildren<Camera>();
        Crosshair = target.transform;
    }

    public void TryAttack(int hand)
    {
        if (!isAttack)
        {
            for (int i = 0; i < EquipController.rWeapon.Length; i++)
            {
                if (EquipController.rWeapon[i].rWeaponPrefab.activeSelf)
                {
                    enableHand = EquipController.rWeapon[i].rWeaponPrefab.GetComponent<Tool>();
                    break;
                }
            }

            StartCoroutine(AttackCoroutine(hand));
        }
    }

    private IEnumerator AttackCoroutine(int hand)
    {
        StatusController.DecreaseStamina(enableHand.stamina);
        isAttack = true;

        yield return new WaitForSeconds(enableHand.attackDisable);

        isSwing = true;
        StartCoroutine(HitCoroutine(hand));

        yield return new WaitForSeconds(enableHand.attackCatch);

        isSwing = false;

        yield return new WaitForSeconds(enableHand.attackDisable);

        isAttack = false;
    }

    private IEnumerator HitCoroutine(int hand)
    {
        while (isSwing)
        {
            if (CheckObject() && StatusController.setStamina >= enableHand.stamina)
            {
                if (hand == Hand)
                {
                    if (hitInfo.transform.tag == "Entity")
                    {
                        SoundController.instance.PlaySE(ThirdPersonController.hitSound);
                        hitInfo.transform.GetComponent<Entity>().GetDamage(enableHand.damage);
                        Instantiate(effect, hitInfo.point, Quaternion.identity);
                    }
                    if (hitInfo.transform.tag == "Rock")
                    {
                        hitInfo.transform.GetComponent<Collection>().Gather(enableHand.damage);
                    }
                    if (hitInfo.transform.tag == "Tree")
                    {
                        hitInfo.transform.GetComponent<Collection>().Gather(enableHand.damage);
                    }
                    if (hitInfo.transform.tag == "Water")
                    {
                        SoundController.instance.PlaySE(ThirdPersonController.bottleSound);
                        hitInfo.transform.GetComponent<Water>().GetWater(hitInfo.point);
                    }
                }
                if (hand == Weapon)
                {
                    if (hitInfo.transform.tag == "Entity")
                    {
                        SoundController.instance.PlaySE(ThirdPersonController.hitSound);
                        hitInfo.transform.GetComponent<Entity>().GetDamage(enableHand.damage);
                        Instantiate(effect, hitInfo.point, Quaternion.identity);
                    }
                }
                if (hand == Bow)
                {

                }
                if (hand == Rig)
                {
                    if (hitInfo.transform.tag == "Entity" && (enableHand.weaponName.Contains("곡괭이") || enableHand.weaponName.Contains("도끼")))
                    {
                        SoundController.instance.PlaySE(ThirdPersonController.hitSound);
                        hitInfo.transform.GetComponent<Entity>().GetDamage(enableHand.damage);
                        Instantiate(effect, hitInfo.point, Quaternion.identity);
                    }
                    if (hitInfo.transform.tag == "Rock" && enableHand.weaponName.Contains("곡괭이"))
                    {
                        hitInfo.transform.GetComponent<Collection>().Gather(enableHand.damage);
                    }
                    if (hitInfo.transform.tag == "Tree" && enableHand.weaponName.Contains("도끼"))
                    {
                        hitInfo.transform.GetComponent<Collection>().Gather(enableHand.damage);
                    }
                    if (hitInfo.transform.tag == "Object" && enableHand.weaponName.Contains("망치"))
                    {
                        hitInfo.transform.GetComponent<Construction>().SetDestroy();
                    }
                }

                isSwing = false;
            }

            yield return null;
        }
    }

    private bool CheckObject()
    {
        if (Physics.Raycast(Crosshair.position, Crosshair.TransformDirection(Vector3.forward), out hitInfo, (enableHand.range + 3f), layerMask))
        {
            return true;
        }

        return false;
    }
}
