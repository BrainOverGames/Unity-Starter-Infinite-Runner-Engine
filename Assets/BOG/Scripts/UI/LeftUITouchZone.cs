using UnityEngine.EventSystems;

namespace BOG
{
    public class LeftUITouchZone :UITouchZone
    {
        public override void OnPointerDown(PointerEventData pointerEventData)
        {
            base.OnPointerDown(pointerEventData);
            InputManager.Instance.LeftButtonDown();
        }

        public override void OnPointerUp(PointerEventData pointerEventData)
        {
            base.OnPointerUp(pointerEventData);
            InputManager.Instance.LeftButtonUp();
        }
    }
}
