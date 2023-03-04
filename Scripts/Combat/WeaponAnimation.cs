﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    [System.Serializable]
    public struct CurveAnimation {
        public float animationLength;
        public float magnitude;
        public AnimationCurve AnimationMotionCurve;
        public float animationSpeed;
        public float p;
    }

    [System.Serializable]
    public struct CurveAnimationVector3 {

        public AnimationCurve X;
        public AnimationCurve Y;
        public AnimationCurve Z;
        
        public float p;
    }

    public enum Animatemode
    {
        _transform,
        _position,
        _rotation
    };
    
    public static float distMap(Vector3 runtimePos, Vector3 start, Vector3 end) 
    {
        float targetDist = Vector3.Distance(start, end);
        float currentDist = Vector3.Distance(runtimePos, end);
        float traveledDist = targetDist - currentDist;

        return 0.001f + traveledDist / targetDist;
    }

    public static float procAnimate(ref float p, float target, AnimationCurve animCurve, float multiplier)
    {
        p = Mathf.MoveTowards(p, target, Time.deltaTime);
        float _animSpeed = multiplier * animCurve.Evaluate(p);
		
		_animSpeed = p>=target ? multiplier : _animSpeed;
        
        return _animSpeed;
    }


    public static void Animate(Transform start, Transform end, float speed, Animatemode animMode)
    {
        switch (animMode)
        {
            case Animatemode._transform:
                start.localPosition = Vector3.Lerp(start.localPosition, end.localPosition, speed * Time.deltaTime);
                start.localRotation = Quaternion.Slerp(start.localRotation, end.localRotation, speed * Time.deltaTime);
                break;
            case Animatemode._position:
                start.localPosition = Vector3.Lerp(start.localPosition, end.localPosition, speed * Time.deltaTime);
                break;
            case Animatemode._rotation:
                start.localRotation = Quaternion.Slerp(start.localRotation, end.localRotation, speed * Time.deltaTime);
                break;

        }
    }

    public static void Animate (Transform moveTransform, Transform start, Transform end, CurveAnimationVector3 posCurves, CurveAnimationVector3 rotCurves, Animatemode animMode) {

        switch (animMode) {
            case Animatemode._transform:

                // "play" the animation
                posCurves.p += Time.deltaTime;
                rotCurves.p += Time.deltaTime;

                // lerp the position
                moveTransform.localPosition = new Vector3(
                    Mathf.Lerp(start.localPosition.x, end.localPosition.x, posCurves.X.Evaluate(posCurves.p) * Time.deltaTime),
                    Mathf.Lerp(start.localPosition.y, end.localPosition.y, posCurves.Y.Evaluate(posCurves.p) * Time.deltaTime),
                    Mathf.Lerp(start.localPosition.z, end.localPosition.z, posCurves.Z.Evaluate(posCurves.p) * Time.deltaTime)
                );

                // lerp the rotation
                start.localRotation = Quaternion.Euler(new Vector3 (
                    Mathf.Lerp(start.localPosition.x, end.localPosition.x, posCurves.X.Evaluate(posCurves.p) * Time.deltaTime),
                    Mathf.Lerp(start.localPosition.y, end.localPosition.y, posCurves.Y.Evaluate(posCurves.p) * Time.deltaTime),
                    Mathf.Lerp(start.localPosition.z, end.localPosition.z, posCurves.Z.Evaluate(posCurves.p) * Time.deltaTime)
                ));
                break;

            case Animatemode._position:

                // "play" the animation
                posCurves.p += Time.deltaTime;

                // lerp the position
                moveTransform.localPosition = new Vector3(
                    Mathf.Lerp(start.localPosition.x, end.localPosition.x, posCurves.X.Evaluate(posCurves.p) * Time.deltaTime),
                    Mathf.Lerp(start.localPosition.y, end.localPosition.y, posCurves.Y.Evaluate(posCurves.p) * Time.deltaTime),
                    Mathf.Lerp(start.localPosition.z, end.localPosition.z, posCurves.Z.Evaluate(posCurves.p) * Time.deltaTime)
                );
                break;
                
            case Animatemode._rotation:

                // "play" the animation
                rotCurves.p += Time.deltaTime;

                // lerp the rotation
                start.localRotation = Quaternion.Euler(new Vector3 (
                    Mathf.Lerp(start.localPosition.x, end.localPosition.x, posCurves.X.Evaluate(posCurves.p) * Time.deltaTime),
                    Mathf.Lerp(start.localPosition.y, end.localPosition.y, posCurves.Y.Evaluate(posCurves.p) * Time.deltaTime),
                    Mathf.Lerp(start.localPosition.z, end.localPosition.z, posCurves.Z.Evaluate(posCurves.p) * Time.deltaTime)
                ));
                break;

        }
    }

    public static void Animate(Transform start, Transform end, Vector3 pos_speed, float rot_speed, Animatemode animMode)
    {
        switch (animMode)
        {
            case Animatemode._transform:

                start.localPosition = new Vector3(
                    Mathf.Lerp(start.localPosition.x, end.localPosition.x, pos_speed.x * Time.deltaTime),
                    Mathf.Lerp(start.localPosition.y, end.localPosition.y, pos_speed.y * Time.deltaTime),
                    Mathf.Lerp(start.localPosition.z, end.localPosition.z, pos_speed.z * Time.deltaTime)
                );

                start.localRotation = Quaternion.Slerp(start.localRotation, end.localRotation, rot_speed * Time.deltaTime);

                break;
            case Animatemode._position:

                start.localPosition = new Vector3(
                    Mathf.Lerp(start.localPosition.x, end.localPosition.x, pos_speed.x * Time.deltaTime),
                    Mathf.Lerp(start.localPosition.y, end.localPosition.y, pos_speed.y * Time.deltaTime),
                    Mathf.Lerp(start.localPosition.z, end.localPosition.z, pos_speed.z * Time.deltaTime)
                );

                break;
            case Animatemode._rotation:

                start.localRotation = Quaternion.Slerp(start.localRotation, end.localRotation, rot_speed * Time.deltaTime);

                break;

        }
    }
    /*
    public static void Animate(Vector3 start, Vector3 end, float speed)
    {
        start = Vector3.Lerp(start, end, speed * Time.deltaTime);
    }

    public static void Animate(Transform start, Transform end, float speed, Animtemode animMode)
    {
        start = Quaternion.Slerp(start, end, speed * Time.deltaTime);
    }*/
}

[System.Serializable]
public struct CurveAnimation
{
    public float animationLength;
    public float magnitude;
    public AnimationCurve AnimationMotionCurve;
    public float animationSpeed;
    public float p;
}

[System.Serializable]
public struct CurveAnimationVector3 {

    public AnimationCurve X;
    public AnimationCurve Y;
    public AnimationCurve Z;
    
    public float p;
}
