using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rayark.Hi.Engine
{
    public class HiEngine : IEffect
    {
        public class ItemInstance
        {
            public Item Data;
            public Vector2 Position;
            public bool IsUsed;
        }

        public const int SWIPE_INIT_COUNT = 5;

        private const float SWIPE_THRESHOLD = 30f;
        private const float SECTION_DISTANCE = 165f;
        private const float SECTION_DIFF = 33f;
        private const float MIN_X_VALUE = 0;
        private const float MAX_X_VALUE = 1;

        private float _xScale;
        private CharacterData _currentCharacter;
        private Item[] _items;
        private List<ItemInstance> _itemInstances;
        private int _swipeRemainCount;
        private float _runMiles;
        private int _sectionIndex;
        
        public Vector2 CurrentCharacterPosition
        {
            get
            {
                return _currentCharacter.Position;
            }
        }

        public float CurrentCharacterSpeed
        {
            get
            {
                return _currentCharacter.Speed;
            }
        }

        public int SwipeRemainCount
        {
            get
            {
                return _swipeRemainCount;
            }
        }

        public IEnumerable<ItemInstance> Items
        {
            get
            {
                return _itemInstances;
            }
        }

        public HiEngine(float xScale, CharacterData currentCharacter, Item[] items)
        {
            _xScale = xScale;
            _currentCharacter = currentCharacter;
            _items = items;
            _swipeRemainCount = SWIPE_INIT_COUNT;
            _runMiles = 0;

            _itemInstances = new List<ItemInstance>();
            var speedUpFloor = _items.FirstOrDefault(item => item is SpeedUpFloor);
            if(speedUpFloor != null)
            {
                _itemInstances.Add(new ItemInstance
                {
                    Data = speedUpFloor,
                    Position = new Vector2(0, 34)
                });
            }
        }

        public void Update(float deltaTime)
        {
            _currentCharacter.Position += deltaTime * _currentCharacter.UnitDirection * _currentCharacter.Speed;

            if(_currentCharacter.Position.x <= MIN_X_VALUE ||
               _currentCharacter.Position.x >= MAX_X_VALUE)
            {
                _currentCharacter.UnitDirection.x = -_currentCharacter.UnitDirection.x;
            }

            _currentCharacter.Position.x = Mathf.Clamp(_currentCharacter.Position.x, MIN_X_VALUE, MAX_X_VALUE);

            _runMiles = _currentCharacter.Position.y;
            int previousSectionIndex = _sectionIndex;
            _sectionIndex = Mathf.FloorToInt((_runMiles + SECTION_DIFF) / SECTION_DISTANCE);
            if (_IsGetSwipe(_sectionIndex, previousSectionIndex))
                ++_swipeRemainCount;

            _currentCharacter.Speed =
                Mathf.Max(0, (1 - _currentCharacter.SpeedDownRatio * deltaTime) * _currentCharacter.Speed - _currentCharacter.SpeedDownAmount * deltaTime);

            _TouchItems(_currentCharacter.Position, _currentCharacter.Size, _itemInstances);
        }

        public void ReduceSwipeRemainCount()
        {
            --_swipeRemainCount;
        }

        public void SpeedUpCharacterSpeed()
        {
            SpeedUpCharacterSpeed(_currentCharacter.SpeedUpRatio, _currentCharacter.SpeedUpAmount);
        }

        public void SpeedUpCharacterSpeed(float speedUpRatio, float speedUpAmount)
        {
            _currentCharacter.Speed =
                _currentCharacter.Speed * speedUpRatio +
                speedUpAmount;
        }

        public void ChangeCharacterDirection(Vector2 direction)
        {
            if (direction.magnitude < SWIPE_THRESHOLD)
                return;
            if (direction.y < 0)
                return;
            if (Mathf.Abs(direction.x) > Mathf.Sqrt(3) * direction.y)
                return;

            direction.x *= _xScale;
            _currentCharacter.UnitDirection = direction.normalized;
            
        }

        private bool _IsGetSwipe(int currentSectionIndex, int previousSectionIndex)
        {
            return currentSectionIndex != previousSectionIndex;
        }

        private void _TouchItems(Vector2 characterPosition, float characterSize, IEnumerable<ItemInstance> itemInstances)
        {
            Rect characterRect = new Rect(characterPosition, new Vector2(characterSize, characterSize));
            foreach(var item in itemInstances)
            {
                if (item.IsUsed)
                    continue;

                Rect itemRect = new Rect(item.Position, item.Data.Size);
                if (itemRect.Overlaps(characterRect))
                {
                    item.Data.GetEffect(this);
                    item.IsUsed = true;
                }
            }
        }

        
    }
}