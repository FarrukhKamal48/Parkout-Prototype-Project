using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sway : MonoBehaviour
{
    public GunSettings Settings;
    public WeaponReferences weaponRefs;

    Vector2 _swayAmount;
    Vector2 _smooth;
    
    Vector2 _swayRotAmount;
    Vector2 _smoothRot;
    Vector2 _ZrotAmount;

    Vector3 def;
    Quaternion defRot;

    // Start is called before the first frame update
    void Start()
    {
        // if (weaponRefs == null || Settings == null)
        //     return;

        def = weaponRefs.swayPivot.localPosition;
        defRot = weaponRefs.swayPivot.localRotation;
        SwaySettings();
    }

    void SwaySettings()
    {
        if (PlayerController.aiming)
        {
            _swayAmount = Settings.aimSwayAmount;
            _smooth = Settings.aimSmooth;

            _swayRotAmount = Settings.aimRotAmount;
            _smoothRot = Settings.aimRotSmooth;
            _ZrotAmount = Settings.aimZAmount;
        }
        else if (PlayerController.Sprinting)
        {
            _swayAmount = Settings.sprintSwayAmount;
            _smooth = Settings.sprintSmooth;

            _swayRotAmount = Settings.sprintRotAmount;
            _smoothRot = Settings.sprintRotSmooth;
            _ZrotAmount = Vector2.zero;
        }
        else if (PlayerController.crouching)
        {
            _swayAmount = Settings.crouchSwayAmount;
            _smooth = Settings.crouchSmooth;

            _swayRotAmount = Settings.crouchRotAmount;
            _smoothRot = Settings.crouchRotSmooth;
            _ZrotAmount = Settings.crouchZAmount;
        }
        else
        {
            _swayAmount = Settings.hipSwayAmount;
            _smooth = Settings.hipSmooth;

            _swayRotAmount = Settings.hipRotAmount;
            _smoothRot = Settings.hipRotSmooth;
            _ZrotAmount = Settings.hipZAmount;
        }
    }

    void UpdateSwayPosition()
    {
        weaponRefs.swayPivot.localPosition = Vector3.Lerp(weaponRefs.swayPivot.localPosition, Settings.resetPosition, _smooth.y * Time.deltaTime);

        float factroX = -Input.GetAxis("Mouse X") * _swayAmount.x;
        float factroY = -Input.GetAxis("Mouse Y") * _swayAmount.x;

        if (factroX > _swayAmount.y)
            factroX = _swayAmount.y;

        if (factroX < -_swayAmount.y)
            factroX = -_swayAmount.y;

        if (factroY > _swayAmount.y)
            factroY = _swayAmount.y;

        if (factroY < -_swayAmount.y)
            factroY = -_swayAmount.y;

        Vector3 final = new Vector3(def.x + factroX, def.y + factroY, def.z);
        weaponRefs.swayPivot.localPosition = Vector3.Lerp(weaponRefs.swayPivot.localPosition, final, _smooth.x * Time.deltaTime);
    }

    void UpdateSwayRotation()
    {
        weaponRefs.swayPivot.localRotation = Quaternion.Slerp(weaponRefs.swayPivot.localRotation, Settings.resetRotation, _smoothRot.y * Time.deltaTime);

        //controls
        float t_x_mouse = Input.GetAxis("Mouse X") * _swayRotAmount.x;
        float t_y_mouse = Input.GetAxis("Mouse Y") * -_swayRotAmount.x;
        
        float t_x_move = Input.GetAxisRaw("Horizontal") * -_ZrotAmount.x;
        float t_y_move = Input.GetAxisRaw("Vertical") * _ZrotAmount.x;

        // if (t_x_mouse > _swayRotAmount.y)
        //     t_x_mouse = _swayRotAmount.y;

        // if (t_x_mouse < -_swayRotAmount.y)
        //     t_x_mouse = -_swayRotAmount.y;

        // if (t_y_mouse > _swayRotAmount.y)
        //     t_y_mouse = _swayRotAmount.y;

        // if (t_y_mouse < -_swayRotAmount.y)
        //     t_y_mouse = -_swayRotAmount.y;
        
        // clamp mouse sway
        t_x_mouse = Mathf.Clamp(t_x_mouse, -_swayRotAmount.y, _swayRotAmount.y);
        t_y_mouse = Mathf.Clamp(t_y_mouse, -_swayRotAmount.y, _swayRotAmount.y);
        
        // clamp movement sway
        t_x_move = Mathf.Clamp(t_x_move, -_ZrotAmount.y, _ZrotAmount.y);
        t_y_move = Mathf.Clamp(t_y_move, -_ZrotAmount.y, _ZrotAmount.y);


        //calculate target rotation
        
        float forwardFactor = t_y_move >= 0f ? Settings.forwardMoveFactor.x * t_y_move : Settings.forwardMoveFactor.y * t_y_move;
        
        Quaternion t_x_adj = Quaternion.AngleAxis(t_x_mouse, Vector3.up);
        Quaternion t_y_adj = Quaternion.AngleAxis(t_y_mouse + forwardFactor, Vector3.right);
        Quaternion t_z_adj = Quaternion.AngleAxis(t_x_move, Vector3.forward);
        Quaternion target_rotation = defRot * t_x_adj * t_y_adj * t_z_adj;

        //rotate towards target rotation
        weaponRefs.swayPivot.localRotation = Quaternion.Slerp(weaponRefs.swayPivot.localRotation, target_rotation, Time.deltaTime * _smoothRot.x);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // if (weaponRefs == null || Settings == null)
        //     return;

        SwaySettings();
        UpdateSwayPosition();
        UpdateSwayRotation();
    }
}
