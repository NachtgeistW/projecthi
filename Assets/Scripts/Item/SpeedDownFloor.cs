using UnityEngine;

namespace Rayark.Hi.Engine
{
    public class SpeedDownFloor : IItem
    {
        private const float SPEED_DOWN_RATIO = 0.6f;
        private const float SPEED_DOWN_AMOUNT = -10f;

        private Vector2 _size;

        public SpeedDownFloor(Vector2 size)
        {
            _size = size;
        }

        Vector2 IItem.Size
        {
            get { return _size; }
        }

        float IItem.GeneratedProbability
        {
            get { return 0.7f; }
        }

        void IItem.GetEffect(IEffect effect)
        {
            effect.SpeedChangeCharacterSpeed(SPEED_DOWN_RATIO, SPEED_DOWN_AMOUNT);
        }
    }
}