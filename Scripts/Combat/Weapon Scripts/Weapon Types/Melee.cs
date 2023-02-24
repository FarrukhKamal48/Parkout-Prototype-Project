using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{
    [Space(5)]
    [Header("Melee Settings & Setup")]
    public KeyCode switchKey;
    public MeleeSettings oneHanded;
    public MeleeSettings twoHanded;
    private MeleeSettings Settings;

    // public Vector3 startPos;
    public Transform RightTrigger, LeftTrigger;
    public LayerMask enemyMask;

    private GameObject impactEffect;
    //bullet Force
    //Gun
    private float attackRate;
    private int maxCombo;
    private float comboResetTimer;
    private int currentCombo = 0;
    private bool allowButtonHold;
    private float triggerRadious;
    private bool aimHold;
    private float aimSpeed;
    private float idleSpeed;

    // public Transform fpsCam;
    // public GunManager gunManager;

    // animation
    [Space(5)]
    [Header("Hip Animation")]
    public Transform hipTransform;
    public Transform hip;

    [Space(5)]
    [Header("Animator")]
    public Animator gunAnimator;

    [Header("Blend Speeds")]
    public float idle_Blend_speed;
    public float shoot_Blend_speed;
    public float reload_Blend_speed;
    public float sprint_Blend_speed;

    [Header("Animation Speeds")]
    public float normalAnimSpeed; 
    public float shootAnimSpeed; 
    public float sprintAnimSpeed; 

    [Space(5)]
    [Header("Sounds")]
    public AudioSource attackSound;
    public AudioSource hitSound;
    public AudioManager audioManager;

    // private variables
    Transform enemyTransform;
    RaycastHit hitInfo;

    //bools
    
    private bool _isOneHanded;

    bool _idle, _aiming, _altADS, _shooting, _readyToShoot, _targetHit, _reloading;

    public static bool idle, aiming, altADS, shooting, readyToShoot, reloading;

    void Awake()
    {
        SetupSettings();

        _readyToShoot = true;
    }

    void Update()
    {
        // set values for settings
        SetupSettings();

        Animation();

        MyInput();
        
        // handle melee attack
        CheckHit();

        // debug stuff

        Melee.idle = _idle;
        Melee.aiming = _aiming;
        Melee.altADS = _altADS;
        Melee.shooting = _shooting;
        Melee.readyToShoot = _readyToShoot;
        Melee.reloading = _reloading;
    }

    public void GetSettings()
    {
        if (Input.GetKeyDown(switchKey))
        {
            _isOneHanded = !_isOneHanded;
        }

        Settings = _isOneHanded ? oneHanded : twoHanded;

        Weapon.isOneHanded = _isOneHanded;
    }

    public override void SetupSettings()
    {
        // get the setting depending on two handed or one handed modes
        GetSettings();

        // set all the settings
        name = Settings.name;
        impactEffect = Settings.impactEffect;
        attackRate = Settings.attackRate;
        maxCombo = Settings.maxCombo;
        comboResetTimer = Settings.comboResetTimer;
        allowButtonHold = Settings.allowButtonHold;
        triggerRadious = Settings.triggerRadious;
        aimHold = Settings.aimHold;
        aimSpeed = Settings.aimSpeed;
        idleSpeed = Settings.idleSpeed;
        normalAnimSpeed = Settings.normalAnimSpeed;
        shootAnimSpeed = Settings.shootAnimSpeed;
        sprintAnimSpeed = Settings.sprintAnimSpeed;
    }

    public override void Animation()
    {
        // if (_idle)
        // {
        //     // gunAnimator.SetFloat("Shoot Blend", Mathf.Lerp(gunAnimator.GetFloat("Shoot Blend"), 0.0f, shoot_Blend_speed * Time.deltaTime));
        //     // gunAnimator.SetFloat("Reload Blend", Mathf.Lerp(gunAnimator.GetFloat("Reload Blend"), 0.0f, reload_Blend_speed * Time.deltaTime));

        //     // if (PlayerController.walking == true)
        //     //     gunAnimator.SetFloat("Idle Blend", Mathf.Lerp(gunAnimator.GetFloat("Idle Blend"), 1f, idle_Blend_speed * Time.deltaTime));
        //     // else
        //     //     gunAnimator.SetFloat("Idle Blend", Mathf.Lerp(gunAnimator.GetFloat("Idle Blend"), 0.0f, idle_Blend_speed * Time.deltaTime));

        // }
        // else if (!_readyToShoot)
        // {
        // }


        // gunAnimator.SetFloat("NORMAL_ANIM_SPEED", normalAnimSpeed);
        // gunAnimator.SetFloat("SHOOT_ANIM_SPEED", shootAnimSpeed);
        // gunAnimator.SetFloat("SPRINT_ANIM_SPEED", sprintAnimSpeed);
        
        
        gunAnimator.SetFloat("Combo Counter", currentCombo);
        //gunAnimator.SetFloat("Reload Blend", 1.0f);

        // gunAnimator.SetBool("Aiming", _aiming);
        gunAnimator.SetBool("Attacking", !_readyToShoot);
        gunAnimator.SetBool("Sprinting", PlayerController.Sprinting);
    }

    public override void MyInput()
    {
        // setting up the bools for aim, _shooting, _idle
        if (aimHold) _aiming = Input.GetKey(KeyCode.Mouse1);
        else if (Input.GetKeyDown(KeyCode.Mouse1)) _aiming = !_aiming;

        //if (_aiming == true) _idle = false;

        if (allowButtonHold) _shooting = Input.GetKey(KeyCode.Mouse0);
        else _shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (_aiming == false /* && _reloading == false*/) _idle = true;
        else _idle = false;

        // _shooting functionallity

        if (_readyToShoot && _shooting)
        {
            Shoot();
        }
    }

    
    public override void Shoot()
    {
        // so we dont call this function over and over
        _readyToShoot = false;
        
        // attack functionality
        Debug.Log("Punch");
        
        // So we can attack again after an attackRate
        StartCoroutine(ResetAttack());
        
        
        //fireSound.Play();
        // audioManager.PlaySound("Fire");

        // PlayerController.cameraLook.GetComponent<RecoilScript>().RecoilFire(_aiming);
    }
    
    bool getEnemy(Transform origin_T)
    {
        return Physics.SphereCast(RightTrigger.position, triggerRadious, RightTrigger.forward, 
        out hitInfo, 0.2f, enemyMask);
    }
    
    int getValidCombo(int combo)
    {
        if (Settings.isRightChoosen[combo] == true)
            return combo;
        
        while(isOneHanded && !Settings.isRightChoosen[combo])
            combo++;
        
        return combo;
    }
    
    void CheckHit()
    {
        currentCombo = (int)Mathf.Clamp(currentCombo, 0, maxCombo);

        if (_readyToShoot)
        {
            currentCombo = _targetHit ? currentCombo++ : 0;
            
            // make sure that this combo does'nt request the left hand when left hand is in use
            currentCombo = getValidCombo(currentCombo);

            return;
        }
        
        if(getEnemy( Settings.isRightChoosen[currentCombo] ? RightTrigger : LeftTrigger ))
        {
            enemyTransform = hitInfo.transform;
            _targetHit = true;
            
            DamageEnemy(enemyTransform, hitInfo);
        }
        else
            _targetHit = false;
        
    }
    
    void DamageEnemy(Transform enemyTransform, RaycastHit hit_info)
    {
        // create an impact effect at the hit position, facing outwards from the surface
        GameObject newImpactEffect = Instantiate(impactEffect, hit_info.point, Quaternion.identity);
        newImpactEffect.transform.forward = hit_info.normal;
        
        Debug.Log("Enemy was hit");
    }
    
    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackRate);
        _readyToShoot = true;
    }
}
