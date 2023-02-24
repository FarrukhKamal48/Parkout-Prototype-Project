using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilCurves : MonoBehaviour
{
    /*
    public AnimationCurve[] position = {
        new AnimationCurve(),
        new AnimationCurve(),
        new AnimationCurve()
    };

    public AnimationCurve[] rotation = {
        new AnimationCurve(),
        new AnimationCurve(),
        new AnimationCurve()
    };*/

    public static IEnumerator StartAnimation(Transform anim_transform, AnimationCurve[] position, AnimationCurve[] rotation, float duration)
    {
        float currentTime = 0.0f;

        while (currentTime < duration)
        {
            anim_transform.localPosition = new Vector3(
            position[0].Evaluate(currentTime),
            position[1].Evaluate(currentTime),
            position[2].Evaluate(currentTime)
            );

            anim_transform.localRotation = Quaternion.Euler(new Vector3(
                rotation[0].Evaluate(currentTime),
                rotation[1].Evaluate(currentTime),
                rotation[2].Evaluate(currentTime)
            ));

            currentTime += Time.deltaTime;

            yield return null;
        }
    }
}
