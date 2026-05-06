using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YIS.Code.Combat;

namespace Work.YIS.Code.UI.Hall
{
    public class HallLocationInfoUI : MonoBehaviour
    {
        [SerializeField] private LocationUIDataSO locationDataSO;
        
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image directionImage;

        private void Awake()
        {
            LoadUIData();
        }

        private void LoadUIData()
        {
            nameText.SetText(locationDataSO.locationName); 
            directionImage.sprite = locationDataSO.directionImage;
        }
    }
}