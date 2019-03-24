using UnityEngine;

namespace Rayark.Hi.Engine
{
    public interface Item
    {
        Vector2 Size { get; }
        float GeneratedProbability { get; }

        void GetEffect(IEffect effect);
    }

    public class SpeedUpFloor : Item
    {
        private const float SPEED_UP_RATIO = 2;
        private const float SPEED_UP_AMOUNT = 30;

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
            effect.SpeedUpCharacterSpeed(SPEED_UP_RATIO, SPEED_UP_AMOUNT);
        }
    }
}