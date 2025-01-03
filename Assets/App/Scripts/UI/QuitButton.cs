using App.Quitting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class QuitButton : MonoBehaviour
    {
        [Inject] private IQuitInvoker _quitInvoker;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Quit);
        }

        private void Quit() 
            => _quitInvoker.Quit();
    }
}