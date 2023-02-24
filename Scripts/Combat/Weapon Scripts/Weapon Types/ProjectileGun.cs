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

    // public Vector3 startPos;

    //bullet
    private GameObject bullet;
    private GameObject muzzleFlash;
    //bullet Force
    private float shootForce;
    private float upwardForce;
    //Gun
    private float fireRate, shootWarmUp, spread, reloadTime, timeBetweenShots;
    private int magSize, bulletsPerTap;
    private bool allowButtonHold;
    private bool aimHold;
    private bool altADShold;
    private float aimSpeed;
    private float idleSpeed;

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

    bool _idle, _aiming, _altADS, _shooting, _readyToShoot, _reloading;

    public static bool idle, aiming, altADS, shooting, readyToShoot, reloading;

    public bool allowInvoke = true;

    void Awake()
    {
        SetupSettings();

        bulletsLeft = magSize;
        _readyToShoot = true;
    }

    void Update()
    {
        SetupSettings();

        Animation();

        MyInput();

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

        Settings = _isOneHanded ? oneHanded : twoHanded;

        Weapon.isOneHanded = _isOneHanded;
    }

    public override void SetupSettings()
    {
        // get the setting depending on two handed or one handed modes
        GetSettings();

        // set all the settings
        name = Settings.name;
        bullet = Settings.bullet;
        muzzleFlash = Settings.muzzleFlash;
        shootForce = Settings.shootForce;
        upwardForce = Settings.upwardForce;
        fireRate = Settings.fireRate;
        shootWarmUp = Settings.shootWarmUp;
        spread = Settings.spread;
        reloadTime = Settings.reloadTime;
        timeBetweenShots = Settings.timeBetweenShots;
        magSize = Settings.magSize;
        bulletsPerTap = Settings.bulletsPerTap;
        allowButtonHold = Settings.allowButtonHold;
        aimHold = Settings.aimHold;
        altADShold = Settings.altADShold;
        aimSpeed = Settings.aimSpeed;
        idleSpeed = Settings.idleSpeed;
        normalAnimSpeed = Settings.normalAnimSpeed;
        shootAnimSpeed = Settings.shootAnimSpeed;
        reloadAnimSpeed = Settings.reloadAnimSpeed;
        sprintAnimSpeed = Settings.sprintAnimSpeed;
    }

    public override void Animation()
    {
        if (_idle)
        {
            WeaponAnimation.Animate(hipTransform, hip, idleSpeed, WeaponAnimation.Animatemode._transform);
            WeaponAnimation.Animate(adsRotator, hip, idleSpeed, WeaponAnimation.Animatemode._rotation);

            gunAnimator.SetFloat("Shoot Blend", Mathf.Lerp(gunAnimator.GetFloat("Shoot Blend"), 0.0f, shoot_Blend_speed * Time.deltaTime));
            gunAnimator.SetFloat("Reload Blend", Mathf.Lerp(gunAnimator.GetFloat("Reload Blend"), 0.0f, reload_Blend_speed * Time.deltaTime));

            if (PlayerController.walking == true)
                gunAnimator.SetFloat("Idle Blend", Mathf.Lerp(gunAnimator.GetFloat("Idle Blend"), 1f, idle_Blend_speed * Time.deltaTime));
            else
                gunAnimator.SetFloat("Idle Blend", Mathf.Lerp(gunAnimator.GetFloat("Idle Blend"), 0.0f, idle_Blend_speed * Time.deltaTime));
        }
        else if (_aiming)
        {
            WeaponAnimation.Animate(hipTransform, ADS, aimSpeed, WeaponAnimation.Animatemode._transform);

            if (_altADS)
                WeaponAnimation.Animate(adsRotator, t_altADS, aimSpeed, WeaponAnimation.Animatemode._rotation);
            else
                WeaponAnimation.Animate(adsRotator, ADS, aimSpeed, WeaponAnimation.Animatemode._rotation);

            gunAnimator.SetFloat("Shoot Blend", Mathf.Lerp(gunAnimator.GetFloat("Shoot Blend"), 1.0f, shoot_Blend_speed * Time.deltaTime));
            gunAnimator.SetFloat("Reload Blend", Mathf.Lerp(gunAnimator.GetFloat("Reload Blend"), 1.0f, reload_Blend_speed * Time.deltaTime));

            if (PlayerController.aimWalking == true)
                gunAnimator.SetFloat("Idle Blend", Mathf.Lerp(gunAnimator.GetFloat("Idle Blend"), -1f, idle_Blend_speed * Time.deltaTime));
            else
                gunAnimator.SetFloat("Idle Blend", Mathf.Lerp(gunAnimator.GetFloat("Idle Blend"), 0.0f, idle_Blend_speed * Time.deltaTime));
        }

        // if (_idle || _aiming || _altADS)
        //     animatorSpeed = 1f;
        // else if (!_readyToShoot)
        //     animatorSpeed = fireRate;
        // else if (_reloading)
        //     animatorSpeed = reloadTime;
        // else if (PlayerController.Sprinting)
        //     animatorSpeed = sprintAnimSpeed;
        // else
        //     animatorSpeed = 1f;
        //
        // gunAnimator.speed = animatorSpeed

        gunAnimator.SetFloat("NORMAL_ANIM_SPEED", normalAnimSpeed);
        gunAnimator.SetFloat("SHOOT_ANIM_SPEED", shootAnimSpeed);
        gunAnimator.SetFloat("RELOAD_ANIM_SPEED", reloadAnimSpeed);
        gunAnimator.SetFloat("SPRINT_ANIM_SPEED", sprintAnimSpeed);
        
        //gunAnimator.SetFloat("Reload Blend", 1.0f);

        gunAnimator.SetBool("Aiming", _aiming);
        gunAnimator.SetBool("Shooting", !_readyToShoot);
        gunAnimator.SetBool("Reloading", _reloading);
        gunAnimator.SetBool("Sprinting", PlayerController.Sprinting);
    }

    public override void MyInput()
    {
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

        if (_shooting && _reloading == false)
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
        else
        {
            Debug.Log("Complete");
            if (_readyToShoot && bulletsLeft > 0)
            {
                bulletsShot = 0;
                Shoot();
                shootEffects(); // so effects like particles and sound run once
            }
        }
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

        currentBullet.transform.forward = shootDirspread.normalized; // applying Random direction to the invisible bullet

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
