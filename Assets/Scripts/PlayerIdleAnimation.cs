using UnityEngine;

public class PlayerIdleAnimation : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sprites;
    private SpriteRenderer srend;
    private int frame;

    private void Awake()
    {
        srend = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Invoke(nameof(Animate), 0f);
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

        Invoke(nameof(Animate), 0.2f);
    }
}
