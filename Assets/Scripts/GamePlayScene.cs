using UnityEngine;

namespace Rayark.Hi
{
    using Engine;

    public class GamePlayScene : MonoBehaviour
    {
        private const float CAMERA_CHARACTER_DIFF_Z = -9;

        [SerializeField]
        private float _xScaleValue;

        [SerializeField]
        private SwipeInputHandler _swipeInputHandler;

        [SerializeField]
        private CharacterData _characterData;

        [SerializeField]
        private CharacterView _characterView;

        [SerializeField]
        private Transform _cameraTransform;

        [SerializeField]
        private PlaneGenerator _planeGenerator;

        private HiEngine _hiEngine;

        void Start()
        {
            _hiEngine = new HiEngine(_xScaleValue, _characterData);
            _characterView.PlayAnimation(CharacterView.AnimationState.Run);
            _swipeInputHandler.OnSwipe += _SpeedUpCharacterSpeed;
        }

        private void OnDestroy()
        {
            _swipeInputHandler.OnSwipe -= _SpeedUpCharacterSpeed;
        }

        void Update()
        {
            _hiEngine.Update(Time.deltaTime);
            
            var characterPosition = _hiEngine.CurrentCharacterPosition;
            _characterView.Position = new Vector3(
                characterPosition.x * (PlaneGenerator.MAX_X_VALUE - PlaneGenerator.MIN_X_VALUE) + PlaneGenerator.MIN_X_VALUE,
                _characterView.Position.y,
                characterPosition.y
                );
            _cameraTransform.localPosition = new Vector3(
                _cameraTransform.localPosition.x,
                _cameraTransform.localPosition.y,
                characterPosition.y + CAMERA_CHARACTER_DIFF_Z);
            _planeGenerator.UpdatePlanes(characterPosition.y);

            if(_hiEngine.CurrentCharacterSpeed <= 0f)
            {
                _characterView.PlayAnimation(CharacterView.AnimationState.Idle);
            }
        }

        private void _SpeedUpCharacterSpeed(Vector2 swipeDirection)
        {
            _hiEngine.SpeedUpCharacterSpeed();
            _hiEngine.ChangeCharacterDirection(swipeDirection);
        }
    }
}