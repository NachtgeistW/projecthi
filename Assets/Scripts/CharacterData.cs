using System;
using UnityEngine;

namespace Rayark.Hi.Engine
{
    [Serializable]
    public class CharacterData
    {
        public Vector2 Position;
        public float Speed;
        public float SlowSpeedRatio;
        public float SlowSpeedAmount;
    }
}