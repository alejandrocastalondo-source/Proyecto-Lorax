using UnityEngine;

public class PuzzleObject : MonoBehaviour
{
    public bool isOn = true;

    public SpriteRenderer spriteRenderer;
    public Sprite onSprite;
    public Sprite offSprite;

    public Animator animator;
    public Vector3 customScale = Vector3.one;

    void Start()
    {
        SetState(false);
    }

    public void SetState(bool state)
    {
        isOn = state;
        transform.localScale = isOn ? customScale : Vector3.one;
        spriteRenderer.sprite = isOn ? onSprite : offSprite;

        if (animator != null)
            animator.enabled = isOn;
    }
}