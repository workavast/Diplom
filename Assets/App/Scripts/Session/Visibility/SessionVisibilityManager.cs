using System;
using App.NetworkRunning;
using UnityEngine;
using Zenject;

namespace App.Session.Visibility
{
    public class SessionVisibilityManager
    {
        [Inject] private readonly NetworkRunnerProvider _networkRunnerProvider;

        private int _isVisibleCalls;
        
        public void SetVisibility(bool isVisible, bool isForce = false)
        {
            if (isForce)
            {
                if (isVisible)
                    _isVisibleCalls = 1;
                else
                    _isVisibleCalls = 0;
            }
            else
            {
                if (isVisible)
                    _isVisibleCalls++;
                else
                    _isVisibleCalls--;
            }
            
            if (_isVisibleCalls == 1)
            {
                if (_networkRunnerProvider.TryGetNetworkRunner(out var runner))
                    runner.SessionInfo.IsOpen = true;
                else
                    throw new NullReferenceException("Runner is null");
            }
            else
            {
                if (_isVisibleCalls < 0)
                    Debug.LogError($"Session visibility calls is less then 0: [{_isVisibleCalls}]");

                if (_isVisibleCalls == 0)
                {
                    if (_networkRunnerProvider.TryGetNetworkRunner(out var runner))
                        runner.SessionInfo.IsOpen = true;
                    else
                        throw new NullReferenceException("Runner is null");
                }
            }
        }
        
        public void SetHardVisibility(bool isVisible)
        {
            if (_networkRunnerProvider.TryGetNetworkRunner(out var runner))
                runner.SessionInfo.IsOpen = runner.SessionInfo.IsVisible = isVisible;
            else
                throw new NullReferenceException("Runner is null");
        }
    }
}