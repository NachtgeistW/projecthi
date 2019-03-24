using UnityEngine;

namespace Rayark.Hi.Engine
{
    public class SpeedDownFloor : Item
    {
        private const float SPEED_DOWN_RATIO = 0.7f;
        private const float SPEED_DOWN_AMOUNT = -10f;

        private Vector2 _size;

        public SpeedDownFloor(Vector2 size)
        {
            _size = size;
        }

        Vector2 Item.Size
        {
            get { return _size; }
        }

        float Item.GeneratedProbability
        {
            get { return 0.5f; }
        }

        void Item.GetEffect(IEffect effect)
        {
            effect.SpeedChangeCharacterSpeed(SPEED_DOWN_RATIO, SPEED_DOWN_AMOUNT);
        }
    }
}