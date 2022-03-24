using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers
{
    public class InputMoveTouch : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private Vector2 _touchPosition = new Vector2(0f, 0f);
        private bool _touch = false;
    
        public Vector2 position => _touchPosition;
        public bool touch => _touch;

        public void OnPointerDown(PointerEventData eventData)
        {
            _touch = true;
            _touchPosition = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _touchPosition = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _touch = false;
            _touchPosition = eventData.position;
        }
    }
}
