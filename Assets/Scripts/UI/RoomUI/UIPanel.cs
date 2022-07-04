using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class UIPanel : MonoBehaviour
    {
        [SerializeField] private Button startNewGameButton;
        [SerializeField] private Button loadGameButton;
        [SerializeField] private Button exitButton;

        private void Start()
        {
            UIController.uIPanel = this;
            if (!UIController.isStart)
            {
                this.gameObject.SetActive(false);
                GameInfo.room.UpdateRoomInfo();
            }
            UIController.isStart = false;

            startNewGameButton.onClick.AddListener(StartNewGame);
            loadGameButton.onClick.AddListener(LoadGame);
            exitButton.onClick.AddListener(Exit);
        }

        public void StartNewGame()
        {
            if (PlayerPrefs.HasKey(GameInfo.key))
                PlayerPrefs.DeleteKey(GameInfo.key);
            GameInfo.room.UpdateRoomInfo();
            StartCoroutine(Waiter(() => this.gameObject.SetActive(false)));
        }

        public void LoadGame()
        {
            if (!PlayerPrefs.HasKey(GameInfo.key))
            {
                Debug.Log("no saved game");
                //UIController.ShowError("no saved game");
            }
            else
            {
                GameInfo.room.UpdateRoomInfo();
                this.gameObject.SetActive(false);
            }
        }

        public void Exit()
        {
            Application.Quit();
            Serializator.Serialize(GameInfo.key, GameInfo.room.roomState);
        }

        private IEnumerator Waiter(Action action)
        {
            while (true)
            {
                yield return new WaitForSeconds(0.6f);
                action.Invoke();
            }
        }
    }
}
