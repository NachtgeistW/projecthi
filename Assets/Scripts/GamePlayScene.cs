using UnityEngine;

namespace Rayark.Hi
{
    using Engine;

    public class GamePlayScene : MonoBehaviour
    {
        private const float CAMERA_CHARACTER_DIFF_Z = -9;
        
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
            _hiEngine = new HiEngine(_characterData);
            _characterView.PlayAnimation(CharacterView.AnimationState.Run);
        }
        
        void Update()
        {
            _hiEngine.Update(Time.deltaTime);
            
            float characterPositionZ = _hiEngine.CurrentCharacterPosition.y;
            _characterView.UpdateCharacterPositionZ(characterPositionZ);
            _cameraTransform.localPosition = new Vector3(
                _cameraTransform.localPosition.x,
                _cameraTransform.localPosition.y,
                characterPositionZ + CAMERA_CHARACTER_DIFF_Z);
            _planeGenerator.UpdatePlanes(characterPositionZ);

            if(_hiEngine.CurrentCharacterSpeed <= 0f)
            {
                _characterView.PlayAnimation(CharacterView.AnimationState.Idle);
            }
        }
    }
}