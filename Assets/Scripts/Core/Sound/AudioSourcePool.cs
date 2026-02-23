using UnityEngine;
using System.Collections;

public class AudioSourcePool : MonoBehaviour, IPool {

    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip, float volume) {

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        StartCoroutine(ReturnAfterPlay());
    }

    public void PlayOneShot(AudioClip clip, float volume) {

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.PlayOneShot(clip);

        StartCoroutine(ReturnAfterPlay());
    }

    private IEnumerator ReturnAfterPlay() {

        yield return new WaitForSeconds(audioSource.clip.length);
        PoolManager.Instance.ReturnAudio(this);
    }

    public void OnSpawned() {
    }

    public void OnDespawned() {

        audioSource.Stop();
        StopAllCoroutines();
    }
}