using App.Damage;
using Fusion;
using UnityEngine;

namespace App.Entities
{
    public class EntityProvider : MonoBehaviour, IEntity, IDamageable
    {
        [SerializeField] private NetEntityBase netEntityBase;
        
        public EntityIdentifier Identifier => netEntityBase.Identifier;
        public EntityType EntityType => netEntityBase.EntityType;
        public GameObject GameObject => netEntityBase.GameObject;
        public NetworkRunner Runner => netEntityBase.Runner;
        public NetworkObject Object => netEntityBase.Object;
        public int NetHealthPoints => netEntityBase.NetHealthPoints;

        public string GetName() => netEntityBase.GetName();

        public void TakeDamage(float damage, IEntity shooter) => netEntityBase.TakeDamage(damage, shooter);
    }
}