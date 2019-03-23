using System;
using UnityEngine;

namespace Rayark.Hi.Engine
{
    [Serializable]
    public class CharacterData
    {
        public Vector2 Position;
        public float Speed;
        public Vector2 UnitDirection;
        public float SpeedDownRatio;
        public float SpeedDownAmount;
        public float SpeedUpRatio;
        public float SpeedUpAmount;
    }
}