// /*
// Created by Darsan
// */

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MainMenu
{
    public class CategoryTileUI : MonoBehaviour,IPointerClickHandler
    {
        public event Action<CategoryTileUI> Clicked; 

        [SerializeField] private TextMeshProUGUI _nameTxt, _progressTxt;
        private LevelGroup _levelGroup;

        public LevelGroup LevelGroup
        {
            get => _levelGroup;
            set
            {
                _levelGroup = value;
                _nameTxt.text = value.name;
                _progressTxt.text = $"{ResourceManager.GetCompletedLevel(value.id)}/{value.levels.Count}";
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke(this);
        }
    }
}