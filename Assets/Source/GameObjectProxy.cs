using UnityEngine;

public class GameObjectProxy : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    public GameObject Target => _target;
}
