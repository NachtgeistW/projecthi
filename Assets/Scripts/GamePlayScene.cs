using UnityEngine;

namespace Rayark.Hi
{
    using Engine;

    public class GamePlayScene : MonoBehaviour
    {
        [SerializeField]
        private CharacterData _characterData;

        [SerializeField]
        private Transform _characterTransform;

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
        }
    }
}