using System.Collections;
using TMPro;
using UnityEngine;

public class MemoryGameController : MonoBehaviour
{
    const int numberOfCards = 10;

    public static MemoryGameController Instance { get; private set; }

    public Vector3[] spritePos;
    public int numberOfPairs = numberOfCards / 2;
    public bool canRotate, first;

    public GameObject card;
    public Sprite[] faceSprites;
    public Sprite backSprite;

    public GameObject gameOver, howToPlay;
    public TextMeshProUGUI attemptsText, finishAttemptsText;

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

        first = true;
        HowToPlay();
        NewGame();    
    }

    public void NewGame()
    {
        MemoryCardController[] cards = FindObjectsOfType<MemoryCardController>();

        foreach (var card in cards)
        {
            Destroy(card.gameObject);
        }

        int posLimit = spritePos.Length;
        int spriteLimit = faceSprites.Length;

        for (int i = 0; i < numberOfPairs; ++i)
        {
            int index = Random.Range(0, posLimit);
            Vector3 cardSpawnPos = spritePos[index];

            // Swap places to avoid same position
            Vector3 auxVector = spritePos[--posLimit];
            spritePos[posLimit] = spritePos[index];
            spritePos[index] = auxVector;

            index = Random.Range(0, posLimit);
            Vector3 cardSpawnPos2 = spritePos[index];

            // Swap places to avoid same position
            auxVector = spritePos[--posLimit];
            spritePos[posLimit] = spritePos[index];
            spritePos[index] = auxVector;
            ///////////////////////
            int spriteIndex = Random.Range(0, spriteLimit);
            
            GameObject spawnedCard1 = Instantiate(card, cardSpawnPos, Quaternion.identity, null);
            GameObject spawnedCard2 = Instantiate(card, cardSpawnPos2, Quaternion.identity, null);
            MemoryCardController card1 = spawnedCard1.GetComponent<MemoryCardController>();
            MemoryCardController card2 = spawnedCard2.GetComponent<MemoryCardController>();

            card1.id = card2.id = faceSprites[spriteIndex].name;
            card1.backSprite = card2.backSprite = backSprite;
            card1.faceSprite = card2.faceSprite = faceSprites[spriteIndex];

            // Swap places to avoid same sprite
            Sprite auxSprite = faceSprites[--spriteLimit];
            faceSprites[spriteLimit] = faceSprites[spriteIndex];
            faceSprites[spriteIndex] = auxSprite;
        }

        gameOver.SetActive(false);
        attemptsText.text = "Jogadas: 0";

        if (!first) Invoke(nameof(ShowCards), 0f);
    }

    private void ShowCards()
    {
        StartCoroutine(IEShowCards());
    }

    private IEnumerator IEShowCards()
    {
        canRotate = false;

        MemoryCardController[] cards = FindObjectsOfType<MemoryCardController>();

        foreach (var card in cards)
        {
            card.GetComponent<SpriteRenderer>().sprite = card.faceSprite;
            card.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        yield return new WaitForSeconds(2f);
        foreach (var card in cards)
        {
            card.GetComponent<SpriteRenderer>().sprite = card.backSprite;
            card.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        canRotate = true;
    }

    public void HowToPlay()
    {
        canRotate = false;
        howToPlay.SetActive(true);
    }

    public void Resume()
    {
        howToPlay.SetActive(false);
        canRotate = true;

        if (first)
        {
            ShowCards();
            first = false;
        }
    }

    public void ExitGame()
    {
        SceneController.Instance.LoadNewScene(1);
    }

    public void Finish()
    {
        Invoke(nameof(GameOver), 0.5f);
    }

    void GameOver()
    {
        gameOver.SetActive(true);
    }

    public void Mute()
    {
        AudioManager.Instance.MuteMusic();
    }
}
