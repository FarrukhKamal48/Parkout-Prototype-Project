using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : Weapon
{
    [Space(5)]
    [Header("Gun Settings & Setup")]
    // public GunSettings Settings;
    public KeyCode switchKey;
    public GunSettings oneHanded;
    public GunSettings twoHanded;

    public GunSettings Settings;
    
    public WeaponReferences weaponRefs;
    
    [Header("Scripts")]
    public sway swayScript;

    // public Vector3 startPos;

    //bullet
    private GameObject bullet;
    private GameObject muzzleFlash;
    //bullet Force
    private float shootForce;
    private float upwardForce;
    //Gun
    private float fireRate, shootWarmUp, sprintToFireDelay, spread, reloadTime, timeBetweenShots;
    private int magSize, bulletsPerTap;
    private bool allowButtonHold;
    private bool aimHold;
    private bool altADShold;
    private float aimSpeed;
    private float idleSpeed;
    private float crouchSpeed;

    [SerializeField] private Transform attackPoint;
    // public Transform fpsCam;
    // public GunManager gunManager;

    // animation
    [Space(5)]
    [Header("Hip Animation")]
    public Transform hipTransform;
    public Transform hip;

    [Space(5)]
    [Header("ADS Animation")]
    public Transform adsRotator;
    public Transform ADS;
    public Transform t_altADS;
    
    [Space(5)]
    [Header("Crouch Animation")]
    public Transform crouch;

    [Space(5)]
    [Header("Shoot Animation")]
    public Transform RecoilPoint;

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
    public float reloadAnimSpeed; 
    public float sprintAnimSpeed; 

    [Space(5)]
    [Header("Sounds")]
    public AudioSource fireSound;
    public AudioManager audioManager;

    // gun clip
    int bulletsLeft, bulletsShot;

    //bools
    private bool _isOneHanded;
    
    private bool isSprintShootDelay = false;

    bool _idle, _aiming, _altADS, _shooting, _readyToShoot, _reloading;

    public static bool idle, aiming, altADS, shooting, readyToShoot, reloading;

    public bool allowInvoke = true;
    
    // private variables

    void Start()
    {
        bulletsLeft = magSize;
        _readyToShoot = true;
        gunAnimator.SetBool("Shooting", false);

        if (gunManager.switching)
            return;

        SetupSettings();

    }

    void Update()
    {
        if (gunManager.switching)
            return;

        SetupSettings();

        Animation();
        manageSounds();

        MyInput();
        HandleShootDelay();
        
        // debug stuff

        ProjectileGun.idle = _idle;
        ProjectileGun.aiming = _aiming;
        ProjectileGun.altADS = _altADS;
        ProjectileGun.shooting = _shooting;
        ProjectileGun.readyToShoot = _readyToShoot;
        ProjectileGun.reloading = _reloading;
    }

    public void GetSettings()
    {
        if (Input.GetKeyDown(switchKey))
        {
            _isOneHanded = !_isOneHanded;
        }

        // Settings = _isOneHanded ? oneHanded : twoHanded;
        Settings = twoHanded;

        Weapon.isOneHanded = _isOneHanded;
    }

    public override void SetupSettings()
    {
        // get the setting depending on two handed or one handed modes
        // GetSettings();
        Settings = twoHanded;

        // set all the gun settings
        
        
        // gun stats
        weaponRefs.name = Settings.name;
        bullet = Settings.bullet;
        muzzleFlash = Settings.muzzleFlash;
        shootForce = Settings.shootForce;
        upwardForce = Settings.upwardForce;
        fireRate = Settings.fireRate;
        shootWarmUp = Settings.shootWarmUp;
        sprintToFireDelay = Settings.sprintToFireDelay;
        spread = Settings.spread;
        reloadTime = Settings.reloadTime;
        timeBetweenShots = Settings.timeBetweenShots;
        magSize = Settings.magSize;
        bulletsPerTap = Settings.bulletsPerTap;
        attackPoint = weaponRefs.attackPoint;

        // key settings
        allowButtonHold = Settings.allowButtonHold;
        aimHold = Settings.aimHold;
        altADShold = Settings.altADShold;

        // animator speeds
        normalAnimSpeed = Settings.normalAnimSpeed;
        shootAnimSpeed = Settings.shootAnimSpeed;
        reloadAnimSpeed = Settings.reloadAnimSpeed;
        sprintAnimSpeed = Settings.sprintAnimSpeed;

        // blend tree smoothing
        idle_Blend_speed = Settings.idle_Blend_speed;
        shoot_Blend_speed = Settings.shoot_Blend_speed;
        reload_Blend_speed = Settings.reload_Blend_speed;
        sprint_Blend_speed = Settings.sprint_Blend_speed;
        
        // proc animation transforms
        hipTransform = weaponRefs.anchor_T;
        hip = weaponRefs.hip_T;

        adsRotator = weaponRefs.adsRotator;
        ADS = weaponRefs.ADS_T;
        t_altADS = weaponRefs.altADS_T;
        
        crouch = weaponRefs.crouch_T;

        // gunAnimator = weaponRefs.gunAnimator;
        // audioManager = weaponRefs.audioManager;
    }
    
    void resetCurveAnimFloats()
    {
        if (!_idle || _aiming || PlayerController.crouching)
        {
            Settings.IdleSpeed.p = 0f;
        }
        
        if (!_aiming)
        {
            Settings.AimSpeed.p = 0f;
            Settings.aimAnim.p = 0f;
        }
        
        if (!PlayerController.crouching || _aiming)
        {
            Settings.CrouchSpeed.p = 0f;
        }
    }
    
    
    void manageSounds() {
        
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            audioManager.PlaySound("Aim_In");
            audioManager.StopSound("Aim_Out");
        }
        if (Input.GetKeyUp(KeyCode.Mouse1)) {
            audioManager.StopSound("Aim_In");
            audioManager.PlaySound("Aim_Out");
        }
    }

    public override void Animation()
    {
        
        // reset procedural animation
        resetCurveAnimFloats();

        if (_idle)
        {
            if (!PlayerController.crouching)
            {
                // lerp to hip position
                float _anim_speed = WeaponAnimation.procAnimate(ref Settings.IdleSpeed.p, 1f, Settings.IdleSpeed.AnimationMotionCurve,
                    Settings.IdleSpeed.magnitude, Settings.IdleSpeed.animationSpeed);

                WeaponAnimation.Animate(hipTransform, hip, _anim_speed, WeaponAnimation.Animatemode._transform);
            }
            else
            {
				// lerp to crouch position
                float _anim_speed = WeaponAnimation.procAnimate(ref Settings.CrouchSpeed.p, 1f, Settings.CrouchSpeed.AnimationMotionCurve, 
                    Settings.CrouchSpeed.magnitude, Settings.CrouchSpeed.animationSpeed);

                WeaponAnimation.Animate(hipTransform, crouch, _anim_speed, WeaponAnimation.Animatemode._transform);
            }

            // set blend tree floats
            gunAnimator.SetFloat("Shoot Blend", Mathf.Lerp(gunAnimator.GetFloat("Shoot Blend"), 0.0f, shoot_Blend_speed * Time.deltaTime));
            gunAnimator.SetFloat("Reload Blend", Mathf.Lerp(gunAnimator.GetFloat("Reload Blend"), 0.0f, reload_Blend_speed * Time.deltaTime));

            // blend walking animations
            if (PlayerController.walking == true)
                gunAnimator.SetFloat("Idle Blend", Mathf.Lerp(gunAnimator.GetFloat("Idle Blend"), 2f, idle_Blend_speed * Time.deltaTime));
            else if (PlayerController.crouchWalking == true)
                gunAnimator.SetFloat("Idle Blend", Mathf.Lerp(gunAnimator.GetFloat("Idle Blend"), 1f, idle_Blend_speed * Time.deltaTime));
            else
                gunAnimator.SetFloat("Idle Blend", Mathf.Lerp(gunAnimator.GetFloat("Idle Blend"), 0f, idle_Blend_speed * Time.deltaTime));

        }
        else if (_aiming)
        {
            // lerp to aim postion
            float _anim_speed = WeaponAnimation.procAnimate(ref Settings.AimSpeed.p, 1f, Settings.AimSpeed.AnimationMotionCurve, 
                Settings.AimSpeed.magnitude, Settings.AimSpeed.animationSpeed);

            WeaponAnimation.Animate(hipTransform, ADS, _anim_speed, WeaponAnimation.Animatemode._transform);

            // WeaponAnimation.Animate(hipTransform, hipTransform, ADS, 
            //     ref Settings.aimAnim, WeaponAnimation.Animatemode._position);

            
            // if (_altADS)
            //     WeaponAnimation.Animate(adsRotator, t_altADS, aimSpeed, WeaponAnimation.Animatemode._rotation);
            // else
            //     WeaponAnimation.Animate(adsRotator, ADS, aimSpeed, WeaponAnimation.Animatemode._rotation);

            // set blend tree floats
            gunAnimator.SetFloat("Shoot Blend", Mathf.Lerp(gunAnimator.GetFloat("Shoot Blend"), 1.0f, shoot_Blend_speed * Time.deltaTime));
            gunAnimator.SetFloat("Reload Blend", Mathf.Lerp(gunAnimator.GetFloat("Reload Blend"), 1.0f, reload_Blend_speed * Time.deltaTime));

            // blend walking animations
            if (PlayerController.aimWalking == true)
                gunAnimator.SetFloat("Idle Blend", Mathf.Lerp(gunAnimator.GetFloat("Idle Blend"), -1f, idle_Blend_speed * Time.deltaTime));
            else
                gunAnimator.SetFloat("Idle Blend", Mathf.Lerp(gunAnimator.GetFloat("Idle Blend"), 0.0f, idle_Blend_speed * Time.deltaTime));
        }


        // animation speeds
        gunAnimator.SetFloat("NORMAL_ANIM_SPEED", normalAnimSpeed);
        gunAnimator.SetFloat("SHOOT_ANIM_SPEED", shootAnimSpeed);
        gunAnimator.SetFloat("RELOAD_ANIM_SPEED", reloadAnimSpeed);
        gunAnimator.SetFloat("SPRINT_ANIM_SPEED", sprintAnimSpeed);
        

        // setting animator parameters

        gunAnimator.SetBool("Switching In", gunManager.switchingIn);
        gunAnimator.SetBool("Switching Out", gunManager.switchingOut);

        gunAnimator.SetBool("Aiming", _aiming);
        gunAnimator.SetBool("Shooting", !_readyToShoot);
        gunAnimator.SetBool("Reloading", _reloading);
        gunAnimator.SetBool("Sprinting", PlayerController.Sprinting);
    }

    public override void MyInput() { 
        
        // setting up the bools for aim, _shooting, _idle
        if (aimHold && !isOneHanded) _aiming = Input.GetKey(KeyCode.Mouse1);
        else if (Input.GetKeyDown(KeyCode.Mouse1) && !isOneHanded) _aiming = !_aiming;

        if (altADShold) _altADS = Input.GetKey(KeyCode.X);
        else if (Input.GetKeyDown(Settings.altAdsKey)) _altADS = !_altADS;

        //if (_aiming == true) _idle = false;

        if (allowButtonHold) _shooting = Input.GetKey(KeyCode.Mouse0);
        else _shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (_aiming == false /* && _reloading == false*/) _idle = true;
        else _idle = false;

        // _shooting functionallity
        if (Input.GetKeyDown(Settings.ReloadKey) && bulletsLeft < magSize && _reloading == false) Reload();

        if (_readyToShoot && _shooting && _reloading == false && bulletsLeft <= 0) Reload();

        if (_shooting && _reloading == false && isSprintShootDelay == false && gunManager.switchingIn == false)
            warmUpShoot();
        else
            elapsedWarmUp = 0f;

    }
    
    float elapsedWarmUp;
    void warmUpShoot()
    {
        if (elapsedWarmUp < shootWarmUp)
        { 
            elapsedWarmUp += Time.deltaTime;
            Debug.Log("Warming");
        }
        else if (_readyToShoot && bulletsLeft > 0) 
        {
            bulletsShot = 0;
            Shoot();
            shootEffects(); // so effects like particles and sound run once
        }
    }
    
    void HandleShootDelay() {

        // sprint to shoot delay
        if (PlayerController.Sprinting == true) {
            timer_sprintShoot = 0f;
            isSprintShootDelay = true;
        } 
        if (PlayerController.Sprinting == false) stopSprintShootDelay();
        
        // weapon switching delay
         
    }

    float timer_sprintShoot = 0f;
    void stopSprintShootDelay() {

        if (timer_sprintShoot < sprintToFireDelay) timer_sprintShoot += Time.deltaTime;
        if (timer_sprintShoot >= sprintToFireDelay) isSprintShootDelay = false;

    } 
     
    void shootEffects()
    {
        // creating the muzzleFlash
        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        // play the shoot sound
        audioManager.PlaySound("Fire");

        // apply recoil
        PlayerController.cameraLook.GetComponent<RecoilScript>().RecoilFire(_aiming);
    }

    public override void Shoot()
    {
        //fireSound.Stop();

        _readyToShoot = false;

        Vector3 shootDirnoSpread = fpsCam.forward;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 shootDirspread = shootDirnoSpread + new Vector3(x, y, 0);

        GameObject currentBullet = Instantiate(bullet, fpsCam.position, Quaternion.identity); // making the invisible bullet
        currentBullet.GetComponent<CustomBullet>().muzzlePoint = attackPoint;

        currentBullet.transform.forward = shootDirspread.normalized; // applying the Random direction to the invisible bullet

        Rigidbody bulletRB = currentBullet.GetComponent<Rigidbody>();

        //bulletRB.velocity = player.rb.velocity;
        bulletRB.AddForce(shootDirspread.normalized * shootForce, ForceMode.Impulse);
        bulletRB.AddForce(fpsCam.up * upwardForce, ForceMode.Impulse);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", fireRate);
            allowInvoke = false;
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);

    }

    void ResetShot()
    {
        _readyToShoot = true;
        allowInvoke = true;
    }

    void Reload()
    {
        _reloading = true;
        elapsedWarmUp = 0f;
        CancelReload();
        Invoke("ReloadFinished", reloadTime);

    }

    void CancelReload()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            _reloading = false;
            CancelInvoke();
        }
    }

    void playSound(Transform source_t, AudioClip clip)
    {
        AudioSource source = source_t.gameObject.AddComponent<AudioSource>();
        source.clip = clip;
    }

    void ReloadFinished()
    {
        bulletsLeft = magSize;
        _reloading = false;
    }
    
}
