using UnityEngine;

public class MainGameController : MonoBehaviour
{
    public AudioClip music;

    private void Start()
    {
        AudioManager.Instance.SetAudiobutton();
        AudioManager.Instance.PlayMusic(music);
    }

    public void PlayGame()
    {
        SceneController.Instance.LoadNewScene(1);
    }

    public void Mute()
    {
        AudioManager.Instance.MuteMusic();
    }
}
