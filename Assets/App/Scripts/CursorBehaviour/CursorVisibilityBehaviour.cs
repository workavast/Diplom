using UnityEngine;

namespace App.CursorBehaviour
{
    public class CursorVisibilityBehaviour
    {
        private int _showRequestsCount;
        
        public void Show()
        {
            _showRequestsCount++;
            CheckCursorVisibilityState();
        }

        public void Hide()
        {
            _showRequestsCount--;
            
            if (_showRequestsCount < 0)
            {
                Debug.LogWarning($"Show cursor requests count less then 0: [{_showRequestsCount}]");
                _showRequestsCount = 0;
            }
            
            CheckCursorVisibilityState();
        }

        public void CheckCursorVisibilityState()
        {
            Cursor.visible = _showRequestsCount > 0;
        }
    }
}