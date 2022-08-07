using UnityEngine;

namespace DefaultNamespace.Grid
{
    public class TileGrid
    {
        private Tile[,] _tiles;
        private Vector2[,] _tilesPosition;
        private Camera _camera;

        private readonly Tile _tilePrefab;
        private readonly int _tileCountX;
        private readonly int _tileCountY;

        private Vector3 _bounds;
        private float _boundsByX;
        private float _boundsByY;

        public int TileCountX => _tileCountX;
        public int TileCountY => _tileCountY;
        public TileGrid(Tile tilePrefab)
        {
            _tilePrefab = tilePrefab;
            _camera = Camera.main;
            
            _boundsByX = _camera.orthographicSize * _camera.aspect - _tilePrefab.GetSpriteRenderer().bounds.extents.x;
            _boundsByY = _camera.orthographicSize - _tilePrefab.GetSpriteRenderer().bounds.extents.y;

            _tileCountX = (int) (_boundsByX / _tilePrefab.GetSpriteRenderer().bounds.extents.x);
            _tileCountY = (int) (_boundsByY/ 2);
            
            _camera.transform.position = new Vector3(_tileCountX / 2 - 0.5f, 
                _tileCountY / 2 - _boundsByY + _tilePrefab.GetSpriteRenderer().bounds.extents.y * 2,
                -10);
        }

        public void GenerateAll(Transform container)
        {
            _tiles = new Tile[_tileCountX,_tileCountY];
            
            for (int x = 0; x < _tileCountX; x++)
            {
                for (int y = 0; y < _tileCountY; y++)
                {
                    _tiles[x,y] = GenerateSingle(new Vector2(x,y),container);
                }
            }
        }

        private Tile GenerateSingle(Vector2 tileSpawn,Transform container) => 
            GameObject.Instantiate(_tilePrefab, tileSpawn, Quaternion.identity, container);
    }
}