using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;

    public Transform sourceTransform;
    [HideInInspector] public AudioSource source;

    public AudioClip[] clips;

    public bool playOnAwake = false;

    [Range(0, 256)] public int priority = 0;

    [Range(0.0f, 1.0f)] public float spatialBlend = 1.0f;

    public Vector2 volume;

    public Vector2 pitch;
}