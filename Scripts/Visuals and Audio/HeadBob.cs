// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform jumpAnimTransform;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private PlayerController controller;

    [Header("HeadBob")]
    [SerializeField] private bool enableHeadbob = true;

    [Header("Base Settings")]
    [SerializeField] private float toggleSpeed = 3f;
    [SerializeField, Range(0, 10f)] private float amplitude = 0.015f;
    [SerializeField, Range(0, 30f)] private float frequency = 10.3f;
    [SerializeField] private float Ybump = 1f; 
    [SerializeField] private bool ZRotBob = false; 
    [SerializeField] private bool ZMoveTilt = true; 
    [SerializeField] private bool YPosBob = true; 

    [Space(5)]
    [Header("Y postion frequency & amplitude")]
    [Space(3)]
    [SerializeField] private Vector2 yWalkFreqMultiplier;
    [SerializeField] private Vector2 yAimFreqMultiplier;
    [SerializeField] private Vector2 ySprintFreqMultiplier;
    [SerializeField] private Vector2 yCrouchFreqMultiplier;

    [Space(5)]
    [Header("Rotaion Bob frequency & amplitude")]
    [Space(3)]
    [SerializeField] private Vector2 frequencyMultiplier;
    [SerializeField] private Vector2 sprintFrequencyMultiplier;
    [SerializeField] private Vector2 aimFrequencyMultiplier;
    [SerializeField] private Vector2 crouchFrequencyMultiplier;

    [Space(5)]
    [Header("Z tilt ammount & smoothing")]
    [Space(3)]
    // [SerializeField] private float _zRotandSmooth.x = 2f; 
    // [SerializeField] private float _zRotandSmooth.y = 2f; 
    [SerializeField] private Vector2 zWalkRotandSmooth;
    [SerializeField] private Vector2 zAimRotandSmooth;
    [SerializeField] private Vector2 zSprintRotandSmooth;
    [SerializeField] private Vector2 zCrouchRotandSmooth;


    [Header("Jump Animation")]
    [SerializeField] public JumpAnimation landAnimation;
    [SerializeField] public JumpAnimation jumpAnimation;

    [SerializeField] private float returnSpeed = 1f;
    [SerializeField] private float animationSmoothing = 1f;

    Vector2 _frequencyMultiplier;
    Vector2 _yFreqMultiplier;
    Vector2 _zRotandSmooth;

    private float _speed;

    private Quaternion startRot;
    private Vector3 startPos;
    private Vector3 jumpAnimStartPos;

    void Awake()
    {
        startRot = cameraHolder.localRotation;
        startPos = cameraHolder.localPosition;

        jumpAnimStartPos = jumpAnimTransform.localPosition;
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
            Vector3 targetPosition = new Vector3(jumpAnimStartPos.x, jumpAnimStartPos.y + (landAnimation.magnitude * landAnimation.AnimationMotionCurve.Evaluate(landAnimation.p)), jumpAnimStartPos.z);
            jumpAnimTransform.localPosition = Vector3.Lerp(jumpAnimTransform.localPosition, targetPosition, animationSmoothing * Time.deltaTime);

            if (p >= landAnimation.animationLength) return;
        }
        else
        {
            jumpAnimTransform.localPosition = Vector3.Lerp(jumpAnimTransform.localPosition, jumpAnimStartPos, returnSpeed * Time.deltaTime);
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
            Vector3 targetPosition = new Vector3(jumpAnimStartPos.x, jumpAnimStartPos.y + (jumpAnimation.magnitude * jumpAnimation.AnimationMotionCurve.Evaluate(jumpAnimation.p)), jumpAnimStartPos.z);
            jumpAnimTransform.localPosition = Vector3.Lerp(jumpAnimTransform.localPosition, targetPosition, animationSmoothing * Time.deltaTime);

            if (p >= jumpAnimation.animationLength) return;
        }
        else
        {
            jumpAnimTransform.localPosition = Vector3.Lerp(jumpAnimTransform.localPosition, jumpAnimStartPos, returnSpeed * Time.deltaTime);
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
        Vector3 targetPosition = new Vector3(jumpAnimStartPos.x, jumpAnimStartPos.y + (landAnimation.magnitude * landAnimation.AnimationMotionCurve.Evaluate(landAnimation.p)), jumpAnimStartPos.z);
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
            _yFreqMultiplier = yWalkFreqMultiplier;
            _zRotandSmooth = zWalkRotandSmooth;
        }
        else if (PlayerController.Sprinting)
        {
            _frequencyMultiplier = sprintFrequencyMultiplier;
            _yFreqMultiplier = ySprintFreqMultiplier;
            _zRotandSmooth = zSprintRotandSmooth;
        }
        else if (PlayerController.aiming)
        {
            _frequencyMultiplier = aimFrequencyMultiplier;
            _yFreqMultiplier = yAimFreqMultiplier;
            _zRotandSmooth = zAimRotandSmooth;
        }
        else if (PlayerController.crouching)
        {
            _frequencyMultiplier = crouchFrequencyMultiplier;
            _yFreqMultiplier = yCrouchFreqMultiplier;
            _zRotandSmooth = zCrouchRotandSmooth;
        }
    }

    public void PlayMotion(Vector3 motion)
    {
        cameraHolder.localEulerAngles += motion;

        if (YPosBob) {
            Vector3 pos = cameraHolder.localPosition;
            pos.x = cameraHolder.localPosition.y + Mathf.Sin(Time.time * _yFreqMultiplier.x * frequency) * _yFreqMultiplier.y * amplitude;
            cameraHolder.localPosition = pos;
        }

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
        
        if (ZMoveTilt)
            pos.z = Mathf.Lerp(pos.z, -PlayerController.X * _zRotandSmooth.x, Time.deltaTime * _zRotandSmooth.y);

        if (ZRotBob)
            pos.z += Mathf.Sin(Time.time * (frequency * _frequencyMultiplier.x) / 2f) * (amplitude * _frequencyMultiplier.y) / 1f;

        return pos;
    }

    private void ResetPosition()
    {
        if (cameraHolder.localRotation != startRot)
            cameraHolder.localRotation = Quaternion.Slerp(cameraHolder.localRotation, startRot, 5 * Time.deltaTime);

        if (cameraHolder.localPosition != startPos)
            cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, startPos, 5 * Time.deltaTime);
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