using UnityEngine;

public class TerminalPuzzle : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    [Header("Sprites")]
    public Sprite offSprite;
    public Sprite onSprite;

    [Header("Platform")]
    public MovingPlatform puzzlePlatform;

    public enum ProjectileType { Red, Blue }

    [Header("Required Projectile")]
    public ProjectileType requiredProjectile;

    private bool activated = false;

    void Start()
    {
        activated = false;
        spriteRenderer.sprite = offSprite;
    }

    public void TryActivate(ProjectileType projectileType)
    {
        if (projectileType != requiredProjectile) return;

        activated = !activated;

        spriteRenderer.sprite = activated ? onSprite : offSprite;

        if (puzzlePlatform != null)
        {
            puzzlePlatform.TogglePlatform(activated);
        }
    }
}