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
    
    public static float distMap(Vector3 runtimePos, Vector3 start, Vector3 end) 
    {
        float targetDist = Vector3.Distance(start, end);
        float currentDist = Vector3.Distance(runtimePos, end);
        float traveledDist = targetDist - currentDist;

        return 0.001f + traveledDist / targetDist;
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
