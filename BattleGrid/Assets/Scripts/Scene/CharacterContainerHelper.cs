using UnityEngine;

public class CharacterContainerHelper : MonoBehaviour
{
    [SerializeField]
    private SimpleCharacterView[] _charactersInView;

    public SimpleCharacterView[] CharactersInScene { get { return _charactersInView; } }

    private void Awake()
    {
        if (_charactersInView == null || _charactersInView.Length <= 0)
        {
            _charactersInView = gameObject.GetComponentsInChildren<SimpleCharacterView>();
        }
    }
}
