using UnityEngine;

namespace DefaultNamespace
{
    public class Point : MonoBehaviour
    {
        [SerializeField] private bool _flip;

        public bool Flip => _flip;
    }
}