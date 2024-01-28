using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundContainer : MonoBehaviour
{
    public List<PlayableSound> sounds;
    private Dictionary<string, PlayableSound> soundMap;

    private void Awake()
    {
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

            sound.source.spatialBlend = sound.spatialBlend;
            sound.source.spatialize = sound.spatialBlend > 0;

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
}
