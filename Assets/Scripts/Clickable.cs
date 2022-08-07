using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Clickable : MonoBehaviour
    {
        [SerializeField] private Reticle _reticleHandler;
        
        private Camera _camera;
        private void Start()
        {
            _camera = Camera.main;
            
            _reticleHandler.Initialize();
            transform.position = new Vector2(_camera.transform.position.x,_camera.transform.position.y * 2);
        }

        private void OnMouseDown()
        {
            _reticleHandler.Selected(this);
        }

        private void OnMouseUp()
        {
            _reticleHandler.Deselected();
        }
    }
}