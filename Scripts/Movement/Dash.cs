using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private KeyCode dashKey;
    [SerializeField] private Vector2 dashForce;
    [SerializeField] private float maxDashes;
    [SerializeField] private float refeulDelay;
    [SerializeField] private bool useCamDir;


    public static bool dashing;

    float remainingDashes;
    bool canDash = true;

    bool allowRefeul = true;


    void Awake()
    {
        remainingDashes = maxDashes;
    }

    void dash(float dashSpeed, Vector3 direction)
    {
        rb.AddForce(direction * dashSpeed, ForceMode.VelocityChange);
        remainingDashes--;
    }


    Vector3 DashDirection()
    {
        if (PlayerController.Y > 0 && PlayerController.X == 0 && useCamDir == true)
        {
            return PlayerController.cameraLook.forward;
        }
        else
        {
            return PlayerController.moveDir;
        }
    }

    void Refeul()
    {
        remainingDashes++;
        allowRefeul = true;
    }

    void Update()
    {
        Vector3 dashDirection = DashDirection();

        if (Input.GetKey(dashKey) && remainingDashes > 0 && canDash == true)
        {
            dash(PlayerController.grounded ? dashForce.x: dashForce.y, dashDirection);
            canDash = false;
        }
        else if (!Input.GetKey(dashKey))
        {
            canDash = true;
        }

        if (remainingDashes < maxDashes)
        {
            if (allowRefeul)
            {
                Invoke("Refeul", refeulDelay);
                allowRefeul = false;
            }
        }

    }
}
