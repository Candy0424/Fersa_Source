using UnityEngine;

namespace YIS.Code.Feedbacks
{
    public abstract class Feedback : MonoBehaviour
    {
        public abstract void PlayFeedback(); 
        public abstract void StopFeedback(); 
    }
}