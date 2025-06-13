using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip deathClip;

    [Header("Audio Settings")]
    [SerializeField] private float musicVolume = 0.1f;

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    private void InitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBackgroundMusic()
    {
        SetupMusicSource();
        musicSource.Play();
    }

    private void SetupMusicSource()
    {
        musicSource.clip = musicClip;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
    }

    public void PlayJumpSound()
    {
        PlaySoundEffect(jumpClip);
    }

    public void PlayDeathSound()
    {
        PlaySoundEffect(deathClip);
    }

    private void PlaySoundEffect(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}