using UnityEngine;
using UnityEngine.Audio;

public static class AudioHelper
{
    public static AudioSource PlayClip2D(AudioClip clip, float volume, bool ignoreListenerPause)
    {
        GameObject audioObject = new GameObject("2DAudio");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        audioSource.resource = clip;
        audioSource.volume = volume;
        audioSource.ignoreListenerPause = ignoreListenerPause; //for once unity just has a straightforward function to easily do the exact thing I want it to

        audioSource.Play();
        Object.Destroy(audioObject, clip.length);
        return audioSource;
    }
}
