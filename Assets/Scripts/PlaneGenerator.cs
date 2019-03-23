using UnityEngine;

namespace Rayark.Hi
{
    public class PlaneGenerator : MonoBehaviour
    {
        public const float MIN_X_VALUE = -4;
        public const float MAX_X_VALUE = 4;
        private const float PLANE_DIFF_Z = 33f;

        [SerializeField]
        private Transform _planeRootTransform;

        [SerializeField]
        private GameObject _planePrefab;

        [SerializeField]
        private int _planeCount = 10;

        private GameObject[] _inScenePlanes;
        private int _currentPlaneIndex = 0;

        void Start()
        {
            _inScenePlanes = new GameObject[_planeCount];
            for(int i = 0; i < _planeCount; ++i)
            {
                _inScenePlanes[i] = Instantiate(_planePrefab, _planeRootTransform);
                _inScenePlanes[i].transform.localPosition = new Vector3(0, 0, i * PLANE_DIFF_Z);
                
                if(i % 5 == 4)
                {
                    _inScenePlanes[i].GetComponent<MeshRenderer>().material.color = Color.blue;
                }
            }
        }
        
        public void UpdatePlanes(float characterPositionZ)
        {
            int nextPlaneIndex = _GetNextPlaneIndex(_currentPlaneIndex);

            if(characterPositionZ > _inScenePlanes[nextPlaneIndex].transform.localPosition.z)
            {
                var currentPlane = _inScenePlanes[_currentPlaneIndex];
                var lastPlane = _inScenePlanes[_GetLastPlaneIndex(_currentPlaneIndex)];

                currentPlane.transform.localPosition = new Vector3(
                   currentPlane.transform.localPosition.x,
                   currentPlane.transform.localPosition.y,
                   lastPlane.transform.localPosition.z + PLANE_DIFF_Z);

                _currentPlaneIndex = _GetNextPlaneIndex(_currentPlaneIndex);
            }
        }


        private int _GetNextPlaneIndex(int index)
        {
            return (_currentPlaneIndex + 1) % _planeCount;
        }

        private int _GetPreviousPlaneIndex(int index)
        {
            return (_currentPlaneIndex - 1 + _planeCount) % _planeCount;
        }

        private int _GetLastPlaneIndex(int currentIndex)
        {
            return _GetPreviousPlaneIndex(currentIndex);
        }
    }
}