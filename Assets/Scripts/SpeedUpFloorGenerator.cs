using System.Collections.Generic;
using UnityEngine;

namespace Rayark.Hi
{
    public class SpeedUpFloorGenerator : MonoBehaviour
    {
        [SerializeField]
        private Transform _speedUpFloorRootTransform;

        [SerializeField]
        private GameObject _speedUpFloorPrefab;

        private List<GameObject> _speedUpFloorInstances = new List<GameObject>();

        public void UpdateSpeedUpFloor(Vector3[] positions)
        {
            if(positions.Length > _speedUpFloorInstances.Count)
            {
                _speedUpFloorInstances.Add(Instantiate(_speedUpFloorPrefab, _speedUpFloorRootTransform));
            }

            var speedUpFloors = _speedUpFloorInstances.ToArray();
            for(int i = 0; i < positions.Length; ++i)
            {
                speedUpFloors[i].transform.localPosition = positions[i];
            }
        }
    }
}