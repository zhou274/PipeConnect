using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private InputField _gridIF;
        [SerializeField] private Text _lvlTxt;
        [SerializeField] private Board _board;
        [SerializeField] private Dropdown _categoryDropDown;
        [SerializeField] private Button _loadBtn, _saveBtn,_gridBtn;
        

        private void Awake()
        {
            _categoryDropDown.options = LevelGroups.Default.Select(group => new Dropdown.OptionData(group.id)).ToList();
            _loadBtn.onClick.AddListener(LoadLevel);
            _saveBtn.onClick.AddListener(SaveLevel);
            _gridBtn.onClick.AddListener(SetUpGrid);
        }

        private void Update()
        {
            _loadBtn.interactable = (int.TryParse(_lvlTxt.text, out var targetLevel) &&
                                     LevelGroups.Default.ElementAt(_categoryDropDown.value)
                                         .Any(level => level.no == targetLevel));

            _saveBtn.interactable = int.TryParse(_lvlTxt.text, out var lvl) && lvl > 0;
            _gridBtn.interactable = int.TryParse(_gridIF.text, out var grid) && grid > 1;
        }

        public void SetUpGrid()
        {
            if (!int.TryParse(_gridIF.text,out var value))
            {
                return;
            }

            _board.Clear();
            _board.SetUp(value);

        }

        public void LoadLevel()
        {
            if (!int.TryParse(_lvlTxt.text,out var value))
            {
                return;
            }

            var level = LevelGroups.Default.ElementAt(_categoryDropDown.value).FirstOrDefault(l => l.no == value);

            if(level.paths==null)
                return;


            _gridIF.text = level.grid.ToString();
            SetUpGrid();

            level.paths.ForEach(data => _board.AddPath(data.points,data.color));
        }

        public void Clear()
        {
            _board.Clear();
        }

        public void SaveLevel()
        {
            if (!int.TryParse(_lvlTxt.text, out var value))
            {
                return;
            }

            if(value<=0)
                return;

            var levelGroup = LevelGroups.Default.ElementAt(_categoryDropDown.value);
            levelGroup.levels.RemoveAll(l => l.no == value);
            levelGroup.levels.Add(new Level
            {
                no = value,
                grid = _board.GridSize,
                paths = _board.CompletedPaths.Select(path => new PathData
                {
                    color = path.GridTiles.First().Color,
                    points = path.GridTiles.Select(tile => tile.Coordinate).ToList()
                }).ToList()
            });
            LevelGroups.Default.Save();

        }
    }
}