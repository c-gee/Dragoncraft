using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dragoncraft
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GameObject _miniMapCameraPrefab;

        void Start()
        {
            Instantiate(_miniMapCameraPrefab);
            SceneManager.LoadScene("GameUI", LoadSceneMode.Additive);
        }
    }

}
