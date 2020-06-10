using UnityEngine;

namespace Rayark.Hi.Engine
{
    public interface IItem
    {
        Vector2 Size { get; }
        float GeneratedProbability { get; }

        void GetEffect(IEffect effect);
    }
    
}