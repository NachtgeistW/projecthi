using UnityEngine;

namespace Rayark.Hi.Engine
{
    public class HiEngine
    {
        private const float SWIPE_THRESHOLD = 30f;
        private const int SWIPE_INIT_COUNT = 5;
        private const float MIN_X_VALUE = 0;
        private const float MAX_X_VALUE = 1;

        private float _xScale;
        private CharacterData _currentCharacter;
        private int _swipeRemainCount;
        
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

        public HiEngine(float xScale, CharacterData currentCharacter)
        {
            _xScale = xScale;
            _currentCharacter = currentCharacter;
            _swipeRemainCount = SWIPE_INIT_COUNT;
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

            _currentCharacter.Speed =
                Mathf.Max(0, (1 - _currentCharacter.SpeedDownRatio * deltaTime) * _currentCharacter.Speed - _currentCharacter.SpeedDownAmount * deltaTime);
        }

        public void ReduceSwipeRemainCount()
        {
            --_swipeRemainCount;
        }

        public void SpeedUpCharacterSpeed()
        {
            _currentCharacter.Speed =
                _currentCharacter.Speed * _currentCharacter.SpeedUpRatio + 
                _currentCharacter.SpeedUpAmount;
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
    }
}