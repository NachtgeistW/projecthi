using UnityEngine;

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

        public HiEngine(CharacterData currentCharacter)
        {
            _currentCharacter = currentCharacter;
        }

        public void Update(float deltaTime)
        {
            _currentCharacter.Position.y += deltaTime * _currentCharacter.Speed;
        }
    }
}