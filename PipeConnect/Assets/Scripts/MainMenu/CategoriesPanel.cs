// /*
// Created by Darsan
// */

using System;
using UnityEngine;

namespace MainMenu
{
    public class CategoriesPanel : ShowHidable
    {
        [SerializeField] private CategoryTileUI _categoryTileUI;
        [SerializeField] private RectTransform _content;


        private void Awake()
        {
            foreach (var levelGroup in LevelGroups.Default)
            {
                var categoryTileUI = Instantiate(_categoryTileUI,_content);
                categoryTileUI.LevelGroup = levelGroup;
                categoryTileUI.Clicked +=CategoryTileUIOnClicked;
            }
        }

        private void CategoryTileUIOnClicked(CategoryTileUI tile)
        {
            var levelsPanel = UIManager.Instance.LevelsPanel;
            levelsPanel.LevelGroup = tile.LevelGroup;
            levelsPanel.Show();
        }

        public void OnClickButton(int mode)
        {
            var levelsPanel = UIManager.Instance.LevelsPanel;
            levelsPanel.Show();
        }

    }
}