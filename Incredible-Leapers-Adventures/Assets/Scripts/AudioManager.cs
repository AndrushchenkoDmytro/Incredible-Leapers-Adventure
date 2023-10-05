using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioMixer audioMixer;
     
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip mainMenuMusic;
    [SerializeField] AudioClip bossFightMusic;
    [SerializeField] AudioClip[] backGroundMusic;

    [SerializeField] AudioSource effectSource;
    [SerializeField] AudioClip[] enemyDeathSound;
    [SerializeField] AudioClip levelSelect;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadValue();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (volume > 0)
        {
            volume = -20 + volume * 40f;
        }
        else
        {
            volume = -80;
        }
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public float GetMusicVolume()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            audioMixer.GetFloat("MusicVolume", out float volume);
            if (volume > -20)
            {
                volume = (20 + volume) * 0.025f;
            }
            else
            {
                volume = 0;
            }
            return volume;
        }
        return 0.5f;
    }

    public void SetEffectVolume(float volume)
    {
        if (volume > 0)
        {
            volume = -20 + volume * 40f;
        }
        else
        {
            volume = -80;
        }
        audioMixer.SetFloat("EffectVolume", volume);
        PlayerPrefs.SetFloat("EffectVolume", volume);
    }

    public float GetEffectVolume()
    {
        if (PlayerPrefs.HasKey("EffectVolume"))
        {
            audioMixer.GetFloat("EffectVolume", out float volume);
            if (volume > -20)
            {
                volume = (20 + volume) * 0.025f;
            }
            else
            {
                volume = 0;
            }
            return volume;
        }
        return 0.5f;
    }

    private void LoadValue()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
            audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        else
            SetMusicVolume(0.5f);

        if (PlayerPrefs.HasKey("EffectVolume"))
            audioMixer.SetFloat("EffectVolume", PlayerPrefs.GetFloat("EffectVolume"));
        else
            SetMusicVolume(0.5f);
    }

    public void PlayEnemyDeathAudioEffect()
    {
        int index =  Random.Range(0, enemyDeathSound.Length);
        effectSource.PlayOneShot(enemyDeathSound[index]);
    }

    public void PlayAudioEffect(AudioClip clip, float volume)
    {
        effectSource.PlayOneShot(clip, volume);
    }

    public void PlayLevelSelectEffect()
    {
        effectSource.PlayOneShot(levelSelect, 1);
    }

    public void PlayBossFightMusic()
    {
        musicSource.clip = bossFightMusic;
        musicSource.Play();
    }

    public void PlayMainMenuMusic()
    {
        musicSource.clip = mainMenuMusic;
        musicSource.Play();
    }

    public void PlayRandomMusic()
    {
        int index = Random.Range(0, backGroundMusic.Length);
        musicSource.clip = backGroundMusic[index];
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

}
