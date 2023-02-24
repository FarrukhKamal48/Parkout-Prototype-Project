using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmIKHandler : MonoBehaviour
{
    [SerializeField] private List<IKLerp> R_locations = new List<IKLerp>();
    [SerializeField] private List<IKLerp> L_locations = new List<IKLerp>();
    [SerializeField] private GunManager gunManager;

    private List<Transform> R_targetTransforms = new List<Transform>();
    private List<Transform> L_targetTransforms = new List<Transform>();

    Transform R_current_T_toMove;
    Transform R_current_T_target;

    Transform L_current_T_toMove;
    Transform L_current_T_target;
    
    void Start()
    {
        SetupTargets();
    }
    
    void Update()
    {
        for (int i = 0; i < R_locations.Count-1; i++)
        {
            // R_targetTransforms[i] = GunManager.currentgun.R_targetTransforms[i];
            // L_targetTransforms[i] = GunManager.currentgun.L_targetTransforms[i];
            // R_targetTransforms.Add(GunManager.currentgun.R_targetTransforms[i]);
            // L_targetTransforms.Add(GunManager.currentgun.L_targetTransforms[i]);

            // for right arm

            R_current_T_toMove = R_locations[i].transformToMove;
            R_current_T_target = R_targetTransforms[i];

            // lerping Positions
            R_current_T_toMove.position = Vector3.Lerp(R_current_T_toMove.position, R_current_T_target.position,
                    R_locations[i].lerpSpeeds.x * Time.deltaTime);

            // lerping Rotations
            R_current_T_toMove.rotation = Quaternion.Slerp(R_current_T_toMove.rotation, R_current_T_target.rotation,
                    R_locations[i].lerpSpeeds.y * Time.deltaTime);



            // for left arm

            L_current_T_toMove = L_locations[i].transformToMove;
            L_current_T_target = L_targetTransforms[i];

            // lerping Positions
            L_current_T_toMove.position = Vector3.Lerp(L_current_T_toMove.position, L_current_T_target.position,
                    L_locations[i].lerpSpeeds.x * Time.deltaTime);

            // lerping Rotations
            L_current_T_toMove.rotation = Quaternion.Slerp(L_current_T_toMove.rotation, L_current_T_target.rotation,
                    L_locations[i].lerpSpeeds.y * Time.deltaTime);
        }
    }
    
    void SetupTargets()
    {
        for (int i = 0; i < R_locations.Count-1; i++)
        {
            // R_targetTransforms[i] = GunManager.currentgun.R_targetTransforms[i];
            // L_targetTransforms[i] = GunManager.currentgun.L_targetTransforms[i];
            R_targetTransforms.Add(GunManager.currentgun.R_targetTransforms[i]);
            L_targetTransforms.Add(GunManager.currentgun.L_targetTransforms[i]);
        }     
    }
    
    public void updateTargets()
    {
        for (int i = 0; i < R_locations.Count-1; i++)
        {
            R_targetTransforms[i] = GunManager.currentgun.R_targetTransforms[i];
            L_targetTransforms[i] = GunManager.currentgun.L_targetTransforms[i];
            // R_targetTransforms.Add(GunManager.currentgun.R_targetTransforms[i]);
            // L_targetTransforms.Add(GunManager.currentgun.L_targetTransforms[i]);
        }     
    }
}
[System.Serializable]
public struct IKLerp
{
    public string name;

    public Transform transformToMove;

    public Vector2 lerpSpeeds;
}

