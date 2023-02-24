using UnityEngine;

[CreateAssetMenu(fileName = "MeleeSettings", menuName = "WeaponSettings/MeleeSettings", order = 0)]
public class MeleeSettings : ScriptableObject 
{
    public new string name;
    public GameObject impactEffect;

    [Space(5)]
    [Header("Attack Settings")]
    public float attackRate;
    public int maxCombo = 3;
    public float comboResetTimer = 0.5f;
    public bool allowButtonHold;
    
    public float triggerRadious = 0.3f;
    public bool[] isRightChoosen;


    [Space(5)]
    [Header("Focus Settings")]
    public bool aimHold;

    [Space(5)]
    [Header("Animation Settings")]
    public float aimSpeed;
    public float idleSpeed;

    public float normalAnimSpeed; 
    public float shootAnimSpeed; 
    public float sprintAnimSpeed; 
}
