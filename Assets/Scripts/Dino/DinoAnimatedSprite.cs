using UnityEngine;

public class DinoAnimatedSprite : MonoBehaviour
{
    public Sprite[] sprites;

    private SpriteRenderer srend;
    private int frame;

    private void Awake()
    {
        srend = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Invoke(nameof(Animate), 0f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Animate()
    {
        frame++;

        if (frame >= sprites.Length)
        {
            frame = 0;
        }
        
        if (frame >= 0 && frame < sprites.Length)
        {
            srend.sprite = sprites[frame];
        }

        Invoke(nameof(Animate), 1f / DinoGameController.Instance.gameSpeed);
    }
}
