using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool isVoiceOn;
    public Sound[] sounds;
    public static AudioManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }


    }

    private void Start()
    {
        Play("Main Theme");
    }

    private void Update()
    {
        setVoice("Main Theme");
    }
    private void setVoice(String name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (!isVoiceOn)
        {
            s.source.volume = 0;
        }

    }
    public void Play(string name)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound" + name + "not found !");
            return;
        }
        s.source.Play();
    }

    public void stopPlaying(String name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound" + name + "not found !");
            return;
        }

        s.source.Stop();
    }
}
