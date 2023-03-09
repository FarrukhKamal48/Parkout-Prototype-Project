using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReferences : MonoBehaviour
{
    public Vector3 startPos;
    [Header("Shooting")]
    public Transform attackPoint;
    public Transform muzzlePoint;
    
    [Space(10)]
    public Transform swayPivot;

    [Space(10)]
    [Header("Component References")]
    public Animator gunAnimator;
    public AudioManager audioManager;
    
    [Header("Proc Animation Transforms")]
    public Transform anchor_T;

    [Space(5)]
    public Transform hip_T;

    [Space(5)]
    public Transform adsRotator;
    public Transform ADS_T;
    public Transform altADS_T;

    [Space(5)]
    public Transform crouch_T;

    [Space(5)]
    public Transform recoil_T;
    
    [Header("Weapon Meshes")]
    public List<MeshRenderer> weaponMeshes = new List<MeshRenderer>();

    public void SetActiveMeshes(bool _active) {
        foreach (MeshRenderer W_mesh in weaponMeshes)
        {
            W_mesh.enabled = _active;
        }
    }
}
