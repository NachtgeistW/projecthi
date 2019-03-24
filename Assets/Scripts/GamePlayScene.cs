using System.Linq;
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

        [SerializeField]
        private SpeedUpFloorGenerator _speedUpFloorGenerator;

        [SerializeField]
        private Text _distanceText;

        [SerializeField]
        private GameObject _gameOverGroup;

        [SerializeField]
        private Text _gameOverDistanceText;

        private HiEngine _hiEngine;
        private bool _isGameOver;

        void Start()
        {
            StartGame();
        }

        private void OnDestroy()
        {
            _UnregisterEngineEvent();
        }

        void Update()
        {
            if (_isGameOver)
                return;

            _hiEngine.Update(Time.deltaTime);

            _speedUpFloorGenerator.UpdateSpeedUpFloor(
                _hiEngine.Items.Where(item => item.Data is SpeedUpFloor)
                .Select(item => new Vector3(item.Position.x, 0, item.Position.y))
                .ToArray());

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

            
            _UpdateRemainSwipeTimeCountText(_hiEngine.SwipeRemainCount);
            _distanceText.text = ((int)_hiEngine.Distance).ToString() + " m" ;

            if (_hiEngine.CurrentCharacterSpeed <= 0f)
            {
                _characterView.PlayAnimation(CharacterView.AnimationState.Idle);
                _UnregisterEngineEvent();

                _swipeInputHandler.enabled = false;

                _gameOverDistanceText.text = ((int)_hiEngine.Distance).ToString() + " m";
                _gameOverGroup.SetActive(true);
                _isGameOver = true;
            }
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

        private void _RegisterEngineEvent()
        {
            _swipeInputHandler.OnSwipe += _SpeedUpCharacterSpeed;
        }

        private void _UnregisterEngineEvent()
        {
            _swipeInputHandler.OnSwipe -= _SpeedUpCharacterSpeed;
        }

        public void StartGame()
        {
            _hiEngine = new HiEngine(_xScaleValue, _characterData, new Item[] { new SpeedUpFloor(new Vector2(3, 6)) });
            _RegisterEngineEvent();

            _characterView.PlayAnimation(CharacterView.AnimationState.Run);
            _UpdateRemainSwipeTimeCountText(_hiEngine.SwipeRemainCount);
            _planeGenerator.Reset();
            
            _isGameOver = false;
            _gameOverGroup.SetActive(false);
            _swipeInputHandler.enabled = true;
        }
    }
}