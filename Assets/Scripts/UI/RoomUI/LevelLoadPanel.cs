using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIScripts
{
    public class LevelLoadPanel : MonoBehaviour
    {
        [SerializeField] private Button startLevelButton;
        [SerializeField] private Button exitButton;
        public event Action close;

        private void Awake()
        {
            UIController.levelLoadPanel = this;
            this.gameObject.SetActive(false);
        }

        private void Start()
        {
            startLevelButton.onClick.AddListener(() =>
            {
                close?.Invoke();
                StartCoroutine(Waiter(() => SceneManager.LoadScene("Scene2")));
            });
            exitButton.onClick.AddListener(() =>
            {
                close?.Invoke();
                StartCoroutine(Waiter(() => this.gameObject.SetActive(false)));
            });
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
