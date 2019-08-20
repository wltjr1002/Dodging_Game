namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    public class TouchControl : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private UIManager uIManager;
        private Vector2 initialTouchPosition;
        public virtual void OnDrag(PointerEventData eventData)
        {
            if(eventData.position.x<initialTouchPosition.x)
            {
                uIManager.SetKeyDowns(1);
                uIManager.SetKeyDowns(-2);
            }
            else
            {
                uIManager.SetKeyDowns(-1);
                uIManager.SetKeyDowns(2);
            }
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            initialTouchPosition = eventData.position;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            initialTouchPosition = Vector2.zero;
            uIManager.SetKeyDowns(-1);
            uIManager.SetKeyDowns(-2);

        }
    }
}


