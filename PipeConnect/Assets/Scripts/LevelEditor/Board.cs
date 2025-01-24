using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

namespace LevelEditor
{
    public class Board : Base.Board
    {
        private readonly List<Path> _completedPaths = new List<Path>();

        public IEnumerable<Path> CompletedPaths => _completedPaths;
        public Path CurrentPath { get; set; }



        protected override void OnDragChangedGridTile(GridTile gridTile)
        {

            foreach (var p in _completedPaths.Where(path => path.GridTiles.Any(tile => tile == gridTile)).ToList())
            {
                _completedPaths.Remove(p);
                Destroy(p.gameObject);
            }

            var gridTiles = CurrentPath.GridTiles.ToList();
            if (gridTiles.Count > 0)
            {
                if ((gridTiles.Last().Coordinate - gridTile.Coordinate).magnitude > 1.1f)
                {
                    return;
                }
            }

            var index = gridTiles.IndexOf(gridTile);
            CurrentPath.GridTiles = index == -1 ? gridTiles.Append(gridTile) : gridTiles.Take(index + 1);
        }

        protected override void OnDragEnd()
        {
            if (CurrentPath.GridTiles.Count() < 3)
            {
                Destroy(CurrentPath.gameObject);
                CurrentPath = null;
                return;
            }

            CurrentPath.Closed = true;
            _completedPaths.Add(CurrentPath);
            CurrentPath = null;
        }

        protected override void OnDragStart(GridTile gridTile)
        {
            foreach (var p in _completedPaths.Where(pth => pth.GridTiles.Any(tile => tile == gridTile)).ToList())
            {
                _completedPaths.Remove(p);
                Destroy(p.gameObject);
            }

            gridTile.Color = Enumerable.Range(0, ColorGroup.Default.Count())
                .FirstOrDefault(i => _completedPaths.All(p => p.GridTiles.First().Color != i)); 
            var path = Instantiate(pathPrefab);
            
            path.Size = TileSize + spacing;
            path.GridTiles = new[] { gridTile };
            CurrentPath = path;
        }

        public override void Clear()
        {
            base.Clear();
            _completedPaths.ForEach(path => Destroy(path.gameObject));
            _completedPaths.Clear();
        }

        public void AddPath(IEnumerable<Vector2Int> points, int color)
        {
            var list = points.ToList();
            this[list[0]].Color =color;
            var path = Instantiate(pathPrefab);
            path.Size = TileSize + spacing;
            path.GridTiles = list.Select(vec => this[vec]);
            
            path.Closed = true;
            _completedPaths.Add(path);
        }
    }
}