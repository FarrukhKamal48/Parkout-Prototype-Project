using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [Space(5)]
    [Header("IK setup")]
    public List<Transform> R_targetTransforms = new List<Transform>();
    public List<Transform> L_targetTransforms = new List<Transform>();


    [HideInInspector] public static bool isOneHanded;
    
    [Space(5)]
    [Header("Weapon References")]
    public ProjectileGun thisGun;
    public Melee thisMelee;
    
    public Transform fpsCam;
    public GunManager gunManager;

    public Vector3 startPos;

    public virtual void SetupSettings()
    {

    }

    public virtual void MyInput()
    {

    }

    public virtual void Animation()
    {

    }

    public virtual void Shoot()
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }
}
