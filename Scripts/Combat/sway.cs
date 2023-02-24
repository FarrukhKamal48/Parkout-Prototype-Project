using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sway : MonoBehaviour
{
    public Transform swayTransform;

    [Space(15)]
    [Header("Position Settings")]

    public Vector3 resetPosition;

    [Space(10)]
    [Tooltip("X is for Sway amount. Y is for maximum Sway amount.")]
    public Vector2 hipSwayAmount = new Vector2(0.055f, 0.09f);

    [Tooltip("X is for smooth amount. Y is for reset smooth amount.")]
    public Vector2 hipSmooth = new Vector2(5f, 2f);


    [Space(10)]
    [Tooltip("X is for Sway amount. Y is for maximum Sway amount.")]
    public Vector2 aimSwayAmount = new Vector2(0.055f, 0.09f);

    [Tooltip("X is for smooth amount. Y is for reset smooth amount.")]
    public Vector2 aimSmooth = new Vector2(5f, 2f);
    

    [Space(10)]
    [Tooltip("X is for Sway amount. Y is for maximum Sway amount.")]
    public Vector2 crouchSwayAmount = new Vector2(0.055f, 0.09f);

    [Tooltip("X is for smooth amount. Y is for reset smooth amount.")]
    public Vector2 crouchSmooth = new Vector2(5f, 2f);


    [Space(10)]
    [Tooltip("X is for Sway amount. Y is for maximum Sway amount.")]
    public Vector2 sprintSwayAmount = new Vector2(0.055f, 0.09f);

    [Tooltip("X is for smooth amount. Y is for reset smooth amount.")]
    public Vector2 sprintSmooth = new Vector2(5f, 2f);


    [Space(15)]
    [Header("Rotation Settings")]

    public Quaternion resetRotation;

    [Space(10)]
    [Tooltip("X is for Sway amount. Y is for maximum Sway amount.")]
    public Vector2 hipRotAmount = new Vector2(0.055f, 0.09f);

    [Tooltip("X is for smooth amount. Y is for reset smooth amount.")]
    public Vector2 hipRotSmooth = new Vector2(5f, 2f);


    [Space(10)]
    [Tooltip("X is for Sway amount. Y is for maximum Sway amount.")]
    public Vector2 aimRotAmount = new Vector2(0.055f, 0.09f);

    [Tooltip("X is for smooth amount. Y is for reset smooth amount.")]
    public Vector2 aimRotSmooth = new Vector2(5f, 2f);
    

    [Space(10)]
    [Tooltip("X is for Sway amount. Y is for maximum Sway amount.")]
    public Vector2 crouchRotAmount = new Vector2(0.055f, 0.09f);

    [Tooltip("X is for smooth amount. Y is for reset smooth amount.")]
    public Vector2 crouchRotSmooth = new Vector2(5f, 2f);


    [Space(10)]
    [Tooltip("X is for Sway amount. Y is for maximum Sway amount.")]
    public Vector2 sprintRotAmount = new Vector2(0.055f, 0.09f);

    [Tooltip("X is for smooth amount. Y is for reset smooth amount.")]
    public Vector2 sprintRotSmooth = new Vector2(5f, 2f);

    Vector2 _swayAmount;
    Vector2 _smooth;
    
    Vector2 _swayRotAmount;
    Vector2 _smoothRot;

    Vector3 def;
    Quaternion defRot;

    // Start is called before the first frame update
    void Start()
    {
        def = swayTransform.localPosition;
        defRot = swayTransform.localRotation;
        SwaySettings();
    }

    void SwaySettings()
    {
        if (PlayerController.aiming)
        {
            _swayAmount = aimSwayAmount;
            _smooth = aimSmooth;

            _swayRotAmount = aimRotAmount;
            _smoothRot = aimRotSmooth;
        }
        else if (PlayerController.Sprinting)
        {
            _swayAmount = sprintSwayAmount;
            _smooth = sprintSmooth;

            _swayRotAmount = sprintRotAmount;
            _smoothRot = sprintRotSmooth;
        }
        else if (PlayerController.crouching)
        {
            _swayAmount = crouchSwayAmount;
            _smooth = crouchSmooth;

            _swayRotAmount = crouchRotAmount;
            _smoothRot = crouchRotSmooth;
        }
        else
        {
            _swayAmount = hipSwayAmount;
            _smooth = hipSmooth;

            _swayRotAmount = hipRotAmount;
            _smoothRot = hipRotSmooth;
        }
    }

    void UpdateSwayPosition()
    {
        swayTransform.localPosition = Vector3.Lerp(swayTransform.localPosition, resetPosition, _smooth.y * Time.deltaTime);

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
        swayTransform.localPosition = Vector3.Lerp(swayTransform.localPosition, final, _smooth.x * Time.deltaTime);
    }

    void UpdateSwayRotation()
    {
        swayTransform.localRotation = Quaternion.Slerp(swayTransform.localRotation, resetRotation, _smoothRot.y * Time.deltaTime);

        //controls
        float t_x_mouse = Input.GetAxis("Mouse X") * _swayRotAmount.x;
        float t_y_mouse = Input.GetAxis("Mouse Y") * -_swayRotAmount.x;

        if (t_x_mouse > _swayRotAmount.y)
            t_x_mouse = _swayRotAmount.y;

        if (t_x_mouse < -_swayRotAmount.y)
            t_x_mouse = -_swayRotAmount.y;

        if (t_y_mouse > _swayRotAmount.y)
            t_y_mouse = _swayRotAmount.y;

        if (t_y_mouse < -_swayRotAmount.y)
            t_y_mouse = -_swayRotAmount.y;


        //calculate target rotation
        Quaternion t_x_adj = Quaternion.AngleAxis(t_x_mouse, Vector3.up);
        Quaternion t_y_adj = Quaternion.AngleAxis(t_y_mouse, Vector3.right);
        Quaternion target_rotation = defRot * t_x_adj * t_y_adj;

        //rotate towards target rotation
        swayTransform.localRotation = Quaternion.Slerp(swayTransform.localRotation, target_rotation, Time.deltaTime * _smoothRot.x);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        SwaySettings();
        UpdateSwayPosition();
        UpdateSwayRotation();
    }
}
