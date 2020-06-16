//Use to decrease RST
using UnityEngine;

namespace Rayark.Hi.Engine
{
    public class SwipeDownItem : IItem
    {
        private const int SwipeDownAmount = -1;

        private readonly Vector2 _size;

        public SwipeDownItem(Vector2 size)
        {
            _size = size;
        }

        Vector2 IItem.Size => _size;

        float IItem.GeneratedProbability => 0.8f;

        void IItem.GetEffect(IEffect effect)
        {
            effect.ChangeRemainSwipeCount(SwipeDownAmount);
        }
    }
}