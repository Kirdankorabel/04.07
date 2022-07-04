using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIScripts
{
    public class WinPanel : MonoBehaviour
    {
        [SerializeField] Button exitToLabButton;

        private void Awake()
        {
            GameUIController.WinPanel = this;
            gameObject.SetActive(false);
        }
        private void Start()
        {
            exitToLabButton.onClick.AddListener(ExitToLab);
        }

        private void ExitToLab()
        {
            StartCoroutine(Waiter(() => SceneManager.LoadScene("Scene1")));
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
