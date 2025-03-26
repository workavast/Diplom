using UnityEngine;
using UnityEngine.UI;

namespace App.UI.WindowsSwitching
{
    [RequireComponent(typeof(Button))]
    public class WindowSwitchBtn : MonoBehaviour
    {
        [SerializeField] private WindowsSwitcher windowsSwitcher;
        [SerializeField] private string key;
        
        protected virtual void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(() => windowsSwitcher.SwitchWindow(key));
        }
    }
}