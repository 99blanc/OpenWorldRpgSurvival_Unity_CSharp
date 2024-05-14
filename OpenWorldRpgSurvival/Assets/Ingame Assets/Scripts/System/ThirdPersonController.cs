using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [HideInInspector] public static ThirdPersonController instance;

    [SerializeField] private float runSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float fallDistance;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;

    [SerializeField] private int sprintDecrease;
    [SerializeField] private int dodgeDecrease;
    [SerializeField] private int jumpDecrease;
    [SerializeField] private int attackDecrease;
    [SerializeField] private int respawnDecrease;

    private bool isRunning, isSprinting, isDragging, isScrolling, isCursoring, isDodging, isFalling, isJumping, isAttacking, isBlocking = false;
    public static bool root, delay, boot, reload, block, dead, spawn, pause, isSwimming = false;

    [SerializeField] private Transform player;
    [SerializeField] private Transform view;
    [SerializeField] private GameObject attackEvent;

    [SerializeField] private string walkSound;
    [SerializeField] private string sprintSound;
    [SerializeField] private string dodgeSound;
    [SerializeField] private string jumpSound;
    [SerializeField] private string landSound;
    [SerializeField] private string handSound;
    public string bottleSound;
    public string blockSound;
    public string hitSound;
    public string hurtSound;
    [SerializeField] private string waterEnterSound;
    [SerializeField] private string waterExitSound;

    private AttackController AttackController;
    private AnimatorController AnimatorController;
    private StatusController StatusController;
    private PauseController PauseController;
    private InventoryController InventoryController;
    private SaveAndLoadController SaveAndLoadController;
    private Rigidbody rigid;
    private Camera fow;
    private Vector2 _posS;
    private Vector3 _posT;
    private Vector3 _spawnPos = new Vector3(80f, 4f, 70f);

    private const int Hand = 0, Weapon = 1, Bow = 2, Rig = 3;


    private void Start()
    {
        SoundController.instance.StopBackgroundSE("Title");
        SoundController.instance.PlayBackgroundSE();

        instance = this;
        rigid = GetComponent<Rigidbody>();
        AttackController = attackEvent.GetComponent<AttackController>();
        AnimatorController = GetComponent<AnimatorController>();
        StatusController = FindObjectOfType<StatusController>();
        PauseController = FindObjectOfType<PauseController>();
        InventoryController = FindObjectOfType<InventoryController>();
        fow = GetComponentInChildren<Camera>();

        if (SaveAndLoadController.instance.button)
        {
            SaveAndLoadController.instance.LoadData();
        }

        Debug.Log("Player Setting : " + SaveAndLoadController.instance.button);
    }

    private void FixedUpdate()
    {
        InputKey();
    }

    private void Update()
    {
        Pause();
        Stand();

        if (!pause)
        {
            if (!dead)
            {
                LookAt();

                if (!isAttacking && !isBlocking)
                {
                    Move();
                }

                Lock();
                Dodge();
                Fall();
                Jump();

                if (!InventoryController.inventoryActive && !InventoryController.gearActive && !BuildController.buildActive)
                {
                    Look();
                    Attack();
                    Block();
                }
            }
            else
            {
                if (!spawn)
                {
                    spawn = true;
                    SoundController.instance.StopBackgroundSE();
                    AnimatorController.LoadDeadAnimation(dead);
                    Respawn();
                }
            }
        }
    }

    private void LateUpdate()
    {
        Zoom();
    }

    private void InputKey()
    {
        isRunning = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        isSprinting = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && StatusController.setStamina >= sprintDecrease;
        isDragging = Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0;
        isScrolling = Input.GetAxis("Mouse ScrollWheel") != 0;
        isCursoring = Input.GetKey(KeyCode.LeftControl);
        isDodging = Input.GetKey(KeyCode.LeftAlt) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) && !isAttacking && !isBlocking;
        isJumping = Input.GetKey(KeyCode.Space) && !isAttacking && !isBlocking;
        isAttacking = Input.GetKey(KeyCode.Mouse0) && (!InventoryController.inventoryActive && !InventoryController.gearActive);
        isBlocking = (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.V)) && (!InventoryController.inventoryActive && !InventoryController.gearActive);
    }

    private bool Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            if (!pause)
            {
                Time.timeScale = 0;
                PauseController.CallMenu();
                root = true;
                Cursor.lockState = CursorLockMode.None;
                pause = true;
                return pause;
            }
            else
            {
                Time.timeScale = 1;
                PauseController.CloseMenu();
                root = false;
                Cursor.lockState = CursorLockMode.Locked;
                pause = false;
                return pause;
            }
        }

        return false;
    }

    private void Stand()
    {
        Vector2 _stand = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        bool isStanding = _stand.magnitude != 0;

        if (!isStanding)
        {
            SoundController.instance.StopSE(walkSound);
            SoundController.instance.StopSE(sprintSound);
        }
    }

    private void Respawn()
    {
        if (dead)
        {
            StartCoroutine(LogicRespawn());
        }
    }

    private IEnumerator LogicRespawn()
    {
        InventoryController.DropItem();

        yield return new WaitForSeconds(respawnDecrease);

        StatusController.setHealth = StatusController.health;
        StatusController.setStamina = StatusController.stamina;
        StatusController.setThirst = StatusController.thirst;
        StatusController.setEat = StatusController.eat;
        StatusController.setRest = StatusController.rest;

        dead = false;
        spawn = false;

        AnimatorController.LoadDeadAnimation(dead);
        AnimatorController.LoadHurtAnimation(spawn);
        SoundController.instance.PlayBackgroundSE();

        yield return new WaitForSeconds(0.5f);

        transform.position = _spawnPos;
    }

    private void Move()
    {
        _posS = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 _lookF = new Vector3(view.forward.x, 0f, view.forward.z).normalized;
        Vector3 _lookR = new Vector3(view.right.x, 0f, view.right.z).normalized;
        Vector3 _dir = _lookF * _posS.y + _lookR * _posS.x;
        player.transform.forward = Vector3.Lerp(player.transform.forward, _lookF, 10f * Time.deltaTime);

        if (isRunning)
        {
            AnimatorController.LoadRunAnimation(isRunning, _posS.x, _posS.y);
            AnimatorController.LoadSprintAnimation(isSprinting);
            transform.position += _dir * Time.deltaTime * runSpeed;
        }
        if (isSprinting && !delay && StatusController.setStamina >= sprintDecrease)
        {
            AnimatorController.LoadRunAnimation(isRunning, _posS.x, _posS.y);
            AnimatorController.LoadSprintAnimation(isSprinting);
            transform.position += _dir * Time.deltaTime * sprintSpeed;
            StatusController.DecreaseStamina(sprintDecrease);
        }
        else
        {
            AnimatorController.LoadRunAnimation(isRunning, _posS.x, _posS.y);
            AnimatorController.LoadSprintAnimation(isSprinting);
        }
    }

    private void LookAt()
    {
        Vector3 _lookF = new Vector3(view.forward.x, 0f, view.forward.z).normalized;
        player.transform.forward = Vector3.Lerp(player.transform.forward, _lookF, 10f * Time.deltaTime);
    }

    private void Look()
    {
        if (isDragging)
        {
            _posS = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            _posT = view.rotation.eulerAngles;
            float _x = _posT.x - _posS.y;

            if (_x < 180f)
            {
                _x = Mathf.Clamp(_x, -1f, 70f);
            }
            else
            {
                _x = Mathf.Clamp(_x, 335f, 361f);
            }

            view.rotation = Quaternion.Euler(_posT.x - _posS.y, _posT.y + _posS.x, _posT.z);
        }
    }

    private void Lock()
    {
        if (isCursoring || root || pause)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Zoom()
    {
        if (isScrolling)
        {
            float _dis = Input.GetAxis("Mouse ScrollWheel") * -1f * zoomSpeed;

            if (fow.fieldOfView >= minZoom && fow.fieldOfView <= maxZoom)
            {
                fow.fieldOfView += _dis;
            }
            else
            {
                if (fow.fieldOfView <= minZoom)
                {
                    fow.fieldOfView = minZoom + 1f;
                }
                if (fow.fieldOfView >= maxZoom)
                {
                    fow.fieldOfView = maxZoom - 1f;
                }
            }
        }
    }

    private void Dodge()
    {
        Vector3 _lookF = new Vector3(view.forward.x, 0f, view.forward.z).normalized;

        if (isDodging && !isSprinting && !boot && StatusController.setStamina >= dodgeDecrease)
        {
            boot = true;
            isDodging = true;
            isAttacking = false;

            if (Input.GetKey(KeyCode.W))
            {
                rigid.AddForce(_lookF * dodgeSpeed, ForceMode.Impulse);
            }
            if (Input.GetKey(KeyCode.S))
            {
                rigid.AddForce(-_lookF * dodgeSpeed, ForceMode.Impulse);
            }

            _posS = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            SoundController.instance.PlaySE(dodgeSound);
            StatusController.DecreaseStamina(dodgeDecrease);
            AnimatorController.LoadDodgeAnimation(isDodging, _posS.x, _posS.y);
            StartCoroutine(LogicDodge());
        }
    }

    private IEnumerator LogicDodge()
    {
        yield return new WaitForSeconds(1f);

        boot = false;
        isDodging = false;
    }

    private void Fall()
    {
        _posT = player.transform.position;

        if (!isFalling && !Physics.Raycast(new Vector3(_posT.x, _posT.y + 0.5f, _posT.z), Vector3.down, fallDistance))
        {
            isFalling = true;
            AnimatorController.LoadFallAnimation(isFalling);
            StartCoroutine(LogicFall());
        }
        if (Physics.Raycast(new Vector3(_posT.x, _posT.y + 0.5f, _posT.z), Vector3.down, fallDistance))
        {
            isFalling = false;
            AnimatorController.LoadFallAnimation(isFalling);
        }
    }

    private IEnumerator LogicFall()
    {
        yield return new WaitForSeconds(3f);

        if (isFalling)
        {
            StatusController.DecreaseHealth(1000);
        }
    }

    private void Jump()
    {
        if (isJumping && !isSprinting && !delay && StatusController.setStamina >= jumpDecrease)
        {
            delay = true;
            isJumping = true;
            rigid.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            _posS = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            SoundController.instance.PlaySE(jumpSound);
            StatusController.DecreaseStamina(jumpDecrease);
            AnimatorController.LoadJumpAnimation(isJumping, _posS.x, _posS.y);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "World" || collision.gameObject.tag == "Object")
        {
            delay = false;
            isJumping = false;
            AnimatorController.LoadJumpAnimation(isJumping, _posS.x, _posS.y);
            SoundController.instance.PlaySE(landSound);
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "World" || collision.gameObject.tag == "Object")
        {
            if (isRunning && !isBlocking)
            {
                SoundController.instance.MoveSE(isRunning, isSprinting);
            }
            else
            {
                if (isBlocking)
                {
                    SoundController.instance.MoveSE(false, false);
                }
                else
                {
                    SoundController.instance.MoveSE(isRunning, isSprinting);
                }
            }
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Water")
        {
            SoundController.instance.PlaySE(waterEnterSound);
            isSwimming = true;
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Water")
        {
            SoundController.instance.PlaySE(waterExitSound);
            isSwimming = false;
        }
    }

    private void Attack()
    {
        if (StatusController.setStamina >= attackDecrease)
        {
            if (EquipController.weapon == Hand)
            {
                if (isAttacking && !reload)
                {
                    AnimatorController.LoadAttackAnimation(isAttacking, Hand);
                    reload = true;
                }
                if (!isAttacking && reload)
                {
                    AnimatorController.LoadAttackAnimation(isAttacking, Hand);
                    reload = false;
                }
                if (isAttacking && AnimatorController.pAnimator.IsInTransition(0) && !AttackController.isAttack)
                {
                    AttackController.TryAttack(Hand);
                    SoundController.instance.PlaySE(handSound);
                }
            }
            if (EquipController.weapon == Weapon)
            {
                if (isAttacking && !reload)
                {
                    AnimatorController.LoadAttackAnimation(isAttacking, Weapon);
                    reload = true;
                }
                if (!isAttacking && reload)
                {
                    AnimatorController.LoadAttackAnimation(isAttacking, Weapon);
                    reload = false;
                }
                if (isAttacking && AnimatorController.pAnimator.IsInTransition(0) && !AttackController.isAttack)
                {
                    AttackController.TryAttack(Weapon);
                    SoundController.instance.PlaySE(handSound);
                }
            }
            if (EquipController.weapon == Bow)
            {
                if (isAttacking && !reload)
                {
                    AnimatorController.LoadAttackAnimation(isAttacking, Bow);
                    reload = true;
                }
                if (!isAttacking && reload)
                {
                    AnimatorController.LoadAttackAnimation(isAttacking, Bow);
                    reload = false;
                }
                if (isAttacking && AnimatorController.pAnimator.IsInTransition(0) && !AttackController.isAttack)
                {
                    AttackController.TryAttack(Bow);
                }
            }
            if (EquipController.weapon == Rig)
            {
                if (isAttacking && !reload)
                {
                    AnimatorController.LoadAttackAnimation(isAttacking, Rig);
                    reload = true;
                }
                if (!isAttacking && reload)
                {
                    AnimatorController.LoadAttackAnimation(isAttacking, Rig);
                    reload = false;
                }
                if (isAttacking && AnimatorController.pAnimator.IsInTransition(0) && !AttackController.isAttack)
                {
                    AttackController.TryAttack(Rig);
                    SoundController.instance.PlaySE(handSound);
                }
            }
        }
        else
        {
            AnimatorController.LoadAttackAnimation(false, Hand);
            AnimatorController.LoadAttackAnimation(false, Weapon);
            AnimatorController.LoadAttackAnimation(false, Bow);
            AnimatorController.LoadAttackAnimation(false, Rig);
        }
    }

    private void Block()
    {
        if (isBlocking && !block)
        {
            AnimatorController.LoadBlockAnimation(isBlocking);
            block = true;
        }
        if (isBlocking)
        {
            block = true;
        }
        else
        {
            AnimatorController.LoadBlockAnimation(isBlocking);
            block = false;
        }
    }

    public bool GetRun()
    {
        if (isRunning)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetSprint()
    {
        if (isSprinting)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetDrag()
    {
        if (isDragging)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetCursor()
    {
        if (isCursoring)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetDodge()
    {
        if (isDodging)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetAttack()
    {
        if (isAttacking)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetBlock()
    {
        if (isBlocking)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
