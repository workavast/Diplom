using UnityEngine;

namespace App.Bullets
{
    public class BulletView : MonoBehaviour
    {
        public void Move(float speed, float deltaTime)
            => Move(transform.forward, speed, deltaTime);
        
        public void Move(Vector3 direction, float speed, float deltaTime) 
            => transform.position += (direction * speed * deltaTime);
    }
}