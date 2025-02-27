using UnityEngine;

public class LobbyGameController : MonoBehaviour
{
    public AudioClip music;

    private void Start()
    {
        AudioManager.Instance.SetAudiobutton();
        AudioManager.Instance.PlayMusic(music);
    }

    public void Mute()
    {
        AudioManager.Instance.MuteMusic();
    }
}
