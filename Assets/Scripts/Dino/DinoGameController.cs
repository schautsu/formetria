using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DinoGameController : MonoBehaviour
{
    public static DinoGameController Instance { get; private set; }

    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.05f;
    public float gameSpeed { get; private set; }

    private DinoPlayerController player;
    private DinoSpawner spawner;

    public GameObject gameOver, howToPlay;
    public TextMeshProUGUI finishScoreText, scoreText, hiscoreText;
    public TextMeshProUGUI idCollectNameText;
    public Image collectImg;
    public int idCollect;

    private float score;

    public AudioClip music;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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

    private void Start()
    {
        AudioManager.Instance.SetAudiobutton();
        AudioManager.Instance.PlayMusic(music);

        player = FindObjectOfType<DinoPlayerController>();
        spawner = FindObjectOfType<DinoSpawner>();

        HowToPlay();
        NewGame();
    }

    public void NewGame()
    {
        DinoObstacle[] obstacles = FindObjectsOfType<DinoObstacle>();

        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        score = 0f;
        gameSpeed = initialGameSpeed;
        enabled = true;

        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        gameOver.SetActive(false);

        UpdateHiscore();
    }

    public void GameOver()
    {
        gameSpeed = 0f;
        enabled = false;

        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
        finishScoreText.text = "Pontos: " + Mathf.FloorToInt(score).ToString("D6");
        gameOver.SetActive(true);

        UpdateHiscore();
    }

    public void HowToPlay()
    {
        Time.timeScale = 0;
        howToPlay.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        howToPlay.SetActive(false);
    }

    public void ExitGame()
    {
        UpdateHiscore();

        SceneController.Instance.LoadNewScene(1);
    }

    private void Update()
    {
        if (gameSpeed < 12f) gameSpeed += gameSpeedIncrease * Time.deltaTime;
        score += gameSpeed * Time.deltaTime;
        scoreText.text = "Pontos:\t" + Mathf.FloorToInt(score).ToString("D6");
    }

    private void UpdateHiscore()
    {
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }

        hiscoreText.text = "Recorde:\t" + Mathf.FloorToInt(hiscore).ToString("D6");
    }

    public void Mute()
    {
        AudioManager.Instance.MuteMusic();
    }
}
