using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SoundData {

    public SoundType soundType;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
}

public class SoundManager : MonoBehaviour {

    public static SoundManager Instance { get; private set; }

    [SerializeField] private List<SoundData> sounds = new List<SoundData>();

    private Dictionary<SoundType, SoundData> soundDictionary = new Dictionary<SoundType, SoundData>();

    private void Awake() {

        if(Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InitializeDictionary();
    }

    private void InitializeDictionary() {

        foreach(SoundData data in sounds) {

            if(!soundDictionary.ContainsKey(data.soundType)) {
                soundDictionary.Add(data.soundType, data);
            }
        }
    }

    public void Play(SoundType soundType) {

        if(!soundDictionary.ContainsKey(soundType)) {
            return;
        }

        SoundData sound = soundDictionary[soundType];

        AudioSourcePool source = PoolManager.Instance.GetAudio();
        source.Play(sound.clip, sound.volume);
    }

    public void PlayOneShot(SoundType soundType) {

        if(!soundDictionary.ContainsKey(soundType)) {
            return;
        }

        SoundData sound = soundDictionary[soundType];

        AudioSourcePool source = PoolManager.Instance.GetAudio();
        source.PlayOneShot(sound.clip, sound.volume);
    }
}