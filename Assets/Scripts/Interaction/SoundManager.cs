using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public Sound[] sounds;
    private static Dictionary<string, float> soundTimerDictionary;

    public static event Action<string> playByNameEvent;

    [Header("Audio Option")]
    public bool useSound = true;

    //Singleton
    public static SoundManager instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        soundTimerDictionary = new Dictionary<string, float>();

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.isLoop;

            if (sound.hasCooldown)
            {
                Debug.Log(sound.name);
                soundTimerDictionary[sound.name] = 0f;
            }
        }

        //Subscribe
        playByNameEvent += Play;
    }

    private void OnDisable()
    {
        //Unsubscribe to event 
        playByNameEvent -= Play;
    }

    public void Play(string name)
    {
        if (useSound)
        {
            //Debug.Log("Play " + name);
            Sound sound = Array.Find(sounds, s => s.name == name);

            if (sound == null)
            {
                Debug.LogError("Sound " + name + " Not Found!");
                return;
            }

            if (!CanPlaySound(sound)) { Debug.Log("Can't play " + name); return; }

            sound.source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);

        if (sound == null)
        {
            Debug.LogError("Sound " + name + " Not Found!");
            return;
        }

        sound.source.Stop();
    }

    private static bool CanPlaySound(Sound sound)
    {
        if (soundTimerDictionary.ContainsKey(sound.name))
        {
            float lastTimePlayed = soundTimerDictionary[sound.name];

            if (lastTimePlayed + sound.clip.length < Time.time)
            {
                soundTimerDictionary[sound.name] = Time.time;
                return true;
            }

            return false;
        }

        return true;
    }

    public static void PlayByNameEvent(string name)
    {
        playByNameEvent?.Invoke(name);
    }
}
