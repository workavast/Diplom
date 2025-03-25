using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.UI.Selection
{
    public class SelectionBtn<TId> : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpText;
        
        private Button _button;
        private TId _id;
        
        public event Action<TId> OnClick;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => OnClick?.Invoke(_id));
        }

        public void SetData(TId id, string textField)
        {
            _id = id;
            tmpText.text = textField;
        }
    }
}