using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    public int health;
    public int stamina;
    public int thirst;
    public int eat;
    public int rest;

    [SerializeField] private int staminaRegenSpeed;
    [SerializeField] private int restRegenSpeed;

    [HideInInspector] public int setStaminaRegen;
    [HideInInspector] public int setRestRegen;

    [SerializeField] private int staminaRegen;
    [SerializeField] private int restRegen;

    [SerializeField] private int thirstDecreaseSpeed;
    [SerializeField] private int eatDecreaseSpeed;
    [SerializeField] private int restDecreaseSpeed;

    [HideInInspector] public int setThirstDecrease;
    [HideInInspector] public int setEatDecrease;
    [HideInInspector] public int setRestDecrease;

    [SerializeField] private Image[] imagesGauge;

    [HideInInspector] public int setHealth;
    [HideInInspector] public int setStamina;
    [HideInInspector] public int setThirst;
    [HideInInspector] public int setEat;
    [HideInInspector] public int setRest;

    [HideInInspector] public int armor = 0;

    [HideInInspector] public bool _isStaminaDamaging;
    [HideInInspector] public bool _isRestDamaging;

    [SerializeField] private GameObject equip;

    private AnimatorController AnimatorController;

    private const int HP = 0, SP = 1, TP = 2, EP = 3, RP = 4;

    private void Start()
    {
        setHealth = health;
        setStamina = stamina;
        setThirst = thirst;
        setEat = eat;
        setRest = rest;

        AnimatorController = FindObjectOfType<AnimatorController>();
    }

    private void Update()
    {
        if (!ThirdPersonController.dead)
        {
            Thirst();
            Eat();
            Rest();
            StaminaRegen();
            RestRegen();
            StaminaRecover();
            RestRecover();
            GaugeUpdate();
        }
    }

    private void Thirst()
    {
        if (setThirst > 0)
        {
            if (setThirstDecrease <= thirstDecreaseSpeed)
            {
                setThirstDecrease++;
            }
            else
            {
                setThirst--;
                setThirstDecrease = 0;
            }

        }
        else
        {

        }
    }

    private void Eat()
    {
        if (setEat > 0)
        {
            if (setEatDecrease <= eatDecreaseSpeed)
            {
                setEatDecrease++;
            }
            else
            {
                setEat--;
                setEatDecrease = 0;
            }
        }
        else
        {

        }
    }

    private void Rest()
    {
        if (setRest > 0)
        {
            if (setRestDecrease <= restDecreaseSpeed)
            {
                setRestDecrease++;
            }
            else
            {
                if (setEat <= 0 && Day.isNight)
                {
                    setRest -= 6;
                }
                else if (Day.isNight)
                {
                    setRest -= 4;
                }
                else if (setEat <= 0)
                {
                    setRest -= 2;
                }
                else
                {
                    setRest--;
                }

                setRestDecrease = 0;
                _isRestDamaging = true;
                setRestRegen = 0;
            }
        }
        else
        {
            ThirdPersonController.dead = true;
        }
    }

    private void StaminaRecover()
    {
        if (!_isStaminaDamaging && setStamina < stamina && setRest != 0)
        {
            if (setThirst < 0)
            {
                setStamina += staminaRegen / 2;
            }
            else
            {
                setStamina += staminaRegen;
            }
        }
    }

    private void RestRecover()
    {
        if (BuildController.setBonfire && setRest < rest)
        {
            setRest += restRegen;
        }
    }

    private void StaminaRegen()
    {
        if (_isStaminaDamaging)
        {
            if (setStaminaRegen < staminaRegenSpeed)
            {
                setStaminaRegen++;
            }
            else
            {
                _isStaminaDamaging = false;
            }
        }
    }

    private void RestRegen()
    {
        if (_isRestDamaging)
        {
            if (setRestRegen < restRegenSpeed)
            {
                setRestRegen++;
            }
        }
        else
        {
            _isRestDamaging = false;
        }
    }

    public void IncreaseHealth(int count)
    {
        if (setHealth + count < health)
        {
            setHealth += count;
        }
        else
        {
            setHealth = health;
        }
    }

    public void DecreaseHealth(int count)
    {
        if (setHealth - count > 0)
        {
            if (armor != 0)
            {
                setHealth -= (int)(count / armor);
            }
            else
            {
                setHealth -= count;
            }

            AnimatorController.LoadHurtAnimation(true);
        }
        else
        {
            ThirdPersonController.dead = true;
            setHealth = 0;
        }
    }

    public void IncreaseStamina(int count)
    {
        if (setStamina + count < stamina)
        {
            setStamina += count;
        }
        else
        {
            setStamina = stamina;
        }
    }

    public void DecreaseStamina(int count)
    {
        _isStaminaDamaging = true;
        setStaminaRegen = 0;

        if (setStamina - count > 0)
        {
            setStamina -= count;
        }
        else
        {
            setStamina = 0;
        }
    }

    public void IncreaseThirst(int count)
    {
        if (setThirst + count < thirst)
        {
            setThirst += count;
        }
        else
        {
            setThirst = thirst;
        }
    }

    public void DecreaseThirst(int count)
    {
        if (setThirst - count > 0)
        {
            setThirst -= count;
        }
        else
        {
            setThirst = 0;
        }
    }

    public void IncreaseEat(int count)
    {
        if (setEat + count < eat)
        {
            setEat += count;
        }
        else
        {
            setEat = eat;
        }
    }

    public void DecreaseEat(int count)
    {
        if (setEat - count > 0)
        {
            setEat -= count;
        }
        else
        {
            setEat = 0;
        }
    }

    public void IncreaseRest(int count)
    {
        if (setRest + count < eat)
        {
            setRest += count;
        }
        else
        {
            setRest = eat;
        }
    }

    public void DecreaseRest(int count)
    {
        if (setRest - count > 0)
        {
            setRest -= count;
        }
        else
        {
            setRest = 0;
        }
    }

    private void GaugeUpdate()
    {
        imagesGauge[HP].fillAmount = (float)setHealth / health;
        imagesGauge[SP].fillAmount = (float)setStamina / stamina;
        imagesGauge[TP].fillAmount = (float)setThirst / thirst;
        imagesGauge[EP].fillAmount = (float)setEat / eat;
        imagesGauge[RP].fillAmount = (float)setRest / rest;
    }
}
