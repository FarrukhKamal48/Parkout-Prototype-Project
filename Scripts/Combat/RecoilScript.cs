using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilScript : MonoBehaviour
{
    public GunSettings gunSettings;

    public Transform recoilTransform;

    // rotations
    Vector3 currentRotation;
    Vector3 targetRotation;

    // positions
    Vector3 currentPosition;
    Vector3 targetPosition;

    void Update()
    {
        if (gunSettings != null)
            ResetSway();
    }
    
    void ResetSway()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, gunSettings.returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, gunSettings.snapiness * Time.deltaTime);

        targetPosition = Vector3.Lerp(targetPosition, Vector3.zero, gunSettings.returnSpeed * Time.deltaTime);
        currentPosition = Vector3.Slerp(currentPosition, targetPosition, gunSettings.snapiness * Time.deltaTime);

        recoilTransform.localRotation = Quaternion.Euler(currentRotation);
        recoilTransform.localPosition = currentPosition;
    }

    public void RecoilFire(bool isAiming)
    {
        Vector3 _recoilPosition;
        Vector3 _recoilRotation;

        if (isAiming == true)
        {
            _recoilPosition = gunSettings.aimRecoilPosition;
            _recoilRotation = gunSettings.aimRecoilRotation;
        }
        else
        {
            _recoilPosition = gunSettings.recoilPosition;
            _recoilRotation = gunSettings.recoilRotation;
        }


        targetPosition += new Vector3(_recoilPosition.x, Random.Range(-_recoilPosition.y, _recoilPosition.y), Random.Range(-_recoilPosition.z, _recoilPosition.z));
        targetRotation += new Vector3(_recoilRotation.x, Random.Range(-_recoilRotation.y, _recoilRotation.y), Random.Range(-_recoilRotation.z, _recoilRotation.z));
    }
}
