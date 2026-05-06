using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YIS.Code.Feedbacks
{
    public class FeedbackPlayer : MonoBehaviour
    {
        public List<Feedback> feedbacks;

        private void Awake()
        {
            feedbacks = GetComponentsInChildren<Feedback>().ToList();
        }
        
        public void PlayFeedbacks()
        {
            feedbacks.ForEach(f => f.PlayFeedback());
        }

        public void StopFeedbacks()
        {
            feedbacks.ForEach(f => f.StopFeedback());
        }
        
        
        #if UNITY_EDITOR

        [ContextMenu("TestFeedbackPlay")]
        public void TestFeedbackPlay()
        {
           PlayFeedbacks(); 
        }
        
        #endif
    }
}