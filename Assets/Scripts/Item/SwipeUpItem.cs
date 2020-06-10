using UnityEngine;

namespace Rayark.Hi.Engine
{
    public class SwipeUpItem : IItem
    {
        private const int SWIPE_UP_AMOUNT = 1;

        private Vector2 _size;

        public SwipeUpItem(Vector2 size)
        {
            _size = size;
        }

        Vector2 IItem.Size
        {
            get { return _size; }
        }

        float IItem.GeneratedProbability
        {
            get { return 0.8f; }
        }

        void IItem.GetEffect(IEffect effect)
        {
            effect.ChangeRemainSwipeCount(SWIPE_UP_AMOUNT);
        }
    }
}