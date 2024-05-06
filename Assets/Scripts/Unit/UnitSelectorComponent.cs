using System.Collections.Generic;
using UnityEngine;

namespace Dragoncraft
{
    public class UnitSelectorComponent : MonoBehaviour
    {
        private MeshCollider _meshCollider = null;
        private Vector3 _startPosition;
        private List<UnitComponent> _units = new List<UnitComponent>();

        private void Awake()
        {
            GameObject plane = GameObject.FindGameObjectWithTag("Plane");

            if (plane != null)
            {
                _meshCollider = plane.GetComponent<MeshCollider>();
            }

            if (_meshCollider == null)
            {
                Debug.LogError("Missing tag and/or MeshCollider reference!");
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _startPosition = GetMousePosition();
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Vector3 endPosition = GetMousePosition();
                SelectUnits(_startPosition, endPosition);
            }
        }

        private Vector3 GetMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;

            if (_meshCollider.Raycast(ray, out hitData, 1000))
            {
                return hitData.point;
            }

            return Vector3.zero;
        }

        private void SelectUnits(Vector3 startPosition, Vector3 endPosition)
        {
            foreach (UnitComponent unit in _units)
            {
                unit.Selected(false);
            }

            _units.Clear();

            Vector3 center = (startPosition + endPosition) / 2;
            float distance = Vector3.Distance(center, endPosition);
            Vector3 halfExtents = new Vector3(distance, distance, distance);

            Collider[] colliders = Physics.OverlapBox(center, halfExtents);

            foreach (Collider collider in colliders)
            {
                UnitComponent unit = collider.GetComponent<UnitComponent>();

                if (unit != null)
                {
                    unit.Selected(true);
                    _units.Add(unit);
                }
            }
        }
    }
}
