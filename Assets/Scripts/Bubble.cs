using UnityEngine;

public class Bubble : MonoBehaviour
{
    private const string BUBBLE_LAYER = "Bubble";
    private const string BUBBLE_MOVE_LAYER = "BubbleMove";

    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private BubbleColor _bubbleColor;
    public Vector2 Pos => transform.position;

    public void Initialize()
    {
        _bubbleColor = new BubbleColor(_spriteRenderer);
    }

    public void Push(Vector2 force)
    {
        SetNewLayer(BUBBLE_MOVE_LAYER);
        _rigidbody2D.AddForce(force, ForceMode2D.Impulse);
    }

    public Rigidbody2D GetRigidbody() =>
        _rigidbody2D;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer(BUBBLE_LAYER))
        {
            FixPosition();
            if(col.contacts.Length > 1)
                transform.position = col.contacts[1].point;
            SetNewLayer(BUBBLE_LAYER);
        }
    }

    private void FixPosition()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
    }

    private void SetNewLayer(string newLayer)
    {
        gameObject.layer = LayerMask.NameToLayer(newLayer);
    }

}