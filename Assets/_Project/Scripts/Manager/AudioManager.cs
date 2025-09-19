using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _maxAudio;
    [SerializeField] private AudioSource _sfxAudio;
    [SerializeField] private AudioClip _soundAudio;
    [SerializeField] private AudioClip _gameoverAudio;
    [SerializeField] private AudioClip _coinSound;
    [SerializeField] private AudioClip _asteroidSound;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound()
    {
        if (_soundAudio != null)
        {
            _maxAudio.Stop();
            _maxAudio.time = 0f;
            _maxAudio.clip = _soundAudio;
            _maxAudio.loop = true;
            _maxAudio.Play();
        }
    }

    public void PlayCoinSound()
    {
        if (_coinSound != null)
        {
            _sfxAudio.PlayOneShot(_coinSound);
        }
    }

    public void PlayAsteroidSound()
    {
        if (_asteroidSound != null)
        {
            _sfxAudio.PlayOneShot(_asteroidSound);
        }
    }

    public void PlayGameOverSound()
    {

        if (_gameoverAudio != null)
        {
            _maxAudio.Stop();
            _sfxAudio.Stop();
            _maxAudio.time = 0f;
            _sfxAudio.time = 0f;
            _maxAudio.clip = _gameoverAudio;
            _maxAudio.loop = true;
            _maxAudio.Play();
        }
    }

    public void StopSound()
    {
        _maxAudio.Stop();
    }
}

