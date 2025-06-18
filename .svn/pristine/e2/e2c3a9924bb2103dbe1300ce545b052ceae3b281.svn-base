using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEvent : MonoBehaviour
{
    public AudioClip hit;
    public AudioClip attack_1;
    public AudioClip attack_2;
    public AudioClip attack_3;
    public AudioClip skill_1;
    public AudioClip skill_2;
    public AudioClip skill_3;
    public AudioClip skill_4;
    public AudioClip skill_5;
    public AudioClip skill_6;

    public float bgm_base_volume = 1;

    private Dictionary<string, AudioClip> dict_Sound;

    void Awake()
    {
        // Initialize the dictionary and add the sounds
        dict_Sound = new Dictionary<string, AudioClip>
        {
            { "hit", hit },
            { "attack_1", attack_1 },
            { "attack_2", attack_2 },
            { "attack_3", attack_3 },
            { "skill_1", skill_1 },
            { "skill_2", skill_2 },
            { "skill_3", skill_3 },
            { "skill_4", skill_4 },
            { "skill_5", skill_5 },
            { "skill_6", skill_6 }
        };
    }

    public void PlaySound(string _snd , GameObject _obj)
    {
        if (!dict_Sound.ContainsKey(_snd))
        {
            Debug.LogWarning($"Sound key '{_snd}' not found in dictionary.");
            return;
        }

        AudioClip clipToPlay = dict_Sound[_snd];

        if (_obj)
        {
            // Ensure the GameObject has an AudioSource component
            AudioSource audioSource = _obj.GetComponent<AudioSource>();
            if (!audioSource)
                audioSource = _obj.AddComponent<AudioSource>();

            audioSource.loop = false;
            audioSource.clip = clipToPlay;
            audioSource.volume = bgm_base_volume;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("GameObject is null. Cannot play sound.");
        }
    }
}
