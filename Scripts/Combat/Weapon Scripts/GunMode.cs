using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProjectileGun))]
public class GunMode : MonoBehaviour
{
    public ProjectileGun gunScript;
    public GunSettings oneHanded;
    public GunSettings twoHanded;
    public KeyCode switchKey;
    private bool _isOneHanded;
    public static bool isOneHanded;

    void Start()
    {
        // gunScript.Settings = _isOneHanded ? oneHanded : twoHanded;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            _isOneHanded = !_isOneHanded;
        }
        // gunScript.Settings = _isOneHanded ? oneHanded : twoHanded;

        GunMode.isOneHanded = _isOneHanded;
    }
}
