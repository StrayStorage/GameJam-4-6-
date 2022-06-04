using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance;

    public List<AudioClip> soundEffectClips;

    public AudioSource soundEffectSource;

    public AudioSource bgmSource;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlaySoundEffect(int index)
    {
        soundEffectSource.PlayOneShot(soundEffectClips[index]);
    }
}
