using App.Entities;
using UnityEngine;

namespace App.Ai
{
    public class AiModel
    {
        public IEntity Target { get; set; }
        public Vector3 LastTargetPosition { get; set; }
        public AiConfig Config { get; private set; }
        
        public AiModel(AiConfig config)
        {
            Config = config;
        }
    }
}