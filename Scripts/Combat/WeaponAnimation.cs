using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    public enum Animatemode
    {
        _transform,
        _position,
        _rotation
    };

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
