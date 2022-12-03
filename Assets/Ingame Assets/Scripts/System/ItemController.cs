using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName;
    [Tooltip("Health, Stamina, Thirst, Eat, Rest")]
    public string[] part;
    public int[] num;
}
public class ItemController : MonoBehaviour
{
    [SerializeField] private string healthSound;
    [SerializeField] private string staminaSound;
    [SerializeField] private string thirstSound;
    [SerializeField] private string eatSound;
    [SerializeField] private string restSound;

    [SerializeField] private ItemEffect[] itemEffects;
    [SerializeField] private StatusController StatusController;

    private const string HP = "Health", SP = "Stamina", TP = "Thirst", EP = "Eat", RP = "Rest";

    public void UseItem(Item item)
    {
        for (int i = 0; i < itemEffects.Length; i++)
        {
            if (itemEffects[i].itemName == item.itemName)
            {
                for (int j = 0; j < itemEffects[i].part.Length; j++)
                {
                    switch (itemEffects[i].part[j])
                    {
                        case HP:
                            StatusController.IncreaseHealth(itemEffects[i].num[j]);
                            SoundController.instance.PlaySE(healthSound);
                            break;
                        case SP:
                            StatusController.IncreaseStamina(itemEffects[i].num[j]);
                            SoundController.instance.PlaySE(staminaSound);
                            break;
                        case TP:
                            StatusController.IncreaseThirst(itemEffects[i].num[j]);
                            SoundController.instance.PlaySE(thirstSound);
                            break;
                        case EP:
                            StatusController.IncreaseEat(itemEffects[i].num[j]);
                            SoundController.instance.PlaySE(eatSound);
                            break;
                        case RP:
                            StatusController.IncreaseRest(itemEffects[i].num[j]);
                            SoundController.instance.PlaySE(restSound);
                            break;
                        default:
                            break;
                    }
                }
                return;
            }
        }
    }
}
