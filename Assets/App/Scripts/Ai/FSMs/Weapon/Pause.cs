using App.Entities;
using UnityEngine;

namespace App.Ai.FSMs.Weapon
{
    public class Pause : WeaponState
    {
        private readonly float _minDuration;
        private readonly float _maxDuration;
        private readonly AiViewZone _aiViewZone;
        private readonly AiModel _aiModel;

        private float _targetDuration;
        
        public Pause(NetEntity entity, float minDuration, float maxDuration, AiViewZone aiViewZone, AiModel aiModel) 
            : base(entity)
        {
            _minDuration = minDuration;
            _maxDuration = maxDuration;
            _aiViewZone = aiViewZone;
            _aiModel = aiModel;
        }

        protected override void OnEnterState()
        {
            _targetDuration = Random.Range(_minDuration, _maxDuration);
        }

        protected override void OnFixedUpdate()
        {
            if (StateTime >= _targetDuration)
            {
                if (_aiViewZone.EntityIsVisible(_aiModel.Target))
                {
                    TryActivateState<Shooting>();
                    return;
                }
                else
                {
                    ForceActivateState<Pause>(true);
                    return;
                }
            }
        }
    }
}