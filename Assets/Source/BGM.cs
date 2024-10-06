using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private void OnEnable()
    {
        Game.OnGameStarted += CB_OnGameStarted;
    }

    private void OnDisable()
    {
        Game.OnGameStarted -= CB_OnGameStarted;
    }

    private void CB_OnGameStarted()
    {
        _audioSource.enabled = true;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();
    }
#endif
}
