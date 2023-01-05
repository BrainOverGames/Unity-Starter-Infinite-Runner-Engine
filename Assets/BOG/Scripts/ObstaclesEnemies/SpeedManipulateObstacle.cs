using UnityEngine;

namespace BOG
{
    public class SpeedManipulateObstacle : MonoBehaviour
    {
        public float speedManipulateFactor;
        public float duration;

        public void ManipulateCharSpeed()
        {
            LevelManager.Instance.TemporarilyMultiplySpeed(speedManipulateFactor, duration);
        }
    }
}
