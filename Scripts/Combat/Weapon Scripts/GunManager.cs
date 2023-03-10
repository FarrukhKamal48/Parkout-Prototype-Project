using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ArmIKHandler iKHandler;
    
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private List<GunSettings> weapons = new List<GunSettings>();

    [Header("WeaponEquip Settings")]
    [SerializeField] private KeyCode equip;
    [SerializeField] private KeyCode drop;
    [SerializeField] private float pickUpRange;
    [SerializeField] private int maxSlot;

    [Header("Info")]
    public int weaponsInSlot;
    public bool slotFull;
    public bool switchingOut = false;
    public bool switchingIn = false;
    public bool switching = false;
    public int selectedIndex = 0;

    public static GunSettings currentgunSettings;
    public static ProjectileGun currentgunScript;
    public static GunSettings prevgunSettings;
    
    [Header("Scripts")]
    public ProjectileGun gunScript;
    public sway swayScript;
    public WeaponReferences weaponRefs;
    public RecoilScript cameraRecoil;

    private Transform currentGunObject;
    
    
    int prevSelectIndex;
    
    float selectionDuration;

    void Awake()
    {
        currentgunScript = gunScript;

        prevgunSettings = weapons[selectedIndex];
        print("fuck");
        
        Equip(weapons);

        StartingWeapon();

        weaponRefs = currentGunObject.GetComponent<WeaponReferences>();
    }

    void Update()
    {
        if (weapons.Count != 0)
        {
            //currentGun_t = weapons[selectedIndex];
            //currentgun = currentGun_t.GetComponent<ProjectileGun>();
            //currentgun.gunManager = this;
        }
        
        if (weaponsInSlot == maxSlot) slotFull = true;
        else slotFull = false;

        RaycastHit hit;

        if (Physics.Raycast(weaponHolder.position, weaponHolder.forward, out hit, pickUpRange))
        {
            if (slotFull == false && Input.GetKeyDown(equip) && hit.transform.GetComponent<Item>() != null)
            {
                Item hitItem = hit.transform.GetComponent<Item>();
                GunSettings choosenItem = hitItem.chooseItem();

                if (choosenItem != null) {
                    Equip(choosenItem);
                    hitItem.remainingInstantiations--;
                }
            }
        }

        if (weaponsInSlot > 0 && Input.GetKeyDown(drop) && selectedIndex != 0)
        {
            Drop(weapons[selectedIndex]);
        }


        // input

        prevSelectIndex = selectedIndex;


        prevgunSettings = weapons[prevSelectIndex];
        selectionDuration = prevgunSettings.switchDuration + 
            prevgunSettings.switchDuration; 


        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedIndex == weaponsInSlot - 1)
                selectedIndex = 0;
            else
                selectedIndex++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (selectedIndex == 0)
                selectedIndex = weaponsInSlot - 1;
            else
                selectedIndex--;
        }
        
        selectedIndex = Mathf.Clamp(selectedIndex, 0, weaponsInSlot - 1);
        
        // UpdateScripts();

        if (prevSelectIndex != selectedIndex)
        {
            StartCoroutine(SelectWeapon());
            // iKHandler.updateTargets();
        }
    }
    
    void UpdateScripts() {

        currentgunScript.twoHanded = currentgunSettings;
        currentgunScript.weaponRefs = weaponRefs;
        currentgunScript.gunAnimator = weaponRefs.transform.GetComponent<Animator>();
        currentgunScript.audioManager = weaponRefs.transform.GetComponent<AudioManager>();
        
        cameraRecoil.gunSettings = currentgunSettings;
        
        swayScript.weaponRefs = weaponRefs;
        swayScript.Settings = currentgunSettings;

    }
    
    void HandleSwitchAnim(Transform prevGunObject) {
        
        // spawn the new gun
        // disable all of its meshes
        //
        // set switching out of the previous gun to true
        // wait for the switch duration of previous gun
        // set switching out of the previous gun to false
        //
        // destroy the previous gun
        //
        // enable the current gun's meshes
        //
        // set switching in of the current gun to true
        // wait for the switch duration of current gun
        // set switching in of the current gun to false
        
        weaponRefs.SetActiveMeshes(false);
        
        switchingOut = true;
        switchingIn = false;
        
        // destroy the previous gun object
        if (prevGunObject != null) {
            Destroy(prevGunObject.gameObject, prevgunSettings.switchDuration);
        }
        else {
            switchingOut = false;
            switchingIn = true;
        }
    }
    
    void StartingWeapon() {
        
        Transform prevGunObject;
        prevGunObject = currentGunObject;

        // set current gun settings
        currentgunSettings = weapons[selectedIndex];
        
        // create the new gun object
        GameObject loadedGunObject = Instantiate(currentgunSettings.weaponObject);

        loadedGunObject.transform.SetParent(weaponHolder);
        loadedGunObject.transform.localEulerAngles = Vector3.zero;
        loadedGunObject.transform.localPosition = currentgunSettings.weaponRefs.startPos;
        loadedGunObject.name = currentgunSettings.name;
        
        currentGunObject = loadedGunObject.transform;
        weaponRefs = currentGunObject.GetComponent<WeaponReferences>();
        
        // destroy the previous gun object
        if (prevGunObject != null) {
            Destroy(prevGunObject.gameObject);
        }
        
        // HandleSwitchAnim(prevGunObject);
        
        UpdateScripts();
    }

    IEnumerator SelectWeapon() {

        switchingIn = false;
        switchingOut = true;
        print("switching out");

        switching = true;
        
        yield return new WaitForSeconds(currentgunSettings.switchDuration);

        if (currentGunObject != null) {
            Destroy(currentGunObject.gameObject);
        }

        switchingIn = true;
        switchingOut = false;
        print("switching in");

        // Transform prevGunObject;
        // prevGunObject = currentGunObject;

        // set current gun settings
        currentgunSettings = weapons[selectedIndex];
        
        // create the new gun object
        GameObject loadedGunObject = Instantiate(currentgunSettings.weaponObject);

        loadedGunObject.transform.SetParent(weaponHolder);
        loadedGunObject.transform.localEulerAngles = Vector3.zero;
        loadedGunObject.transform.localPosition = currentgunSettings.weaponRefs.startPos;
        loadedGunObject.name = currentgunSettings.name;
        
        currentGunObject = loadedGunObject.transform;
        weaponRefs = currentGunObject.GetComponent<WeaponReferences>();
        
        yield return new WaitForSeconds(currentgunSettings.switchDuration);
        
        switchingIn = false;
        switchingOut = false;
        
        switching = false;
        print("switched");

        // destroy the previous gun object
        // if (prevGunObject != null) {
        //     Destroy(prevGunObject.gameObject);
        // }
        
        // HandleSwitchAnim(prevGunObject);
        
        UpdateScripts();


        // to set whether the player is switching a weapon
        // StartCoroutine(SelectionDelay());
        
    }
    
    IEnumerator SelectionDelay() {

        switchingOut = true;
        switchingIn = false;
        
        yield return new WaitForSeconds(prevgunSettings.switchDuration);
        
        switchingOut = false;
        switchingIn = true;
        
        yield return new WaitForSeconds(currentgunSettings.switchDuration);

        switchingOut = false;
        switchingIn = false;

    }

    void Equip(GunSettings weapon)
    {
        weaponsInSlot++;

        // GameObject loadedGunObject = Instantiate(weapon.weaponObject);

        // loadedGunObject.transform.SetParent(weaponHolder);
        // loadedGunObject.transform.localEulerAngles = Vector3.zero;
        // loadedGunObject.transform.localPosition = weapon.weaponRefs.startPos;

        // weapon.fpsCam = weaponHolder;

        // weapons.Add(loadedGun.transform);
        weapons.Add(weapon);
    }

    void Equip(List<GunSettings> _weapons)
    {
        weaponsInSlot += _weapons.Count;

        // GameObject loadedGunObject = Instantiate(_weapons[selectedIndex].weaponObject);
        // WeaponReferences weaponRefs = _weapons[selectedIndex].weaponRefs;

        // loadedGunObject.transform.SetParent(weaponHolder);
        // loadedGunObject.transform.localEulerAngles = Vector3.zero;
        // loadedGunObject.transform.localPosition = weaponRefs.startPos;

        // currentgun.fpsCam = weaponHolder;

        // SelectWeapon();
    }

    void Drop(GunSettings weapon)
    {
        if (weaponsInSlot == 1)
            return;

        weaponsInSlot--;

        selectedIndex = weaponsInSlot - 1;

        Destroy(currentGunObject.gameObject);
        weapons.Remove(weapon);

        StartCoroutine(SelectWeapon());
    }
}

[System.Serializable]
public struct WeaponItem {
    public GunSettings Settings;
    public Transform instanceTransform;
    
    public WeaponItem (GunSettings Settings, Transform instanceTransform) {
        this.Settings = Settings;
        this.instanceTransform = instanceTransform;
    }
}