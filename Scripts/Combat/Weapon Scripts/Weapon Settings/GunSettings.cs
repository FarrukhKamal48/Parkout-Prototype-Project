using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunSettings", menuName = "WeaponSettings/GunSettings", order = 0)]
public class GunSettings : ScriptableObject
{
    public new string name;
    public GameObject bullet;
    public GameObject muzzleFlash;

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
}
