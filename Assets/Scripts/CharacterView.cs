using UnityEngine;

namespace Rayark.Hi
{
    public class CharacterView : MonoBehaviour
    {
        public enum AnimationState
        {
            Idle,
            Run
        }

        [SerializeField]
        private Transform _characterTransform;

        [SerializeField]
        private Animator _animator;
        
        public void UpdateCharacterPositionZ(float z)
        {
            _characterTransform.localPosition = new Vector3(
                _characterTransform.localPosition.x,
                _characterTransform.localPosition.y,
                z
                );
        }

        public void PlayAnimation(AnimationState state)
        {
            _animator.Play(state.ToString());
        }
    }
}