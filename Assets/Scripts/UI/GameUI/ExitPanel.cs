using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace UIScripts
{
    public class ExitPanel : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button exitButton;

        private void Awake()
        {
            GameUIController.ExitPanel = this;
            gameObject.SetActive(false);
        }

        private void Start()
        {
            closeButton.onClick.AddListener(() => StartCoroutine(Waiter(() => this.gameObject.SetActive(false))));
            exitButton.onClick.AddListener(() => StartCoroutine(Waiter(() => SceneManager.LoadScene("Scene1"))));
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
