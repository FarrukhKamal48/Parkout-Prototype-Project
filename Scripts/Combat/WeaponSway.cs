using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private Transform pivot;

    [Header("Sway Amount")]
    private float SwayAmount;
    [SerializeField] private float aimSwayAmount;
    [SerializeField] private float hipSwayAmount;
    [SerializeField] private float sprintSwayAmount;


    [Header("Sway Smoothing")]
    private float SwaySmoothing;
    [SerializeField] private float aimSwaySmoothing;
    [SerializeField] private float hipSwaySmoothing;
    [SerializeField] private float sprintSwaySmoothing;


    [Header("Sway Reset Smoothing")]
    private float SwayResetSmoothing;
    [SerializeField] private float aimSwayResetSmoothing;
    [SerializeField] private float hipSwayResetSmoothing;
    [SerializeField] private float sprintSwayResetSmoothing;


    [SerializeField] private float SwayClampX, SwayClampY;

    [SerializeField] private bool xInvert, yInvert;

    Vector2 inputView;

    Vector3 newWeaponRotaion;
    Vector3 newWeaponRotaionVelecity;

    Vector3 targetWeaponRotaion;
    Vector3 targetWeaponRotaionVelecity;

    void Awake()
    {
        newWeaponRotaion = pivot.localRotation.eulerAngles;

        // defalut sway settings
        SwayAmount = hipSwayAmount;
        SwaySmoothing = hipSwaySmoothing;
        SwayResetSmoothing = hipSwayResetSmoothing;
    }

    void Update()
    {
        SwaySettings();
        ApplySway();
    }

    void SwaySettings()
    {
        if (PlayerController.aiming)
        {
            SwayAmount = aimSwayAmount;
            SwaySmoothing = aimSwaySmoothing;
            SwayResetSmoothing = aimSwayResetSmoothing;
        }
        else if (PlayerController.Sprinting)
        {
            SwayAmount = sprintSwayAmount;
            SwaySmoothing = sprintSwaySmoothing;
            SwayResetSmoothing = sprintSwayResetSmoothing;
        }
        else
        {
            SwayAmount = hipSwayAmount;
            SwaySmoothing = hipSwaySmoothing;
            SwayResetSmoothing = hipSwayResetSmoothing;
        }
    }

    void ApplySway()
    {
        inputView = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        targetWeaponRotaion.y += SwayAmount * (xInvert ? -inputView.x : inputView.x) * Time.deltaTime;
        targetWeaponRotaion.x += SwayAmount * (yInvert ? inputView.y : -inputView.y) * Time.deltaTime;

        targetWeaponRotaion.x = Mathf.Clamp(targetWeaponRotaion.x, -SwayClampX, SwayClampX);
        targetWeaponRotaion.y = Mathf.Clamp(targetWeaponRotaion.y, -SwayClampY, SwayClampY);

        targetWeaponRotaion = Vector3.SmoothDamp(targetWeaponRotaion, Vector3.zero, ref targetWeaponRotaionVelecity, 1.0f / SwayResetSmoothing);
        newWeaponRotaion = Vector3.SmoothDamp(newWeaponRotaion, targetWeaponRotaion, ref newWeaponRotaionVelecity, 1.0f / SwaySmoothing);

        pivot.localRotation = Quaternion.Euler(newWeaponRotaion);
    }
}
