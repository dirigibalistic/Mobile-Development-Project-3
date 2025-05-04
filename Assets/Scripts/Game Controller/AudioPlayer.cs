using Unity.VisualScripting;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _prepPhaseMusic;
    [SerializeField] private AudioClip _combatPhaseMusic;
    [SerializeField] private AudioClip _buttonSound;
    [SerializeField] private AudioClip _startRoundSound;
    [SerializeField] private AudioClip _playerDamageSound;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayButtonSound()
    {
        AudioHelper.PlayClip2D(_buttonSound, 0.5f, true);
    }

    public void PlayStartRoundSound()
    {
        AudioHelper.PlayClip2D(_startRoundSound, 0.5f, true);
    }

    public void PlayPrepMusic()
    {
        _audioSource.clip = _prepPhaseMusic;
        _audioSource.Play();
    }

    public void PlayCombatMusic()
    {
        _audioSource.clip = _combatPhaseMusic;
        _audioSource.Play();
    }

    public void StopMusic()
    {
       _audioSource.Stop();
    }
}