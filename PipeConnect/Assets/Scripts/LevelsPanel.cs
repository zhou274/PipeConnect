﻿// /*
// Created by Darsan
// */

using System.Collections.Generic;
using System.Linq;
using MyGame;
using UnityEngine;

public class LevelsPanel : ShowHidable
{
    [SerializeField] private LevelTileUI _levelTileUIPrefab;
    [SerializeField] private RectTransform _content;

    public LevelGroup LevelGroup
    {
        get => _levelGroup;
        set
        {
            _levelGroup = value;

           

            for (var i = 0; i < value.levels.Count; i++)
            {
                var level = value.levels[i];
                if (_tiles.Count<=i)
                {
                    var levelTileUI = Instantiate(_levelTileUIPrefab,_content);
                    levelTileUI.Clicked +=LevelTileUIOnClicked;
                    _tiles.Add(levelTileUI);
                }

                _tiles[i].MViewModel = new LevelTileUI.ViewModel
                {
                    Level = level,
                    Locked = ResourceManager.IsLevelLocked(value.id,level.no),
                    Completed = ResourceManager.GetCompletedLevel(value.id)>=level.no
                };
            }

        }
    }



    private readonly List<LevelTileUI> _tiles = new List<LevelTileUI>();
    private LevelGroup _levelGroup;


    private void LevelTileUIOnClicked(LevelTileUI tileUI)
    {
        if (tileUI.MViewModel.Locked)
        {
            return;
        }

        GameManager.LoadGame(new LoadGameData
        {
            Level = tileUI.MViewModel.Level,
            LevelGroup = LevelGroup
        });
    }
}