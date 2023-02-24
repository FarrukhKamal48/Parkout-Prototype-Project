using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunSettings", menuName = "WeaponSettings/GunSettings", order = 0)]
public class GunSettings : ScriptableObject
{
    public new string name;
    public GameObject bullet;
    public GameObject muzzleFlash;

    [Space(5)]
    [Header("Controls")]
    public KeyCode ReloadKey;
    public KeyCode altAdsKey;

    [Space(5)]
    [Header("Bullet Force")]
    public float shootForce;
    public float upwardForce;


    [Space(5)]
    [Header("Shoot Settings")]
    public float fireRate;
    public float spread;
    public float reloadTime;
    public float timeBetweenShots;
    public int magSize;
    public int bulletsPerTap;
    public bool allowButtonHold;


    [Space(5)]
    [Header("Aim Settings")]
    public bool aimHold;
    public bool altADShold;

    [Space(5)]
    [Header("Recoil Settings")]
    public Vector3 recoilPosition;
    public Vector3 recoilRotation;

    [Space(3)]
    public Vector3 aimRecoilPosition;
    public Vector3 aimRecoilRotation;

    [Space(3)]
    public float snapiness;
    public float returnSpeed;

    [Space(5)]
    [Header("Animation Settings")]
    public float aimSpeed;
    public float idleSpeed;

    public float normalAnimSpeed; 
    public float shootAnimSpeed; 
    public float reloadAnimSpeed; 
    public float sprintAnimSpeed; 
}
