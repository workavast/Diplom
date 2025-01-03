using System;
using UnityEngine;


namespace Avastrad.UI.MobileSafeArea
{
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class SafeAreaAdapter: MonoBehaviour
    {
        [SerializeField] private Pair isAdaptXAnchors = new(true, true);
        [SerializeField] private Pair isAdaptYAnchors = new(true, true);
        
        private Pair _prevIsAdaptX;
        private Pair _prevIsAdaptY;
        
        private Vector2 _defaultAnchorMin;
        private Vector2 _defaultAnchorMax;
        
        private RectTransform _rectTransform;
        private ScreenOrientation _prevOrientation;
        private Vector2Int _prevScreenSize;
        private Rect _prevSafeAreaRect;

        private void Awake()
        {
            _prevIsAdaptX = isAdaptXAnchors;
            _prevIsAdaptY = isAdaptYAnchors;
            
            _rectTransform = GetComponent<RectTransform>();
            _defaultAnchorMin = _rectTransform.anchorMin;
            _defaultAnchorMax = _rectTransform.anchorMax;
            
            _prevOrientation = Screen.orientation;
            _prevScreenSize = new Vector2Int(Screen.width, Screen.height);
            
            AdaptUiForSafeZone();
        }

#if UNITY_EDITOR
        private void LateUpdate() 
            => AdaptUiForSafeZone();
#endif

        private void AdaptUiForSafeZone()
        {
            var safeAreaRect = Screen.safeArea;
            
            if(safeAreaRect != _prevSafeAreaRect || 
               Screen.width != _prevScreenSize.x || 
               Screen.height != _prevScreenSize.y || 
               Screen.orientation != _prevOrientation || 
               _prevIsAdaptX != isAdaptXAnchors || 
               _prevIsAdaptY != isAdaptYAnchors)
            {
                var anchorMin = safeAreaRect.position;
                var anchorMax = safeAreaRect.position + safeAreaRect.size;

                if (isAdaptXAnchors.MinAnchor)
                    anchorMin.x /= Screen.width;
                else
                    anchorMin.x = _defaultAnchorMin.x;
                    
                if (isAdaptXAnchors.MaxAnchor)
                    anchorMax.x /= Screen.width;
                else
                    anchorMax.x = _defaultAnchorMax.x;
 
                if (isAdaptYAnchors.MinAnchor) 
                    anchorMin.y /= Screen.height;
                else
                    anchorMin.y = _defaultAnchorMin.y;

                if (isAdaptYAnchors.MaxAnchor) 
                    anchorMax.y /= Screen.height;
                else
                    anchorMax.y = _defaultAnchorMax.y;

                _rectTransform.anchorMin = anchorMin;
                _rectTransform.anchorMax = anchorMax;
                
                _prevIsAdaptX = isAdaptXAnchors;
                _prevIsAdaptY = isAdaptYAnchors;
                _prevOrientation = Screen.orientation;
                _prevScreenSize.x = Screen.width;
                _prevScreenSize.y = Screen.height;
                _prevSafeAreaRect = safeAreaRect;
            }
        }
        
        [Serializable]
        private struct Pair
        {
            [field: SerializeField] public bool MinAnchor { get; private set; }
            [field: SerializeField] public bool MaxAnchor { get; private set; }

            public Pair(bool minAnchor, bool maxAnchor)
            {
                MinAnchor = minAnchor;
                MaxAnchor = maxAnchor;
            }

            public static bool operator ==(Pair pairLeft, Pair pairRight) 
                => pairLeft.MinAnchor == pairRight.MinAnchor && pairLeft.MaxAnchor == pairRight.MaxAnchor;

            public static bool operator !=(Pair pairLeft, Pair pairRight) 
                => !(pairLeft == pairRight);
            
            public override bool Equals(object obj)
            {
                if (obj is Pair)
                {
                    var pair = (Pair)obj;
                    return this == pair;
                }
                return false;
            }

            public override int GetHashCode() 
                => (MinAnchor, MaxAnchor).GetHashCode();
        }
    }
}