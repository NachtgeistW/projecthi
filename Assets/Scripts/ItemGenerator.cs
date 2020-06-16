using System.Collections.Generic;
using UnityEngine;

namespace Rayark.Hi
{
    public class ItemGenerator : MonoBehaviour
    {
        public class ItemData
        {
            public Vector3 Position;
            public bool IsUsed;
        }

        [SerializeField]
        private Transform _itemRootTransform;

        [SerializeField]
        private GameObject _itemPrefab;

        private List<GameObject> _itemInstances = new List<GameObject>();

        private bool _isDisplay = true;

        public void SetDisplayUsedItem(bool isDisplay)
        {
            _isDisplay = isDisplay;
        }

        public void UpdateItems(ItemData[] items)
        {
            if(items.Length > _itemInstances.Count)
            {
                for (int i = _itemInstances.Count; i < items.Length; ++i)
                {
                    _itemInstances.Add(Instantiate(_itemPrefab, _itemRootTransform));
                }
            }

            var itemInstances = _itemInstances.ToArray();
            for(int i = 0; i < items.Length; ++i)
            {
                itemInstances[i].SetActive(!items[i].IsUsed || _isDisplay);
                itemInstances[i].transform.localPosition = items[i].Position;
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