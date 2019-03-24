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
        private const float ITEM_SECTION_DISTANCE = 50f;
        private const float SWIPE_SECTION_DISTANCE = 165f;
        private const float SWIPE_SECTION_DIFF = 33f;
        private const float ITEM_GENERATED_DISTANCE = 330f;
        private const int ITEM_SECTION_DIFF = (int)(ITEM_GENERATED_DISTANCE / ITEM_SECTION_DISTANCE);
        private const float MIN_X_VALUE = 0;
        private const float MAX_X_VALUE = 1;
        private const float EMPTY_PROBABILITY = 0.3f;

        private float _xScale;
        private CharacterData _currentCharacter;
        private Item[] _items;
        private List<ItemInstance> _itemInstances;
        private int _swipeRemainCount;
        private float _runMiles;
        private int _swipeSectionIndex;
        private int _lastItemGeneratedSectionIndex;
        
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

        public float Distance
        {
            get
            {
                return _runMiles;
            }
        }


        public HiEngine(float xScale, CharacterData currentCharacter, Item[] items)
        {
            _xScale = xScale;
            _currentCharacter = new CharacterData
            {
                Position = currentCharacter.Position,
                Size = currentCharacter.Size,
                Speed = currentCharacter.Speed,
                SpeedUpRatio = currentCharacter.SpeedUpRatio,
                SpeedUpAmount = currentCharacter.SpeedUpAmount,
                UnitDirection = currentCharacter.UnitDirection,
                SpeedDownRatio = currentCharacter.SpeedDownRatio,
                SpeedDownAmount = currentCharacter.SpeedDownAmount,
            };
            _items = items;
            _swipeRemainCount = SWIPE_INIT_COUNT;
            _runMiles = 0;
            _lastItemGeneratedSectionIndex = -1;

            _itemInstances = new List<ItemInstance>();
            /*
            var speedUpFloor = _items.FirstOrDefault(item => item is SpeedUpFloor);
            if(speedUpFloor != null)
            {
                _itemInstances.Add(new ItemInstance
                {
                    Data = speedUpFloor,
                    Position = new Vector2(0, 34)
                });
            }
            */
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
            int previousSectionIndex = _swipeSectionIndex;
            _swipeSectionIndex = Mathf.FloorToInt((_runMiles + SWIPE_SECTION_DIFF) / SWIPE_SECTION_DISTANCE);
            if (_IsGetSwipe(_swipeSectionIndex, previousSectionIndex))
                (this as IEffect).ChangeRemainSwipeCount(1);

            _currentCharacter.Speed =
                Mathf.Max(0, (1 - _currentCharacter.SpeedDownRatio * deltaTime) * _currentCharacter.Speed - _currentCharacter.SpeedDownAmount * deltaTime);

            _GenerateItems(_lastItemGeneratedSectionIndex);
            _TouchItems(_currentCharacter.Position, _currentCharacter.Size, _itemInstances);

        }

        public void ReduceSwipeRemainCount()
        {
            (this as IEffect).ChangeRemainSwipeCount(-1);
        }

        public void SpeedUpCharacterSpeed()
        {
            SpeedChangeCharacterSpeed(_currentCharacter.SpeedUpRatio, _currentCharacter.SpeedUpAmount);
        }
        
        public void SpeedChangeCharacterSpeed(float speedChangeRatio, float speedChangeAmount)
        {
            _currentCharacter.Speed =
                _currentCharacter.Speed * speedChangeRatio +
                speedChangeAmount;

            _currentCharacter.Speed = Mathf.Max(0f, _currentCharacter.Speed);
        }

        void IEffect.ChangeRemainSwipeCount(int amount)
        {
            _swipeRemainCount += amount;
            _swipeRemainCount = Mathf.Max(0, _swipeRemainCount);
        }


        public void ChangeCharacterDirection(Vector2 direction)
        {
            if (direction.magnitude < SWIPE_THRESHOLD)
                return;
            if (direction.y < 0)
                direction.y = -direction.y;
            if (Mathf.Abs(direction.x) > Mathf.Sqrt(3) * direction.y)
                direction = new Vector2(direction.x, Mathf.Abs(direction.x / Mathf.Sqrt(3)));

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

                Rect itemRect = new Rect(item.Position - item.Data.Size / 2, item.Data.Size);
                if (itemRect.Overlaps(characterRect))
                {
                    item.Data.GetEffect(this);
                    item.IsUsed = true;
                }
            }
        }

        private void _GenerateItems(int lastItemGeneratedSectionIndex)
        {
            int currentItemSectionIndex = Mathf.FloorToInt(_runMiles / ITEM_SECTION_DISTANCE);
            if (currentItemSectionIndex >= lastItemGeneratedSectionIndex - ITEM_SECTION_DIFF)
            {
                for(int i = lastItemGeneratedSectionIndex + 1; i <= currentItemSectionIndex + ITEM_SECTION_DIFF; ++i)
                {
                    var itemIndex = _SelectItemGeneratedIndex(_items.Select(item => item.GeneratedProbability).Concat(new float[] { EMPTY_PROBABILITY }).ToArray());
                    if (itemIndex >= _items.Length)
                        continue;
                    var generatedItem = _items[itemIndex];
                    
                    Rect itemRange = new Rect(new Vector2(0, i * ITEM_SECTION_DISTANCE), new Vector2(5, 10));
                    _itemInstances.Add(new ItemInstance
                    {
                        Data = generatedItem,
                        Position = new Vector2(Random.Range(itemRange.xMin, itemRange.xMax), Random.Range(itemRange.yMin, itemRange.yMax)),
                        IsUsed = false,
                    });
                            
                    _lastItemGeneratedSectionIndex = i;
                }
            }
        }
        

        private int _SelectItemGeneratedIndex(float[] weights)
        {
            float totalWeight = weights.Sum();
            float selectedWeight = Random.Range(0f, totalWeight);
            for(int i = 0; i < weights.Length; ++i)
            {
                selectedWeight -= weights[i];
                if(selectedWeight <= 0f)
                {
                    return i;
                }
            }

            return weights.Length - 1;
        }
    }
}