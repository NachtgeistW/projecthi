using UnityEngine;

namespace Rayark.Hi.Engine
{
    public class SpeedUpFloor : Item
    {
        private const float SPEED_UP_RATIO = 1.5f;
        private const float SPEED_UP_AMOUNT = 50;

        private Vector2 _size;

        public SpeedUpFloor(Vector2 size)
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
            effect.SpeedChangeCharacterSpeed(SPEED_UP_RATIO, SPEED_UP_AMOUNT);
        }
    }
}