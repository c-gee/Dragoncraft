using UnityEngine;
using UnityEngine.UI;

namespace Dragoncraft
{
    [RequireComponent(typeof(Image), typeof(RectTransform))]
    public class UnitSelectorUI : MonoBehaviour
    {
        private RectTransform _transform;
        private Image _selector;
        private Vector3 _startPosition;
        private Vector3 _offset;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
            _selector = GetComponent<Image>();
            _selector.enabled = false;
            _offset = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _selector.enabled = true;
                _startPosition = Input.mousePosition;
                _transform.localPosition = _startPosition - _offset;
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                _transform.localScale = _startPosition - Input.mousePosition;
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                _selector.enabled = false;
            }
        }
    }
}
