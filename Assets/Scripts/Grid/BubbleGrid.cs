using System;
using UnityEngine;
using UnityEngine.WSA;

namespace DefaultNamespace.Grid
{
    public class BubbleGrid : MonoBehaviour
    {
        [Header("Tile Grid")]
        [SerializeField] private Tile _tilePrefab;

        [SerializeField] private Bubble _bubblePrefab;
        private Bubble[,] _bubbles;
        private TileGrid _tileGrid;
        private void Awake()
        {
            _tileGrid = new TileGrid(_tilePrefab);
            _tileGrid.GenerateAll(transform);

            BallSpawn();
        }

        private void BallSpawn()
        {
            _bubbles = new Bubble[_tileGrid.TileCountX,_tileGrid.TileCountY];
            for (int x = 0; x < _bubbles.GetLength(0); x++)
            {
                for (int y = 0; y < _bubbles.GetLength(1); y++)
                {
                    Bubble bubble = Instantiate(_bubblePrefab, new Vector2(x, y), Quaternion.identity, transform);
                    bubble.Initialize();
                    bubble.GetRigidbody().bodyType = RigidbodyType2D.Static;
                    _bubbles[x, y] = bubble;
                }
            }
        }
    }
}