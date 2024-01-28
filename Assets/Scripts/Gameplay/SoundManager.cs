using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;
    public static SoundManager Instance { get => _instance; }

    public List<PlayableSound> sounds;
    private Dictionary<string, PlayableSound> soundMap;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        soundMap = new Dictionary<string, PlayableSound>();

        for (int i = 0, count = sounds.Count; i < count; i++)
        {
            PlayableSound sound = sounds[i];

            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            // do playonawake sounds differently not here
            sound.source.playOnAwake = false;
            sound.source.loop = sound.loop;

            if (!soundMap.ContainsKey(sound.name))
            {
                soundMap.Add(sound.name, sound);
            }
            else
            {
                Debug.LogError($"Duplicate sound {sound.name} found.");
            }
        }
    }

    public void PlaySound(string sound)
    {
        if (soundMap.TryGetValue(sound, out PlayableSound playable))
        {
            playable.source.Play();
        }
        else
        {
            Debug.LogError($"Sound {sound} not found");
        }
    }

    public void StopSound(string sound)
    {
        if (soundMap.TryGetValue(sound, out PlayableSound playable))
        {
            playable.source.Stop();
        }
        else
        {
            Debug.LogError($"Sound {sound} not found");
        }
    }
}

[System.Serializable]
public class PlayableSound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1.0f;
    [Range(0f, 1.5f)]
    public float pitch = 1.0f;

    public bool loop;
    public float spatialBlend;

    [HideInInspector]
    public AudioSource source;
}
