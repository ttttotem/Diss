using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    /* use:
    AudioManager am = FindObjectOfType<AudioManager>();
            if(am != null)
            {
                am.Play("correct");
            }
    */

private void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Start()
    {
        Play("levelTheme");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds,sound => sound.name == name);
        if(s == null)
        {
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.source.Stop();
    }

    public void StartFadeStarter(string name, float duration, float targetVolume)
    {
        StopAllCoroutines();
        StartCoroutine(StartFade(name, duration, targetVolume));

    }

    public IEnumerator StartFade(string name, float duration, float targetVolume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        float currentTime = 0;
        float start = s.source.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            s.source.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
