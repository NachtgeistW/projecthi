using UnityEngine;

namespace Rayark.Hi
{
    using Engine;

    public class GamePlayScene : MonoBehaviour
    {
        private const float CAMERA_CHARACTER_DIFF_Z = -10;


        [SerializeField]
        private CharacterData _characterData;

        [SerializeField]
        private Transform _characterTransform;

        [SerializeField]
        private Transform _cameraTransform;

        private HiEngine _hiEngine;

        void Start()
        {
            _hiEngine = new HiEngine(_characterData);
        }
        
        void Update()
        {
            _hiEngine.Update(Time.deltaTime);

            _characterTransform.localPosition = new Vector3(
                _characterTransform.localPosition.x,
                _characterTransform.localPosition.y,
                _hiEngine.CurrentCharacterPosition.y);
            _cameraTransform.localPosition = new Vector3(
                _cameraTransform.localPosition.x,
                _cameraTransform.localPosition.y,
                _hiEngine.CurrentCharacterPosition.y + CAMERA_CHARACTER_DIFF_Z);
        }
    }
}