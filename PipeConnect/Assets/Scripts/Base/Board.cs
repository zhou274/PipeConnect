// /*
// Created by Darsan
// */

using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

namespace Base
{
    public class Board : MonoBehaviour
    {
        [SerializeField] protected GridTile gridTilePrefab;
        [SerializeField] protected float width;
        [SerializeField] protected float spacing;
        [SerializeField] protected Path pathPrefab;

        public float TileSize { get; private set; }
        public int GridSize { get; private set; }

        private readonly List<GridTile> _tiles = new List<GridTile>();
        private GridTile _lastGridTile;

        public GridTile this[Vector2Int vec] => _tiles[vec.y * GridSize + vec.x];

        public IEnumerable<GridTile> Tiles => _tiles;
        
        public bool Dragging { get; private set; }

   

        public void SetUp(int count)
        {
            GridSize = count;
            TileSize = (width - (count - 1) * spacing) / count;

            var lowerLeft = transform.position - new Vector3(1, 1, 0) * width / 2;

            for (var i = 0; i < count; i++)
            {
                for (var j = 0; j < count; j++)
                {
                    var gridTile = Instantiate(gridTilePrefab, transform);
                    gridTile.Coordinate = new Vector2Int(j, i);
                    gridTile.Size = TileSize;
                    gridTile.transform.position = lowerLeft + ((i + 0.5f) * TileSize + i * spacing) * Vector3.up +
                                                  ((j + 0.5f) * TileSize + j * spacing) * Vector3.right;
                    _tiles.Add(gridTile);
                }
            }
        }

        protected virtual void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var col = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (col != null && col.CompareTag("GridTile"))
                {
                    var gridTile = col.GetComponent<GridTile>();
                    OnDragStart(gridTile);
                    Dragging = true;
                    _lastGridTile = gridTile;
                }
            }

            if (Dragging)
            {
                var col = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (col != null && col.CompareTag("GridTile"))
                {
                    var gridTile = col.GetComponent<GridTile>();

                    if (gridTile != _lastGridTile)
                    {
                        OnDragChangedGridTile(gridTile);
                        _lastGridTile = gridTile;
                    }
                }
            }

            if (Dragging && Input.GetMouseButtonUp(0))
            {
                Dragging = false;
                OnDragEnd();
            }
        }

        protected virtual void OnDragChangedGridTile(GridTile gridTile)
        {
            
        }

        protected virtual void OnDragEnd()
        {
        }

        protected virtual void OnDragStart(GridTile gridTile)
        {

        }

        public virtual void Clear()
        {
            _tiles.ForEach(tile => Destroy(tile.gameObject));
            _tiles.Clear();
        }
    }
}