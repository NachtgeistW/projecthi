using UnityEngine;

namespace Rayark.Hi
{
    public static class RectExtension
    {
        public static Rect ToRect(Vector2 centerPosition, Vector2 size)
        {
            return new Rect(centerPosition.x - size.x / 2, centerPosition.y - size.y / 2,
                size.x, size.y);
        }
    }
}