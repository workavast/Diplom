using Fusion;
using UnityEngine;

namespace App.Entities
{
    public interface IEntity
    {
        EntityIdentifier Identifier { get; }
        EntityType EntityType { get; }
        GameObject GameObject { get; }
        NetworkRunner Runner { get; }
        NetworkObject Object { get; }
        int NetHealthPoints { get; }

        string GetName();
    }
}