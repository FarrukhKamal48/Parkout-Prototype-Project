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
        
        if (weaponsInSlot == maxSlot) slotFull = true;
        else slotFull = false;

        
        HandlePickAndDrop();

        prevSelectIndex = selectedIndex;

        prevgunSettings = weapons[prevSelectIndex];

        SelectIndex();

        if (prevSelectIndex != selectedIndex && !switching)
        {
            StartCoroutine(SelectWeapon());
        }
        
        
    }
    
    void HandlePickAndDrop() {
        
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
    }
    
    void SetSwitchingBools() {
        
        weaponRefs.gunAnimator.SetBool("Switching In", switchingIn);
        weaponRefs.gunAnimator.SetBool("Switching Out", switchingOut);
    }
    
    void SelectIndex() {
        
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
    }
    
    void UpdateScripts() {

        currentgunScript.twoHanded = currentgunSettings;
        currentgunScript.weaponRefs = weaponRefs;
        currentgunScript.gunAnimator = weaponRefs.gunAnimator;
        currentgunScript.audioManager = weaponRefs.audioManager;
        
        cameraRecoil.gunSettings = currentgunSettings;
        
        swayScript.weaponRefs = weaponRefs;
        swayScript.Settings = currentgunSettings;

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

        print("One");

        // holster in
        switchingIn = false;
        switchingOut = true;
        switching = true;

        SetSwitchingBools();

        
        yield return new WaitForSeconds(currentgunSettings.switchDuration);


        if (currentGunObject != null) {
            Destroy(currentGunObject.gameObject);
        }

        // holster out
        switchingIn = true;
        switchingOut = false;

        // set current gun settings
        currentgunSettings = weapons[selectedIndex];
        
        // create the new gun object
        GameObject loadedGunObject = Instantiate(currentgunSettings.weaponObject);
        
        // set gun objects position, rotation, name and parent
        loadedGunObject.transform.SetParent(weaponHolder);
        loadedGunObject.transform.localEulerAngles = Vector3.zero;
        loadedGunObject.transform.localPosition = currentgunSettings.weaponRefs.startPos;
        loadedGunObject.name = currentgunSettings.name;

        // set references
        currentGunObject = loadedGunObject.transform;
        weaponRefs = currentGunObject.GetComponent<WeaponReferences>();

        SetSwitchingBools();
        

        yield return new WaitForSeconds(currentgunSettings.switchDuration);

        // stop switching
        switchingIn = false;
        switchingOut = false;
        switching = false;

        SetSwitchingBools();

        UpdateScripts();
        
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