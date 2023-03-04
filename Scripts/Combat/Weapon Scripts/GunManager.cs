using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ArmIKHandler iKHandler;
    
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private List<Transform> weapons = new List<Transform>();

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
    public int selectedIndex = 0;

    Transform currentGun_t;
    public static Weapon currentgun;
    public static Weapon prevgun;
    
    int prevSelectIndex;
    
    float selectionDuration;

    void Start()
    {

        prevgun = weapons[selectedIndex].GetComponent<Weapon>();
        prevgun.gunManager = this;
        
        Equip(weapons);

        SelectWeapon();

    }

    void Update()
    {
        if (weapons.Count != 0)
        {
            //currentGun_t = weapons[selectedIndex];
            //currentgun = currentGun_t.GetComponent<ProjectileGun>();
            //currentgun.gunManager = this;
        }

        if (weaponsInSlot >= maxSlot) slotFull = true;
        else slotFull = false;

        RaycastHit hit;

        if (Physics.Raycast(weaponHolder.position, weaponHolder.forward, out hit, pickUpRange))
        {
            if (slotFull == false && Input.GetKeyDown(equip) && hit.transform.GetComponent<Item>() != null)
            {
                Item hitItem = hit.transform.GetComponent<Item>();
                GameObject choosenItem = hitItem.chooseItem();

                if (choosenItem != null) {
                    Equip(choosenItem);
                    hitItem.remainingInstantiations--;
                }
            }
        }

        if (weaponsInSlot > 0 && Input.GetKeyDown(drop) && selectedIndex != 0)
        {
            Drop(weapons[selectedIndex].gameObject);
        }


        // input

        prevSelectIndex = selectedIndex;

        prevgun = weapons[prevSelectIndex].GetComponent<Weapon>();
        selectionDuration = prevgun.thisGun.Settings.switchDuration + 
            prevgun.thisGun.Settings.switchDuration; 


        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedIndex >= weaponsInSlot - 1)
                selectedIndex = 0;
            else
                selectedIndex++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (selectedIndex <= 0)
                selectedIndex = weaponsInSlot - 1;
            else
                selectedIndex--;
        }

        if (prevSelectIndex != selectedIndex)
        {
            SelectWeapon();
            iKHandler.updateTargets();
        }
    }

    void SelectWeapon()
    {
        
        int i = 0;
        foreach (Transform _weapon in weapons)
        {
            if (i == selectedIndex)
            {
                _weapon.gameObject.SetActive(true);
            }
            else
            {
                _weapon.gameObject.SetActive(false);
            }
            i++;

            _weapon.GetComponent<Weapon>().gunManager = this;
        }

        // set current gun script
        currentGun_t = weapons[selectedIndex];
        currentgun = currentGun_t.GetComponent<Weapon>();
        currentgun.gunManager = this;

        // to set whether the player is switching a weapon
        StartCoroutine(SelectionDelay());
        
    }
    
    IEnumerator SelectionDelay() {

        switchingOut = true;
        switchingIn = false;
        
        yield return new WaitForSeconds(prevgun.thisGun.Settings.switchDuration);
        
        switchingOut = false;
        switchingIn = true;
        
        yield return new WaitForSeconds(currentgun.thisGun.Settings.switchDuration);

        switchingOut = false;
        switchingIn = false;

    }
    
    IEnumerator DelaySwitch(int prevIndex, int currentIndex) {

        yield return new WaitForSeconds(prevgun.thisGun.Settings.switchDuration);
        weapons[prevIndex].gameObject.SetActive(false);
        weapons[currentIndex].gameObject.SetActive(true);

    }

    void Equip(GameObject weapon)
    {
        weaponsInSlot++;

        GameObject loadedGun = Instantiate(weapon);
        Weapon gunScript = loadedGun.GetComponent<Weapon>();

        loadedGun.transform.SetParent(weaponHolder);
        loadedGun.transform.localEulerAngles = Vector3.zero;
        loadedGun.transform.localPosition = gunScript.startPos;

        gunScript.fpsCam = weaponHolder;

        weapons.Add(loadedGun.transform);

        SelectWeapon();
    }

    void Equip(List<Transform> _weapons)
    {
        weaponsInSlot += _weapons.Count;

        GameObject loadedGun = Instantiate(_weapons[selectedIndex].gameObject);
        Weapon gunScript = loadedGun.GetComponent<Weapon>();

        loadedGun.transform.SetParent(weaponHolder);
        loadedGun.transform.localEulerAngles = Vector3.zero;
        loadedGun.transform.localPosition = gunScript.startPos;

        weapons[selectedIndex] = loadedGun.transform;

        gunScript.fpsCam = weaponHolder;

        SelectWeapon();
    }

    void Drop(GameObject weapon)
    {
        weaponsInSlot--;

        Destroy(weapon);
        weapons.Remove(weapon.transform);

        selectedIndex = weaponsInSlot - 1;
        SelectWeapon();
    }
}
