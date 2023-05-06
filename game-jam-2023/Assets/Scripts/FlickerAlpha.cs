using UnityEngine;

public class FlickerSprite : MonoBehaviour
{
    // Expose the minAlpha, maxAlpha and updateFrequency fields
    public float minAlpha = 0.2f;
    public float maxAlpha = 1.0f;
    public float updateFrequency = 1f;

    // Reference to the Sprite Renderer component
    private SpriteRenderer spriteRenderer;

    // Variables for smooth transition
    private float currentAlpha;
    private float targetAlpha;
    private float transitionStartTime;

    // Initialize the Sprite Renderer reference
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentAlpha = spriteRenderer.color.a;
        targetAlpha = Random.Range(minAlpha, maxAlpha);
        transitionStartTime = Time.time;
    }

    // Update the alpha value of the sprite renderer smoothly and with a configurable frequency
    void Update()
    {
        // Calculate the progress of the transition
        float progress = (Time.time - transitionStartTime) * updateFrequency;

        // Smoothly transitioning between alpha values
        currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, progress);
        Color newColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, currentAlpha);
        spriteRenderer.color = newColor;

        // Update the target alpha value based on updateFrequency
        if (Mathf.Approximately(currentAlpha, targetAlpha))
        {
            targetAlpha = Random.Range(minAlpha, maxAlpha);
            transitionStartTime = Time.time;
        }
    }
}