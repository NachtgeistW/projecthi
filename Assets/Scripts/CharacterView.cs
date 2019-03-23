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
        
        public Vector3 Position
        {
            get
            {
                return _characterTransform.localPosition;
            }

            set
            {
                _characterTransform.localPosition = value;
            }
        }

        public Vector3 Scale
        {
            set
            {
                _characterTransform.localScale = value;
            }
        }

        public void PlayAnimation(AnimationState state)
        {
            _animator.Play(state.ToString());
        }
    }
}