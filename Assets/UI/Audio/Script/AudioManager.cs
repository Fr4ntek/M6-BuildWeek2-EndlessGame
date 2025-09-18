using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager I { get; private set; }

    [Header("Clips")]
    public AudioClip hoverClip;
    public AudioClip selectClip;
    public AudioClip menuMusic;

    [Header("Volumes")]
    [Range(0f, 1f)] public float sfxVolume = 0.9f;
    [Range(0f, 1f)] public float musicVolume = 0.6f;

    AudioSource sfxSource;
    AudioSource musicSource;

    float lastHoverTime;
    [SerializeField] float hoverCooldown = 0.05f; // 50 ms

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        // SFX source
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;

        // Music source
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;

        ApplyVolumes();
    }

    void Start()
    {
        if (menuMusic) PlayMusic(menuMusic);
    }

    public void PlayHover()
    {
        if (!hoverClip) return;

        if (Time.unscaledTime - lastHoverTime < hoverCooldown)
            return; // troppo ravvicinato, skip

        sfxSource.PlayOneShot(hoverClip, sfxVolume);
        lastHoverTime = Time.unscaledTime;
    }

    public void PlaySelect()
    {
        if (selectClip) sfxSource.PlayOneShot(selectClip, sfxVolume);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (!clip) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;
        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void StopMusic() => musicSource.Stop();

    public void SetSfxVolume(float v) { sfxVolume = Mathf.Clamp01(v); ApplyVolumes(); }
    public void SetMusicVolume(float v) { musicVolume = Mathf.Clamp01(v); ApplyVolumes(); }
    public void MuteSfx(bool mute) { sfxSource.mute = mute; }
    public void MuteMusic(bool mute) { musicSource.mute = mute; }

    void ApplyVolumes()
    {
        sfxSource.volume = sfxVolume;
        musicSource.volume = musicVolume;
    }


}
