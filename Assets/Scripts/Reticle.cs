using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class Reticle : MonoBehaviour
    {

        [Header("Animation Settings")] [SerializeField]
        private AnimationCurve _animCurve;

        [SerializeField] private float _selectAnimTime;
        [SerializeField] private float _deselectAnimTime;

        [Header("Drag Settings")] [SerializeField]
        private float _amplitude;

        [SerializeField] private float _stiffness;
        [SerializeField] private float _clamp;

        [Header("Points")] [SerializeField] private List<Point> _points = new List<Point>();
        [SerializeField] private List<ShootProjectile> _launchPoints = new List<ShootProjectile>();

        private readonly List<Vector3> _pointsStartPos = new List<Vector3>();
        private Clickable _selectedObject;
        private Camera _camera;
        private Point _point;
        private int _count = 0;

        private float _magnitude;
        private float _forceDistance;

        public void Initialize()
        {
            _camera = Camera.main;
            _point = _points[_points.Count - 1];
            foreach (Point point in _points)
            {
                _pointsStartPos.Add(point.transform.localPosition);
            }
        }

        private void Update()
        {
            if (_selectedObject)
            {
                StopAllCoroutines();
                Selected(_selectedObject);
            }
        }

        private void HandleRotation(GameObject item)
        {
            Vector2 dir = Input.mousePosition - _camera.WorldToScreenPoint(transform.position);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            item.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }


        private void HandlePoint(Vector3 mousePosRelative, float distance, int i, Point point)
        {
            int dir = 0;
            if (point.Flip)
                dir = -1;
            else
                dir = 1;

            Vector3 startPos = _pointsStartPos[i];

            float magnitude = (_amplitude * distance) / distance;
            float pointIdentity = (startPos.x * magnitude) * dir;
            Vector3 targetPos = mousePosRelative * (pointIdentity * dir);

            float lerpTime = (_selectAnimTime / pointIdentity) * Time.deltaTime;
            Vector3 lerpPos = Vector3.Lerp(point.transform.localPosition, targetPos, lerpTime);

            lerpPos.z = 0;
            float pointDistance = Vector3.Distance(point.transform.position, transform.position);
            lerpPos = Vector3.ClampMagnitude(lerpPos, (pointIdentity / magnitude) + (pointDistance / _clamp));

            point.transform.localPosition = lerpPos;
        }

        public void Selected(Clickable selected)
        {
            gameObject.SetActive(true);
            transform.position = selected.transform.position;
            _selectedObject = selected;
            Vector3 mousePos = Input.mousePosition;
            Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(mousePos);
            Vector3 mousePosRelative = mouseWorldPos - transform.position;
            for (int i = 0; i < _points.Count; i++)
            {
                HandleRotation(_points[i].gameObject);
                float distance = Vector2.Distance(transform.position, mouseWorldPos);
                Point point = _points[i];
                HandlePoint(mousePosRelative, distance, i, point);
            }

            _forceDistance = Vector3.Distance(_point.transform.position, transform.position);
        }

        public void Deselected()
        {
            _selectedObject = null;
            for (int i = 0; i < _points.Count; i++)
                StartCoroutine(LerpObject(_points[i], Vector3.zero, _deselectAnimTime));
        }

        private void FireProjectiles()
        {
            if (_forceDistance < 0.3f)
                return;

            for (int i = 0; i < _launchPoints.Count; i++)
            {
                _launchPoints[i].FireProjectile(_forceDistance);
            }

            gameObject.SetActive(false);
        }

        private IEnumerator LerpObject(Point item, Vector3 pos, float time)
        {
            Vector3 currentPos = item.transform.localPosition;
            float elapsed = 0f;
            float ratio = 0;
            while (ratio < 1)
            {
                elapsed += Time.fixedDeltaTime;
                float offset = _animCurve.Evaluate(ratio);
                float newOffset = offset - ratio;
                newOffset = newOffset / _stiffness;
                offset = newOffset + ratio;
                float invertOffset = 1.0f - offset;
                item.transform.localPosition = Vector3.Lerp(currentPos, pos, ratio) * invertOffset;

                yield return null;
                ratio = (elapsed / time);
            }

            _count++;
            if (_count >= _points.Count)
            {
                FireProjectiles();
            }
        }
    }
}