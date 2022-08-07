using UnityEngine;

namespace DefaultNamespace.Grid
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public Vector2 Pos => transform.position;
        public SpriteRenderer GetSpriteRenderer() => 
            _spriteRenderer;
    }
}