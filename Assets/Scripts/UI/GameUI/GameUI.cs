using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private Button exitButton;

        private void Start()
        {
            //exitButton.onClick.AddListener(() => StartCoroutine(Waiter(() => GameUIController.Exit())));
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
