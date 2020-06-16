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
        private ItemGenerator _speedUpFloorGenerator;

        [SerializeField]
        private ItemGenerator _speedDownFloorGenerator;

        [SerializeField]
        private ItemGenerator _swipeUpItemGenerator;

        [SerializeField]
        private ItemGenerator _swipeDownItemGenerator;

        [SerializeField]
        private Text _distanceText;

        [SerializeField]
        private GameObject _gameOverGroup;

        [SerializeField]
        private Text _gameOverDistanceText;

        private HiEngine _hiEngine;
        private bool _isGameOver;
        private ItemGenerator[] _itemGenerators;

        void Start()
        {
            _itemGenerators = new ItemGenerator[]
            {
                _speedUpFloorGenerator,
                _speedDownFloorGenerator,
                _swipeUpItemGenerator,
                _swipeDownItemGenerator,
            };

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

            _UpdateItemViews();

            var characterPosition = _hiEngine.CurrentCharacterPosition;
            _characterView.Position = new Vector3(
                characterPosition.x,
                _characterView.Position.y,
                characterPosition.y
                );
            _cameraTransform.localPosition = new Vector3(
                _cameraTransform.localPosition.x,
                _cameraTransform.localPosition.y,
                characterPosition.y + CAMERA_CHARACTER_DIFF_Z);
            _planeGenerator.UpdatePlanes(characterPosition.y);

            _UpdateRemainSwipeTimeCountText(_hiEngine.SwipeRemainCount);
            _distanceText.text = ((int)_hiEngine.Distance).ToString() + " m";

            if (!(_hiEngine.CurrentCharacterSpeed <= 0f)) return;
            _characterView.PlayAnimation(CharacterView.AnimationState.Idle);
            _UnregisterEngineEvent();

            _swipeInputHandler.enabled = false;

            _gameOverDistanceText.text = ((int)_hiEngine.Distance).ToString() + " m";
            _gameOverGroup.SetActive(true);
            _isGameOver = true;
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

        private void _UpdateItemViews()
        {
            var speedUpFloorItems = _hiEngine.Items.Where(item => item.Data is SpeedUpFloor);
            _speedUpFloorGenerator.UpdateItems(
                speedUpFloorItems.Select(item => new ItemGenerator.ItemData
                {
                    Position = new Vector3(item.Position.x, 0, item.Position.y),
                    IsUsed = item.IsUsed
                })
                .ToArray());

            var speedDownFloorItems = _hiEngine.Items.Where(item => item.Data is SpeedDownFloor);
            _speedDownFloorGenerator.UpdateItems(
                speedDownFloorItems.Select(item => new ItemGenerator.ItemData
                {
                    Position = new Vector3(item.Position.x, 0, item.Position.y),
                    IsUsed = item.IsUsed
                })
                .ToArray());

            var swipeUpItems = _hiEngine.Items.Where(item => item.Data is SwipeUpItem);
            _swipeUpItemGenerator.UpdateItems(
                swipeUpItems.Select(item => new ItemGenerator.ItemData
                {
                    Position = new Vector3(item.Position.x, 0, item.Position.y),
                    IsUsed = item.IsUsed
                })
                .ToArray());

            var swipeDownItems = _hiEngine.Items.Where(item => item.Data is SwipeDownItem);
            _swipeDownItemGenerator.UpdateItems(
                swipeDownItems.Select(item => new ItemGenerator.ItemData
                {
                    Position = new Vector3(item.Position.x, 0, item.Position.y),
                    IsUsed = item.IsUsed
                })
                .ToArray());
        }

        public void StartGame()
        {
            _hiEngine = new HiEngine(PlaneGenerator.MIN_X_VALUE, PlaneGenerator.MAX_X_VALUE, _xScaleValue, _characterData,
                new IItem[] {
                    new SpeedUpFloor(new Vector2(2, 4)),
                    new SpeedDownFloor(new Vector2(2, 2)),
                    new SwipeUpItem(new Vector2(2, 2)),
                    new SwipeDownItem(new Vector2(2, 2))
                });
            _RegisterEngineEvent();

            _characterView.PlayAnimation(CharacterView.AnimationState.Run);
            _UpdateRemainSwipeTimeCountText(_hiEngine.SwipeRemainCount);
            _planeGenerator.Reset();

            foreach (var itemGenerator in _itemGenerators)
            {
                itemGenerator.ReleaseObjects();
            }

            _swipeUpItemGenerator.SetDisplayUsedItem(false);
            _swipeDownItemGenerator.SetDisplayUsedItem(false);

            _isGameOver = false;
            _gameOverGroup.SetActive(false);
            _swipeInputHandler.enabled = true;
        }
    }
}