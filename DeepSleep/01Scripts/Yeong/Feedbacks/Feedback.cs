using UnityEngine;

namespace YH.Feedbacks
{
    public abstract class Feedback : MonoBehaviour
    {
        public abstract void CreateFeedback();
        public abstract void FinishFeedback();
    }
}
