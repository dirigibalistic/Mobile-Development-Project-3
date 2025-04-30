using UnityEngine;
using UnityEngine.Audio;

public static class AudioHelper
{
    public static AudioSource PlayClip2D(AudioClip clip, float volume)
    {
        GameObject audioObject = new GameObject("2DAudio");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        audioSource.resource = clip;
        audioSource.volume = volume;

        audioSource.Play();
        Object.Destroy(audioObject, clip.length);
        return audioSource;
    }
}
