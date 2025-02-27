using TMPro;
using UnityEngine;

public class BoxesGameController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer placeSpriteRenderer;

    public static BoxesGameController Instance { get; private set; }
    public int numberOfShapes;
    public bool canClick;
    public Vector3[] shapePos, placePos;
    public Sprite[] shapesSprites, placesSprites;
    public GameObject shape, place;

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

        spriteRenderer = shape.GetComponent<SpriteRenderer>();
        placeSpriteRenderer = place.GetComponent<SpriteRenderer>();
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

        HowToPlay();
        NewGame();  
    }

    public void NewGame()
    {
        BoxesShapeController[] shapes = FindObjectsOfType<BoxesShapeController>();
        GameObject[] places = GameObject.FindGameObjectsWithTag("ShapePlace");

        for (int i = 0; i < shapes.Length; ++i)
        {
            Destroy(shapes[i].gameObject);
            Destroy(places[i]);
        }

        int posLimit = shapePos.Length;
        int spriteLimit = shapesSprites.Length;

        for (int i = 0; i < numberOfShapes; ++i)
        {
            int index = Random.Range(0, posLimit);
            Vector3 shapeSpawnPos = shapePos[index];
            int index2 = Random.Range(0, posLimit);
            Vector3 placeSpawnPos = placePos[index2];

            // Swap places to avoid same position
            Vector3 auxVector = shapePos[--posLimit];
            shapePos[posLimit] = shapePos[index];
            shapePos[index] = auxVector;

            auxVector = placePos[posLimit];
            placePos[posLimit] = placePos[index2];
            placePos[index2] = auxVector;

            int spriteIndex = Random.Range(0, spriteLimit);
            spriteRenderer.sprite = shapesSprites[spriteIndex];
            placeSpriteRenderer.sprite = placesSprites[spriteIndex];

            // Swap places to avoid same sprite
            Sprite auxSprite = shapesSprites[--spriteLimit];
            shapesSprites[spriteLimit] = shapesSprites[spriteIndex];
            shapesSprites[spriteIndex] = auxSprite;

            auxSprite = placesSprites[spriteLimit];
            placesSprites[spriteLimit] = placesSprites[spriteIndex];
            placesSprites[spriteIndex] = auxSprite;

            GameObject spawnedShape = Instantiate(shape, shapeSpawnPos, Quaternion.identity);
            GameObject spawnedPlace = Instantiate(place, placeSpawnPos, Quaternion.identity);

            BoxesShapeController shapeTr = spawnedShape.GetComponent<BoxesShapeController>();
            shapeTr.shapePlace = spawnedPlace.transform;
        }

        gameOver.SetActive(false);
        attemptsText.text = "Jogadas: 0";
    }

    public void HowToPlay()
    {
        canClick = false;
        howToPlay.SetActive(true);
    }

    public void Resume()
    {
        howToPlay.SetActive(false);
        canClick = true;
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
