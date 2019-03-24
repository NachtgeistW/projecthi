using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField]
        private Text _remainSwipeTimeCountText;

        private HiEngine _hiEngine;

        void Start()
        {
            _hiEngine = new HiEngine(_xScaleValue, _characterData);
            _characterView.PlayAnimation(CharacterView.AnimationState.Run);
            _swipeInputHandler.OnSwipe += _SpeedUpCharacterSpeed;
            _UpdateRemainSwipeTimeCountText(_hiEngine.SwipeRemainCount);
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

            _UpdateRemainSwipeTimeCountText(_hiEngine.SwipeRemainCount);
        }

        private void _SpeedUpCharacterSpeed(Vector2 swipeDirection)
        {
            if (_hiEngine.SwipeRemainCount > 0)
            {
                _hiEngine.SpeedUpCharacterSpeed();
                _hiEngine.ChangeCharacterDirection(swipeDirection);
                _hiEngine.ReduceSwipeRemainCount();
                _UpdateRemainSwipeTimeCountText(_hiEngine.SwipeRemainCount);
            }
        }

        private void _UpdateRemainSwipeTimeCountText(int count)
        {
            _remainSwipeTimeCountText.text = count.ToString();
        }
    }
}