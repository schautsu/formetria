using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCardController : MonoBehaviour
{
    public SpriteRenderer rend;
    public string id;
    public Sprite faceSprite, backSprite;
    public static bool coroutineAllowed;
    public bool faceUp, locked;

    private MemoryCardController firstInPair, secondInPair;

    public static Queue<MemoryCardController> sequence;
    public static int pairsFound, attempts;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        rend.sprite = backSprite;
        coroutineAllowed = true;
        faceUp = false;
        locked = false;
        sequence = new Queue<MemoryCardController>();
        pairsFound = 0;
        attempts = 0;
    }

    private void OnMouseDown()
    {
        if (MemoryGameController.Instance.canRotate && !locked && coroutineAllowed)
        {
            StartCoroutine(RotateCard());
        }
    }

    public IEnumerator RotateCard()
    {
        locked = true;
        coroutineAllowed = false;

        sequence.Enqueue(this);
        for (float i = 0f; i <= 180f; i += 10f)
        {
            transform.rotation = Quaternion.Euler(0f, i, 0f);
            if (i == 90f)
            {
                rend.sprite = faceSprite;
            }
            yield return new WaitForSeconds(0.01f);
        }

        coroutineAllowed = true;

        if (sequence.Count == 2)
        {
            attempts++;
            MemoryGameController.Instance.attemptsText.text = "Jogadas: " + attempts;
            CheckResults();
        }
    }

    private void CheckResults()
    {
        firstInPair = sequence.Dequeue();
        secondInPair = sequence.Dequeue();

        if (firstInPair.id == secondInPair.id)
        {
            ++pairsFound;
        }
        else
        {
            sequence.Clear();
            firstInPair.StartCoroutine("RotateBack");
            secondInPair.StartCoroutine("RotateBack");
        }

        if (pairsFound == MemoryGameController.Instance.numberOfPairs)
        {
            MemoryGameController.Instance.finishAttemptsText.text = "Jogadas: " + attempts;
            pairsFound = 0;
            attempts = 0;
            MemoryGameController.Instance.Finish();
        }
    }

    public IEnumerator RotateBack()
    {
        yield return new WaitForSeconds(0.2f);

        for (float i = 180f; i >= 0f; i -= 10f)
        {
            transform.rotation = Quaternion.Euler(0f, i, 0f);
            if (i == 90f)
            {
                rend.sprite = backSprite;
            }
            yield return new WaitForSeconds(0.01f);
        }
        locked = false;
    }
}
