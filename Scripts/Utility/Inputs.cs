using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    public static bool grounded, readytojump, walking, Sprinting, crouching, sliding, grappling;
    static bool sprinting;
    public static Transform cameraLook;

    public static Vector3 MoveInput()
    {
        float X = Input.GetAxis("Horizontal");
        float Y = Input.GetAxis("Vertical");

        Vector3 moveInput = new Vector3(X, 0, Y);

        return moveInput;
    }

    void PlayerState()
    {
        walking = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && Sprinting == false && crouching == false;

        if (walking == true)
        {
            sprinting = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            sprinting = !sprinting;
        }
        Sprinting = sprinting && Input.GetKey(KeyCode.W) && crouching == false && grounded == true;

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouching = !crouching;
        }
    }

    void Update()
    {
        PlayerState();
    }
}
