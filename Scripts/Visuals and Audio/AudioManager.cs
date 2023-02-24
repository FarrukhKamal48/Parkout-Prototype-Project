using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = s.sourceTransform.gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clips[0];

            s.source.playOnAwake = false;

            s.source.priority = 0;
            s.source.volume = UnityEngine.Random.Range(s.volume.x, s.volume.y);
            s.source.pitch = UnityEngine.Random.Range(s.pitch.x, s.pitch.y);

            s.source.spatialBlend = 1.0f;
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        s.source.Stop();
        s.source.clip = s.clips[UnityEngine.Random.Range(0, s.clips.Length)];
        s.source.volume = UnityEngine.Random.Range(s.volume.x, s.volume.y);
        s.source.pitch = UnityEngine.Random.Range(s.pitch.x, s.pitch.y);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }

        s.source.Play();
    }

    public void PlaySound(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        s.source.Stop();
        s.source.clip = s.clips[UnityEngine.Random.Range(0, s.clips.Length)];

        float volumeRange = s.volume.y - s.volume.x;
        s.source.volume = UnityEngine.Random.Range(volume - volumeRange/2f, volume + volumeRange/2f);

        s.source.pitch = UnityEngine.Random.Range(s.pitch.x, s.pitch.y);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }

        s.source.Play();
    }
}
