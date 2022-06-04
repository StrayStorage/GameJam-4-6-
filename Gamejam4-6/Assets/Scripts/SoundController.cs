using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundController : MonoBehaviour
{
    

    public static SoundController Instance;

    public List<AudioClip> soundEffectClips;

    public AudioSource soundEffectSource;

    public AudioSource bgmSource;

    public List<PlayerData> PlayerDataList = new List<PlayerData>();

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



[Serializable]
public class PlayerData
{
    public string Name;
    public string Email;
    public string Password;

    public PlayerData(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }
}
