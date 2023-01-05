using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BOG
{
    [RequireComponent(typeof(Rect))]
    [RequireComponent(typeof(CanvasGroup))]
    public class UITouchZone : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Button uiBtn;

        public virtual void OnPointerDown(PointerEventData pointerEventData)
        {
            //Debug.Log(name + "Game Object Click in Progress");
        }

        public virtual void OnPointerUp(PointerEventData pointerEventData)
        {
            //Debug.Log(name + "No longer being clicked");
        }
    }
}
