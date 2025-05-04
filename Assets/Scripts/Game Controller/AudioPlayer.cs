using Unity.VisualScripting;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _prepPhaseMusic;
    [SerializeField] private AudioClip _combatPhaseMusic;
    [SerializeField] private AudioClip _buttonSound;
    [SerializeField] private AudioClip _startRoundSound;
    [SerializeField] private AudioClip _playerDamageSound;
    [SerializeField] private AudioClip _loseSound;
    [SerializeField] private AudioClip _winSound;

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

    public void PlayWinSound()
    {
        AudioHelper.PlayClip2D(_winSound, 0.5f, true);
    }
    public void PlayLoseSound()
    {
        AudioHelper.PlayClip2D(_loseSound, 0.5f, true);
    }
}