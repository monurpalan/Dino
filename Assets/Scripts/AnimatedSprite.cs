using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float baseAnimationSpeed = 1f;

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private const string ANIMATE_METHOD = "Animate";

    private void Awake()
    {
        InitializeComponents();
    }

    private void OnEnable()
    {
        StartAnimation();
    }

    private void OnDisable()
    {
        StopAnimation();
    }

    private void InitializeComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ValidateSprites();
    }

    private void ValidateSprites()
    {
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogWarning("No sprites assigned to AnimatedSprite component!");
        }
    }

    private void StartAnimation()
    {
        currentFrame = 0;
        Invoke(ANIMATE_METHOD, 0f);
    }

    private void StopAnimation()
    {
        CancelInvoke(ANIMATE_METHOD);
    }

    public void Animate()
    {
        if (sprites == null || sprites.Length == 0) return;

        UpdateFrame();
        UpdateSprite();
        ScheduleNextFrame();
    }

    private void UpdateFrame()
    {
        currentFrame = (currentFrame + 1) % sprites.Length; // Bir sonraki frame'e geçer, dizi sonuna gelirse başa döner
    }

    private void UpdateSprite()
    {
        if (IsValidFrame())
        {
            spriteRenderer.sprite = sprites[currentFrame]; // Mevcut frame'deki sprite'i SpriteRenderer'a uygular
        }
    }

    private bool IsValidFrame()
    {
        return currentFrame >= 0 && currentFrame < sprites.Length; // Frame index'inin geçerli olup olmadığını kontrol eder
    }

    private void ScheduleNextFrame()
    {
        // Animasyon hızı, temel hızın oyun hızına bölünmesiyle dinamik olarak ayarlanır
        float animationSpeed = baseAnimationSpeed / GameManager.instance.gameSpeed;
        Invoke(ANIMATE_METHOD, animationSpeed);
    }
}