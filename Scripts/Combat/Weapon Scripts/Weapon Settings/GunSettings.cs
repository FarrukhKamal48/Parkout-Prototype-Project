using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunSettings", menuName = "WeaponSettings/GunSettings", order = 0)]
public class GunSettings : ScriptableObject
{
    [Header("Prefabs")]
    public new string name;
    public GameObject weaponObject;
    public GameObject bullet;
    public GameObject muzzleFlash;
    
    [Header("Scripts")]
    public WeaponReferences weaponRefs;
    public AudioManager audioManager;

    [Space(10)]
    [Header("Controls")]
    public KeyCode ReloadKey;
    public KeyCode altAdsKey;

    [Space(10)]
    [Header("Bullet Force")]
    public float shootForce;
    public float upwardForce;


    [Space(10)]
    [Header("Shoot Settings")]
    public float fireRate;
    public float shootWarmUp;
    public float sprintToFireDelay = 1f;
    public float spread;
    public float reloadTime;
    public float timeBetweenShots;
    public int magSize;
    public int bulletsPerTap;
    public float switchDuration = 1f;
    public bool allowButtonHold;


    [Space(10)]
    [Header("Aim Settings")]
    public bool aimHold;
    public bool altADShold;

    [Space(10)]
    [Header("Recoil Settings")]
    public Vector3 recoilPosition;
    public Vector3 recoilRotation;

    [Space(3)]
    public Vector3 aimRecoilPosition;
    public Vector3 aimRecoilRotation;

    [Space(3)]
    public float snapiness;
    public float returnSpeed;

    [Space(10)]
    [Header("Animation Speed Curves")]

    public CurveAnimation AimSpeed;

    public CurveAnimation IdleSpeed;

    public CurveAnimation CrouchSpeed;

    [Header("Linear Animation")]
    public CurveAnimationVector3 aimAnim;

    [Space(10)]
    [Header("Animator Clip Playback Speeds")]
    public float normalAnimSpeed; 
    public float shootAnimSpeed; 
    public float reloadAnimSpeed; 
    public float sprintAnimSpeed; 
    
    [Space(10)]
    [Header("Animator Blend tree settings")]
    public float idle_Blend_speed;
    public float shoot_Blend_speed;
    public float reload_Blend_speed;
    public float sprint_Blend_speed;
    
    [Space(10)]
    [Header("Weapon Sway Settings")]

    [Header("Position Settings")]

    public Vector3 resetPosition;

    [Space(5)]
    [Tooltip("X is for Sway amount. Y is for maximum Sway amount.")]
    public Vector2 hipSwayAmount = new Vector2(0.055f, 0.09f);

    [Tooltip("X is for smooth amount. Y is for reset smooth amount.")]
    public Vector2 hipSmooth = new Vector2(5f, 2f);


    [Space(5)]
    [Tooltip("X is for Sway amount. Y is for maximum Sway amount.")]
    public Vector2 aimSwayAmount = new Vector2(0.055f, 0.09f);

    [Tooltip("X is for smooth amount. Y is for reset smooth amount.")]
    public Vector2 aimSmooth = new Vector2(5f, 2f);
    

    [Space(5)]
    [Tooltip("X is for Sway amount. Y is for maximum Sway amount.")]
    public Vector2 crouchSwayAmount = new Vector2(0.055f, 0.09f);

    [Tooltip("X is for smooth amount. Y is for reset smooth amount.")]
    public Vector2 crouchSmooth = new Vector2(5f, 2f);


    [Space(5)]
    [Tooltip("X is for Sway amount. Y is for maximum Sway amount.")]
    public Vector2 sprintSwayAmount = new Vector2(0.055f, 0.09f);

    [Tooltip("X is for smooth amount. Y is for reset smooth amount.")]
    public Vector2 sprintSmooth = new Vector2(5f, 2f);


    [Space(10)]
    [Header("Rotation Settings")]

    public Quaternion resetRotation;

    [Space(5)]
    [Tooltip("X is for Sway amount. Y is for maximum Sway amount.")]
    public Vector2 hipRotAmount = new Vector2(0.055f, 0.09f);

    [Tooltip("X is for smooth amount. Y is for reset smooth amount.")]
    public Vector2 hipRotSmooth = new Vector2(5f, 2f);


    [Space(5)]
    [Tooltip("X is for Sway amount. Y is for maximum Sway amount.")]
    public Vector2 aimRotAmount = new Vector2(0.055f, 0.09f);

    [Tooltip("X is for smooth amount. Y is for reset smooth amount.")]
    public Vector2 aimRotSmooth = new Vector2(5f, 2f);
    

    [Space(5)]
    [Tooltip("X is for Sway amount. Y is for maximum Sway amount.")]
    public Vector2 crouchRotAmount = new Vector2(0.055f, 0.09f);

    [Tooltip("X is for smooth amount. Y is for reset smooth amount.")]
    public Vector2 crouchRotSmooth = new Vector2(5f, 2f);


    [Space(5)]
    [Tooltip("X is for Sway amount. Y is for maximum Sway amount.")]
    public Vector2 sprintRotAmount = new Vector2(0.055f, 0.09f);

    [Tooltip("X is for smooth amount. Y is for reset smooth amount.")]
    public Vector2 sprintRotSmooth = new Vector2(5f, 2f);
    
    [Space(5)]
    [Header("Movement Sway")]
    [Tooltip("X is for forwardfactor. Y is for bacward factor")]
    public Vector2 forwardMoveFactor = new Vector2(0.8f, 0.4f);

    // [Tooltip("X is for Sway amount. Y is for maximum Sway amount.")]
    public Vector2 hipZAmount;

    // [Tooltip("X is for Sway amount. Y is for maximum Sway amount.")]
    public Vector2 aimZAmount;

    // [Tooltip("X is for Sway amount. Y is for maximum Sway amount.")]
    public Vector2 crouchZAmount;
}
