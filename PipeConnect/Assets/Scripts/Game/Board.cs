// /*
// Created by Darsan
// */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Board : Base.Board
    {

        [SerializeField] private AudioClip _closePathClip;
        [SerializeField] private AudioClip _breakPathClip;
        public event Action Completed; 

        private readonly List<Path> _completedPaths = new List<Path>();


        public Path CurrentPath { get; set; }

        public bool IsCompleted { get; private set; }

        private readonly Stack<UndoCommand> _undoStack = new Stack<UndoCommand>();
        private readonly List<GridTile[]> _currentActionRemovedPaths = new List<GridTile[]>();
        public bool HasUndo => _undoStack.Count > 0;

        public void SetUp(int grid, IEnumerable<PathData> datas)
        {
            SetUp(grid);
            foreach (var data in datas)
            {
                this[data.points.First()].HasEnd = true;
                this[data.points.Last()].HasEnd = true;
                this[data.points.First()].Color = data.color;
                this[data.points.Last()].Color = data.color;
            }
            
        }


        // ReSharper disable Unity.PerformanceAnalysis
        protected override void OnDragChangedGridTile(GridTile gridTile)
        {
            if (CurrentPath == null || (gridTile.HasEnd && gridTile.Color != CurrentPath.GridTiles.First().Color))
                return;


            

            foreach (var p in _completedPaths.Where(path => path.GridTiles.Any(tile => tile == gridTile)).ToList())
            {
                
                RemovePath(p);
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
            if (index == -1 && !(gridTiles.Count>1 && CurrentPath.Closed))
                CurrentPath.GridTiles = gridTiles.Append(gridTile);
            else if(index!=-1)
                CurrentPath.GridTiles = gridTiles.Take(index + 1);

            var closed = CurrentPath.GridTiles.Count() > 1 && CurrentPath.GridTiles.Last().HasEnd;
            if (!CurrentPath.Closed && closed)
            {
                PlayClipIfCan(_closePathClip);
            }

            CurrentPath.Closed = closed;
        }

        public void Undo()
        {
            var command = _undoStack.Pop();

            if (command.CreatedPath!=null)
            {
                var path = _completedPaths.First((item)=>item.GridTiles.First() == command.CreatedPath.First() || item.GridTiles.First()== command.CreatedPath.Last());
                _completedPaths.Remove(path);
                Destroy(path.gameObject);
            }

            command.DestroyedPaths?.ForEach((path) =>
            {
                var p = Instantiate(pathPrefab);
                p.Size = TileSize + spacing;
                p.GridTiles = path;
                p.Closed = true;
                _completedPaths.Add(p);
            });
        }
        
        private void RemovePath(Path path)
        {
            _currentActionRemovedPaths.Add(path.GridTiles.ToArray());
            PlayClipIfCan(_breakPathClip);
            _completedPaths.Remove(path);
            Destroy(path.gameObject);
        }
        
        
        private void PlayClipIfCan(AudioClip clip,float volume=0.35f)
        {
            if(!AudioManager.IsSoundEnable || clip==null)
                return;
            AudioSource.PlayClipAtPoint(clip,Camera.main.transform.position,volume);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        protected override void OnDragEnd()
        {
            if(IsCompleted)
                return;
            if(CurrentPath==null)
                return;
            if(CurrentPath.Closed)
            {
                
                _completedPaths.Add(CurrentPath);
                _undoStack.Push(new UndoCommand
                {
                    CreatedPath = CurrentPath.GridTiles.ToArray(),
                    DestroyedPaths = _currentActionRemovedPaths.Count!=0 ? _currentActionRemovedPaths.ToList():null
                });
            }
            else
            {
                Destroy(CurrentPath.gameObject);

                if (_currentActionRemovedPaths.Count > 0)
                {
                    _undoStack.Push(new UndoCommand
                    {
                        DestroyedPaths = _currentActionRemovedPaths.ToList()
                    });
                }
            }

          
            CurrentPath = null;
_currentActionRemovedPaths.Clear();
            if (_completedPaths.Count == Tiles.Where(tile => tile.HasEnd).GroupBy(tile => tile.Color).Count() &&
                _completedPaths.Sum(path => path.GridTiles.Count()) == GridSize * GridSize)
            {
                IsCompleted = true;
                Completed?.Invoke();
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        protected override void OnDragStart(GridTile gridTile)
        {
            if (IsCompleted)
                return;
            if (!gridTile.HasEnd)
                return;
            foreach (var p in _completedPaths.Where(pth => pth.GridTiles.Any(tile => tile == gridTile)).ToList())
            {
               RemovePath(p);
            }
            var path = Instantiate(pathPrefab);
            path.Size = TileSize + spacing;
            path.GridTiles = new[] { gridTile };
            CurrentPath = path;
        }

        public struct UndoCommand
        {
            public GridTile[] CreatedPath { get; set; }
            public new List<GridTile[]> DestroyedPaths
            { get; set; }
        }
    }
}