using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilScript : MonoBehaviour
{
    public Transform recoilTransform;

    // rotations
    Vector3 currentRotation;
    Vector3 targetRotation;

    // positions
    Vector3 currentPosition;
    Vector3 targetPosition;

    ProjectileGun gunScript;

    void Update()
    {
        gunScript = GunManager.currentgun.thisGun;

        if (GunManager.currentgun.thisGun != null)
            ResetSway();
    }
    
    void ResetSway()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, gunScript.Settings.returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, gunScript.Settings.snapiness * Time.deltaTime);

        targetPosition = Vector3.Lerp(targetPosition, Vector3.zero, gunScript.Settings.returnSpeed * Time.deltaTime);
        currentPosition = Vector3.Slerp(currentPosition, targetPosition, gunScript.Settings.snapiness * Time.deltaTime);

        recoilTransform.localRotation = Quaternion.Euler(currentRotation);
        recoilTransform.localPosition = currentPosition;
    }

    public void RecoilFire(bool isAiming)
    {
        Vector3 _recoilPosition;
        Vector3 _recoilRotation;

        if (isAiming == true)
        {
            _recoilPosition = gunScript.Settings.aimRecoilPosition;
            _recoilRotation = gunScript.Settings.aimRecoilRotation;
        }
        else
        {
            _recoilPosition = gunScript.Settings.recoilPosition;
            _recoilRotation = gunScript.Settings.recoilRotation;
        }


        targetPosition += new Vector3(_recoilPosition.x, Random.Range(-_recoilPosition.y, _recoilPosition.y), Random.Range(-_recoilPosition.z, _recoilPosition.z));
        targetRotation += new Vector3(_recoilRotation.x, Random.Range(-_recoilRotation.y, _recoilRotation.y), Random.Range(-_recoilRotation.z, _recoilRotation.z));
    }
}
