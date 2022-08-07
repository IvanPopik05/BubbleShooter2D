using UnityEngine;

namespace DefaultNamespace
{
    public class ShootProjectile : MonoBehaviour
    {
        [SerializeField] private Bubble _bubblePrefab;
        [SerializeField] private float _speed;
        public void FireProjectile(float forceDistance)
        {
            Bubble newBubble = Instantiate(_bubblePrefab, null);
            newBubble.transform.position = transform.position;
            newBubble.Initialize();
            newBubble.Push(-transform.right * (_speed * forceDistance));
        }
    }
}