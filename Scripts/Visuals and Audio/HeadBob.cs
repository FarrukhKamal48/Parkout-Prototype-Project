using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform jumpAnimTransform;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private PlayerController controller;

    [Header("HeadBob")]
    [SerializeField] private bool enableHeadbob = true;

    public float toggleSpeed = 3f;
    [SerializeField, Range(0, 10f)] private float amplitude = 0.015f;
    [SerializeField, Range(0, 30f)] private float frequency = 10.3f;
    [SerializeField] private Vector2 frequencyMultiplier;
    [SerializeField] private Vector2 sprintFrequencyMultiplier;
    [SerializeField] private Vector2 aimFrequencyMultiplier;
    [SerializeField] private Vector2 crouchFrequencyMultiplier;
    [SerializeField] private float Zrotation = 2f; 
    [SerializeField] private float ZrotSmooth = 2f; 
    [SerializeField] private float Ybump = 1f; 


    [Header("Jump Animation")]
    [SerializeField] public JumpAnimation landAnimation;
    [SerializeField] public JumpAnimation jumpAnimation;

    [SerializeField] private float returnSpeed = 1f;
    [SerializeField] private float animationSmoothing = 1f;

    Vector2 _frequencyMultiplier;


    private float _speed;

    private Quaternion startRot;
    private Vector3 startPos;

    void Awake()
    {
        startRot = jumpAnimTransform.localRotation;
        startPos = jumpAnimTransform.localPosition;
    }

    void Update()
    {
        ApplyHeadbob();
        ApplyJumpSmoothing();
    }

    float p = 0f;
    void ApplyJumpSmoothing()
    {
        // camera desync when landing
        if (controller._grounded == true && landAnimation.p < landAnimation.animationLength)
        {
            landAnimation.p += Time.deltaTime * landAnimation.animationSpeed;
            //jumpAnimTransform.localPosition.y = Mathf.Lerp(jumpAnimTransform.localPosition.y, landDesyncHeight.Evaluate(p));
            Vector3 targetPosition = new Vector3(startPos.x, startPos.y + (landAnimation.magnitude * landAnimation.AnimationMotionCurve.Evaluate(landAnimation.p)), startPos.z);
            jumpAnimTransform.localPosition = Vector3.Lerp(jumpAnimTransform.localPosition, targetPosition, animationSmoothing * Time.deltaTime);

            if (p >= landAnimation.animationLength) return;
        }
        else
        {
            jumpAnimTransform.localPosition = Vector3.Lerp(jumpAnimTransform.localPosition, startPos, returnSpeed * Time.deltaTime);
        }
        if (controller._grounded == false)
        {
            landAnimation.p = 0f;
        }

        // camera desync when jumping
        if (controller._grounded == false && jumpAnimation.p < jumpAnimation.animationLength)
        {
            jumpAnimation.p += Time.deltaTime * jumpAnimation.animationSpeed;
            //jumpAnimTransform.localPosition.y = Mathf.Lerp(jumpAnimTransform.localPosition.y, landDesyncHeight.Evaluate(p));
            Vector3 targetPosition = new Vector3(startPos.x, startPos.y + (jumpAnimation.magnitude * jumpAnimation.AnimationMotionCurve.Evaluate(jumpAnimation.p)), startPos.z);
            jumpAnimTransform.localPosition = Vector3.Lerp(jumpAnimTransform.localPosition, targetPosition, animationSmoothing * Time.deltaTime);

            if (p >= jumpAnimation.animationLength) return;
        }
        else
        {
            jumpAnimTransform.localPosition = Vector3.Lerp(jumpAnimTransform.localPosition, startPos, returnSpeed * Time.deltaTime);
        }
        if (controller._grounded == true)
        {
            jumpAnimation.p = 0.0f;
        }
    }

    void jumpSmooth()
    {
        landAnimation.p += Time.deltaTime * landAnimation.animationSpeed;
        //jumpAnimTransform.localPosition.y = Mathf.Lerp(jumpAnimTransform.localPosition.y, landDesyncHeight.Evaluate(p));
        Vector3 targetPosition = new Vector3(startPos.x, startPos.y + (landAnimation.magnitude * landAnimation.AnimationMotionCurve.Evaluate(landAnimation.p)), startPos.z);
        jumpAnimTransform.localPosition = Vector3.Lerp(jumpAnimTransform.localPosition, targetPosition, animationSmoothing * Time.deltaTime);

        if (p >= landAnimation.animationLength) return;
    }

    void ApplyHeadbob()
    {
        if (!enableHeadbob) return;

        CalcSpeed();
        SetFrequency();

        CheckMotion();
        ResetPosition();
    }

    void CalcSpeed()
    {
        Vector3 horizontalPlane = new Vector3(controller.rb.velocity.x, 0f, controller.rb.velocity.z);
        _speed = horizontalPlane.magnitude;
    }

    void SetFrequency()
    {
        if (PlayerController.walking)
        {
            _frequencyMultiplier = frequencyMultiplier;
        }
        else if (PlayerController.Sprinting)
        {
            _frequencyMultiplier = sprintFrequencyMultiplier;
        }
        else if (PlayerController.aiming)
        {
            _frequencyMultiplier = aimFrequencyMultiplier;
        }
        else if (PlayerController.crouching)
        {
            _frequencyMultiplier = crouchFrequencyMultiplier;
        }
    }

    public void PlayMotion(Vector3 motion)
    {
        cameraHolder.localEulerAngles += motion;
    }

    void CheckMotion()
    {
        if (_speed < toggleSpeed) return;
        if (PlayerController.grounded == false) return;

        PlayMotion(FootStepMotion());
    }

    Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.x += (Mathf.Sin(Time.time * frequency * _frequencyMultiplier.x) * (amplitude * _frequencyMultiplier.y)) - (Ybump * amplitude * _frequencyMultiplier.y * (Mathf.Sin(frequency * _frequencyMultiplier.x * ((Time.time - (Mathf.PI/2f)) + 1))));
        pos.y += Mathf.Cos(Time.time * (frequency * _frequencyMultiplier.x) / 2f) * (amplitude * _frequencyMultiplier.y) * 2f;
        pos.z = Mathf.Lerp(pos.z, -PlayerController.X * Zrotation, Time.deltaTime * ZrotSmooth);
        // pos.z += Mathf.Sin(Time.time * (frequency * _frequencyMultiplier.x) / 2f) * (amplitude * _frequencyMultiplier.y) / 1.5f;
        return pos;
    }

    private void ResetPosition()
    {
        if (cameraHolder.localRotation != startRot)
            cameraHolder.localRotation = Quaternion.Slerp(cameraHolder.localRotation, startRot, 5 * Time.deltaTime);
    }
}

