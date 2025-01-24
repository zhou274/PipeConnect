using System.Collections;
using MyGame;
using UnityEngine;

namespace Game
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [SerializeField] private LevelCompletePanel _levelCompletePanel;
        [SerializeField] private GameObject _winEffect;




        private void Awake()
        {
            Instance = this;
        }

        
        private void OnEnable()
        {
            LevelManager.LevelCompleted +=LevelManagerOnLevelCompleted;
        }


        private void OnDisable()
        {
            LevelManager.LevelCompleted -= LevelManagerOnLevelCompleted;
        }

        private void LevelManagerOnLevelCompleted()
        {
            StartCoroutine(LevelCompletedEnumerator());
        }

        private IEnumerator LevelCompletedEnumerator()
        {
            yield return new WaitForSeconds(0.2f);
            var point = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f)).WithZ(0);
            Instantiate(_winEffect, point, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
            _levelCompletePanel.Show();
        }


        public void LoadNextLevel()
        {
            var levelGroup = LevelManager.Instance.LevelGroup;
            var levelNo = LevelManager.Instance.Level.no;
            if (!ResourceManager.HasLevel(levelGroup.id, levelNo + 1))
            {
                SharedUIManager.PopUpPanel.ShowAsInfo("Congratulations!", "You have successfully Completed this Game Mode",
                    () =>
                    {
                        GameManager.LoadScene("MainMenu");
                    });
                return;
            }

            GameManager.LoadGame(new LoadGameData
            {
                Level = ResourceManager.GetLevel(levelGroup.id, levelNo + 1),
                LevelGroup = levelGroup
            });
        }
    }
}