using System.Collections.Generic;
using UnityEngine;

namespace Rayark.Hi
{
    public class ItemGenerator : MonoBehaviour
    {
        [SerializeField]
        private Transform _itemRootTransform;

        [SerializeField]
        private GameObject _itemPrefab;

        private List<GameObject> _itemInstances = new List<GameObject>();

        public void UpdateItems(Vector3[] positions)
        {
            if(positions.Length > _itemInstances.Count)
            {
                for (int i = _itemInstances.Count; i < positions.Length; ++i)
                {
                    _itemInstances.Add(Instantiate(_itemPrefab, _itemRootTransform));
                }
            }

            var items = _itemInstances.ToArray();
            for(int i = 0; i < positions.Length; ++i)
            {
                items[i].transform.localPosition = positions[i];
            }
        }

        public void ReleaseObjects()
        {
            foreach(var instance in _itemInstances)
            {
                Destroy(instance);
            }
            _itemInstances.Clear();
        }
    }
}