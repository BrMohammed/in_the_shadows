using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
            
        DontDestroyOnLoad(gameObject);
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volum;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        
    }
    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            //Debug.LogWarning("Sound: " + name + " not found !");
            return;
        }
        s.source.Play();
    }
    public void MuteSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            //Debug.LogWarning("Sound: " + name + " not found !");
            return;
        }
        s.source.mute = !s.source.mute;
    }
    public void MuteShield(string name,bool mute)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            //Debug.LogWarning("Shield soun error");
            return;
        }
        if(mute == true)
            s.source.mute = true;
        else
            s.source.mute = false;
    }
    public void PauseSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            //Debug.LogWarning("Sound: " + name + " not found !");
            return;
        }
        s.source.Pause();
    }
    public void StopeSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            //Debug.LogWarning("Sound: " + name + " not found !");
            return;
        }
        s.source.Stop();
    }
    private void Start()
    {
        PlaySound("Theme");
    }
}
