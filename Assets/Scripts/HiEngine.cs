﻿using UnityEngine;

namespace Rayark.Hi.Engine
{
    public class HiEngine
    {
        private CharacterData _currentCharacter;

        public Vector2 CurrentCharacterPosition
        {
            get
            {
                return _currentCharacter.Position;
            }
        }

        public float CurrentCharacterSpeed
        {
            get
            {
                return _currentCharacter.Speed;
            }
        }

        public HiEngine(CharacterData currentCharacter)
        {
            _currentCharacter = currentCharacter;
        }

        public void Update(float deltaTime)
        {
            _currentCharacter.Position.y += deltaTime * _currentCharacter.Speed;
            _currentCharacter.Speed =
                Mathf.Max(0, (1 - _currentCharacter.SpeedDownRatio * deltaTime) * _currentCharacter.Speed - _currentCharacter.SpeedDownAmount * deltaTime);
        }

        public void SpeedUpCharacterSpeed()
        {
            _currentCharacter.Speed =
                _currentCharacter.Speed * _currentCharacter.SpeedUpRatio + 
                _currentCharacter.SpeedUpAmount;
        }
    }
}