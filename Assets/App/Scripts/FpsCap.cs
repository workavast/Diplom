using UnityEngine;

namespace App
{
    public class FpsCap : MonoBehaviour
    {
        private void Awake() 
            => Application.targetFrameRate = 60;
    }
}