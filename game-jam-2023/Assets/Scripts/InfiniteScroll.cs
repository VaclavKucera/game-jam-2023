using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class InfiniteScroll : MonoBehaviour
{
    // Expose the scrollVector and animationSpeed fields
    public Vector2 scrollVector = Vector2.right;
    public float animationSpeed = 1.0f;

    // Reference to Sprite Renderer component
    private SpriteRenderer spriteRenderer;

    // Variables to store tile size and accumulated scroll distance
    private Vector2 tileSize;
    private Vector2 accumulatedScroll;

    // Initialize the Sprite Renderer reference and calculate tile size
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tileSize = spriteRenderer.sprite.bounds.size;
    }

    // Move the GameObject along the scrollVector and "jump back" one tile when needed
    void Update()
    {
        // Calculate the distance to move
        Vector2 moveDistance = scrollVector * animationSpeed * Time.deltaTime;

        // Update the position and accumulated scroll distance
        transform.position += (Vector3)moveDistance;
        accumulatedScroll += moveDistance;

        // Check if we need to "jump back" one tile
        if (Mathf.Abs(accumulatedScroll.x) >= tileSize.x)
        {
            transform.position -= new Vector3(Mathf.Sign(scrollVector.x) * tileSize.x, 0, 0);
            accumulatedScroll.x = 0f;
        }

        if (Mathf.Abs(accumulatedScroll.y) >= tileSize.y)
        {
            transform.position -= new Vector3(0, Mathf.Sign(scrollVector.y) * tileSize.y, 0);
            accumulatedScroll.y = 0f;
        }
    }
}