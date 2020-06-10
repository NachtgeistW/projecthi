using UnityEngine;

namespace Rayark.Hi.Engine
{
    public class SpeedUpFloor : IItem
    {
        private const float SPEED_UP_RATIO = 1.5f;
        private const float SPEED_UP_AMOUNT = 50;

        private Vector2 _size;

        public SpeedUpFloor(Vector2 size)
        {
            _size = size;
        }

        Vector2 IItem.Size
        {
            get { return _size; }
        }

        float IItem.GeneratedProbability
        {
            get { return 0.5f; }
        }

        void IItem.GetEffect(IEffect effect)
        {
            effect.SpeedChangeCharacterSpeed(SPEED_UP_RATIO, SPEED_UP_AMOUNT);
        }
    }
}