[System.Serializable]
public struct JumpAnimation
{
    public float animationLength;
    public float magnitude;
    public AnimationCurve AnimationMotionCurve;
    public float animationSpeed;
    public float p;
}

// Stabalized Head Bob

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class HeadBob : MonoBehaviour
// {
//     [Header("References")]
//     [SerializeField] private Transform jumpAnimTransform;
//     [SerializeField] private Transform cameraHolder;
//     [SerializeField] private Transform stabalizeTransform;
//     [SerializeField] private PlayerController controller;
//
//     [Header("HeadBob")]
//     [SerializeField] private bool enableHeadbob = true;
//
//     public float toggleSpeed = 3f;
//     [SerializeField, Range(0, 10f)] private float amplitude = 0.015f;
//     [SerializeField, Range(0, 30f)] private float frequency = 10.3f;
//     [SerializeField] private Vector2 frequencyMultiplier;
//     [SerializeField] private Vector2 sprintFrequencyMultiplier;
//     [SerializeField] private Vector2 aimFrequencyMultiplier;
//     [SerializeField] private Vector2 crouchFrequencyMultiplier;
//
//
//     [Header("Jump Animation")]
//     [SerializeField] public JumpAnimation landAnimation;
//     [SerializeField] public JumpAnimation jumpAnimation;
//
//     [SerializeField] private float returnSpeed = 1f;
//     [SerializeField] private float animationSmoothing = 1f;
//
//     Vector2 _frequencyMultiplier;
//
//
//     private float _speed;
//
//     private Quaternion startRot;
//     private Vector3 startPos;
//
//     private Vector3 resetPosition;
//
//     void Awake()
//     {
//         startRot = jumpAnimTransform.localRotation;
//         startPos = jumpAnimTransform.localPosition;
//
//         resetPosition = cameraHolder.localPosition;
//     }
//
//     void Update()
//     {
//         ApplyHeadbob();
//         ApplyJumpSmoothing();
//     }
//
//     float p = 0f;
//     void ApplyJumpSmoothing()
//     {
//         // camera desync when landing
//         if (controller._grounded == true && landAnimation.p < landAnimation.animationLength)
//         {
//             landAnimation.p += Time.deltaTime * landAnimation.animationSpeed;
//             //jumpAnimTransform.localPosition.y = Mathf.Lerp(jumpAnimTransform.localPosition.y, landDesyncHeight.Evaluate(p));
//             Vector3 targetPosition = new Vector3(startPos.x, startPos.y + (landAnimation.magnitude * landAnimation.AnimationMotionCurve.Evaluate(landAnimation.p)), startPos.z);
//             jumpAnimTransform.localPosition = Vector3.Lerp(jumpAnimTransform.localPosition, targetPosition, animationSmoothing * Time.deltaTime);
//
//             if (p >= landAnimation.animationLength) return;
//         }
//         else
//         {
//             jumpAnimTransform.localPosition = Vector3.Lerp(jumpAnimTransform.localPosition, startPos, returnSpeed * Time.deltaTime);
//         }
//         if (controller._grounded == false)
//         {
//             landAnimation.p = 0f;
//         }
//
//         // camera desync when jumping
//         if (controller._grounded == false && jumpAnimation.p < jumpAnimation.animationLength)
//         {
//             jumpAnimation.p += Time.deltaTime * jumpAnimation.animationSpeed;
//             //jumpAnimTransform.localPosition.y = Mathf.Lerp(jumpAnimTransform.localPosition.y, landDesyncHeight.Evaluate(p));
//             Vector3 targetPosition = new Vector3(startPos.x, startPos.y + (jumpAnimation.magnitude * jumpAnimation.AnimationMotionCurve.Evaluate(jumpAnimation.p)), startPos.z);
//             jumpAnimTransform.localPosition = Vector3.Lerp(jumpAnimTransform.localPosition, targetPosition, animationSmoothing * Time.deltaTime);
//
//             if (p >= jumpAnimation.animationLength) return;
//         }
//         else
//         {
//             jumpAnimTransform.localPosition = Vector3.Lerp(jumpAnimTransform.localPosition, startPos, returnSpeed * Time.deltaTime);
//         }
//         if (controller._grounded == true)
//         {
//             jumpAnimation.p = 0.0f;
//         }
//     }
//
//     void jumpSmooth()
//     {
//         landAnimation.p += Time.deltaTime * landAnimation.animationSpeed;
//         //jumpAnimTransform.localPosition.y = Mathf.Lerp(jumpAnimTransform.localPosition.y, landDesyncHeight.Evaluate(p));
//         Vector3 targetPosition = new Vector3(startPos.x, startPos.y + (landAnimation.magnitude * landAnimation.AnimationMotionCurve.Evaluate(landAnimation.p)), startPos.z);
//         jumpAnimTransform.localPosition = Vector3.Lerp(jumpAnimTransform.localPosition, targetPosition, animationSmoothing * Time.deltaTime);
//
//         if (p >= landAnimation.animationLength) return;
//     }
//
//     void ApplyHeadbob()
//     {
//         if (!enableHeadbob) return;
//
//         CalcSpeed();
//         SetFrequency();
//
//         CheckMotion();
//         ResetPosition();
//
//         stabalizeTransform.LookAt(FocusTarget());
//
//         stabalizeTransform.localPosition -= new Vector3(0f, 0f, stabalizeTransform.localPosition.z);
//     }
//
//     void CalcSpeed()
//     {
//         Vector3 horizontalPlane = new Vector3(controller.rb.velocity.x, 0f, controller.rb.velocity.z);
//         _speed = horizontalPlane.magnitude;
//     }
//
//     void SetFrequency()
//     {
//         if (PlayerController.walking)
//         {
//             _frequencyMultiplier = frequencyMultiplier;
//         }
//         else if (PlayerController.Sprinting)
//         {
//             _frequencyMultiplier = sprintFrequencyMultiplier;
//         }
//         else if (PlayerController.aiming)
//         {
//             _frequencyMultiplier = aimFrequencyMultiplier;
//         }
//         else if (PlayerController.crouching)
//         {
//             _frequencyMultiplier = crouchFrequencyMultiplier;
//         }
//     }
//
//     public void PlayMotion(Vector3 motion)
//     {
//         // cameraHolder.localEulerAngles += motion;
//         cameraHolder.localPosition += motion;
//     }
//
//     void CheckMotion()
//     {
//         if (_speed < toggleSpeed) return;
//         if (PlayerController.grounded == false) return;
//
//         PlayMotion(FootStepMotion());
//     }
//
//     Vector3 FootStepMotion()
//     {
//         Vector3 pos = Vector3.zero;
//         pos.y += Mathf.Sin(Time.time * frequency * _frequencyMultiplier.x) * (amplitude * _frequencyMultiplier.y);
//         pos.x += Mathf.Cos(Time.time * (frequency * _frequencyMultiplier.x) / 2f) * (amplitude * _frequencyMultiplier.y) * 2f;
//         pos.z = 0f;
//         // pos.z += Mathf.Sin(Time.time * (frequency * _frequencyMultiplier.x) / 2f) * (amplitude * _frequencyMultiplier.y) / 1.5f;
//         return pos;
//     }
//
//     private void ResetPosition()
//     {
//         if (cameraHolder.localPosition != resetPosition)
//             cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, resetPosition, 5 * Time.deltaTime);
//     }
//
//     private Vector3 FocusTarget()
//     {
//         Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraHolder.localPosition.y, transform.position.z);
//         pos += cameraHolder.forward * 15.0f;
//         return pos;
//     }
// }
//
// [System.Serializable]
// public struct JumpAnimation
// {
//     public float animationLength;
//     public float magnitude;
//     public AnimationCurve AnimationMotionCurve;
//     public float animationSpeed;
//     public float p;
// }
