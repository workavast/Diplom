using UnityEngine;
using UnityEngine.UI;

namespace App.UI.WindowsSwitching
{
    [RequireComponent(typeof(Button))]
    public class WindowSwitchBtn : MonoBehaviour
    {
        [SerializeField] private string key;
        [SerializeField] private WindowsSwitcher windowsSwitcher;
        
        private void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(() => windowsSwitcher.SwitchWindow(key));
        }
    }
}