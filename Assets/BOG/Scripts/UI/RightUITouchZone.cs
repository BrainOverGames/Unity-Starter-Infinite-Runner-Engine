using UnityEngine.EventSystems;

namespace BOG
{
    public class RightUITouchZone : UITouchZone
    {
        public override void OnPointerDown(PointerEventData pointerEventData)
        {
            base.OnPointerDown(pointerEventData);
            InputManager.Instance.RightButtonDown();
        }

        public override void OnPointerUp(PointerEventData pointerEventData)
        {
            base.OnPointerUp(pointerEventData);
            InputManager.Instance.RightButtonUp();
        }
    }
}
