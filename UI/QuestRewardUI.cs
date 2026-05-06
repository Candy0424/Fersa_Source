using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YIS.Code.Combat
{
    public class QuestRewardUI : MonoBehaviour
    {
        [SerializeField] private Image rewardImage;
        
        public void Initialize(Sprite sprite)
        {
            rewardImage.sprite = sprite;
        }
    }
}