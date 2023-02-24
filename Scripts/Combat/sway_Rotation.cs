using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sway_Rotation : MonoBehaviour
{

    public Animator animator;

    public MonoBehaviour aimSway;
    public MonoBehaviour hipSway;
    

    public Transform ThisTransform;

    public float resetPosiotionSmooth = 2f;

    public Vector3 resetPositon;

    Vector3 currentPosition;

    //public float resetPositionSmooth = 2f;
    
    
    public float intensity;
    
    public float MaxAmount = 0.1f;

    public float smooth;
    public bool isMine;

    private Quaternion origin_rotation;


    private void Start()
    {
        origin_rotation = transform.localRotation;
    }

    private void Update()
    {
        ThisTransform.localPosition = Vector3.Lerp(ThisTransform.localPosition, resetPositon, resetPosiotionSmooth * Time.deltaTime);

        if (animator.GetBool("Scoped") == true)
        {
            aimSway.enabled = true;
            hipSway.enabled = false;
        }

        if (animator.GetBool("Scoped") == false)
        {
            aimSway.enabled = false;
            hipSway.enabled = true;
        }

        UpdateSway();
    }


    private void UpdateSway()
    {
        //controls
        float t_x_mouse = Input.GetAxis("Mouse X");
        float t_y_mouse = Input.GetAxis("Mouse Y");

        if (t_x_mouse > MaxAmount)
            t_x_mouse = MaxAmount;

        if (t_x_mouse < -MaxAmount)
            t_x_mouse = -MaxAmount;

        if (t_y_mouse > MaxAmount)
            t_y_mouse = MaxAmount;

        if (t_y_mouse < -MaxAmount)
            t_y_mouse = -MaxAmount;

        if (!isMine)
        {
            t_x_mouse = 0;
            t_y_mouse = 0;
        }

        //calculate target rotation
        Quaternion t_x_adj = Quaternion.AngleAxis(intensity * t_x_mouse, Vector3.up);
        Quaternion t_y_adj = Quaternion.AngleAxis(-intensity * t_y_mouse, Vector3.right);
        Quaternion target_rotation = origin_rotation * t_x_adj * t_y_adj;

        //rotate towards target rotation
        transform.localRotation = Quaternion.Lerp(transform.localRotation, target_rotation, Time.deltaTime * smooth);
    }

}
