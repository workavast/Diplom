using System;
using UnityEngine;

namespace Avastrad.CustomTimer
{
    public class Timer : IReadOnlyTimer
    {
        public float MaxTime { get; private set; }
        public float CurrentTime { get; private set; }
        
        public bool TimerIsEnd { get; private set; }
        public bool Paused { get; private set; }
        
        public event Action OnTimerEnd;
        
        public Timer(float maxTime, float startTime = 0, bool paused = false)
        {
            this.MaxTime = maxTime;
            this.CurrentTime = startTime;
            Paused = paused;
            
            if (CurrentTime >= MaxTime)
                TimerIsEnd = true;
        }
        
        /// <summary>
        /// Set new max time of timer
        /// </summary>
        /// <remarks>when reset is false and timer already ended then doesnt invoke timer end</remarks>
        public void SetMaxTime(float newMaxTime, bool reset = true, bool saveCurrentTime = false)
        {
            MaxTime = newMaxTime;
            
            var prevCurrentTime = 0f;
            if (saveCurrentTime)
                prevCurrentTime = CurrentTime;

            if (reset)
            {
                Reset();
                UpdateTimer(prevCurrentTime);
            }
            else
            {
                CurrentTime = prevCurrentTime;
                
                if (!TimerIsEnd && !Paused && CurrentTime >= MaxTime)
                {
                    TimerIsEnd = true;
                    OnTimerEnd?.Invoke();
                }
            }
        }

        /// <summary>
        /// Set new max time of timer
        /// </summary>
        /// <remarks>if timer already ended then doesnt invoke timer end</remarks>
        public void SetCurrentTime(float newCurrentTime)
        {
            CurrentTime = newCurrentTime;
                
            if (!TimerIsEnd && !Paused && CurrentTime >= MaxTime)
            {
                TimerIsEnd = true;
                OnTimerEnd?.Invoke();
            }
        }
        
        public void Reset(bool paused = false)
        {
            TimerIsEnd = false;
            CurrentTime = 0;
            Paused = paused;
        }
        
        public void SetPause(bool paused = true) 
            => Paused = paused;
        
        public void Tick(float time)
        {
            if(TimerIsEnd || Paused) 
                return;
            
            UpdateTimer(time);
        }

        private void UpdateTimer(float time)
        {
            CurrentTime += time;
            CurrentTime = Mathf.Clamp(CurrentTime, 0, MaxTime);
            
            if (CurrentTime >= MaxTime)
            {
                TimerIsEnd = true;
                OnTimerEnd?.Invoke();
            }
        }
    }
}