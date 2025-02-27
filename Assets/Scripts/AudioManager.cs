using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private Sprite audioOn, audioOff;
    private Image audioButton;
    private int muted;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void SetAudiobutton()
    {
        audioButton = GameObject.FindGameObjectWithTag("AudioButton").GetComponent<Image>();

        muted = PlayerPrefs.GetInt("muted", 0);
        if (muted == 1)
        {
            musicSource.mute = true;
            audioButton.sprite = audioOff;
        }
    }

    public void PlayMusic(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.Play();
    }

    public void MuteMusic()
    {
        musicSource.mute = !musicSource.mute;

        muted = muted == 1 ? 0 : 1;
        audioButton.sprite = muted == 1 ? audioOff : audioOn;
        PlayerPrefs.SetInt("muted", muted);
    }
}
