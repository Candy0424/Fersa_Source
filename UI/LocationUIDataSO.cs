using UnityEngine;

namespace YIS.Code.Combat
{
    [CreateAssetMenu(fileName = "NewLocationData", menuName = "SO/UI/LocationData", order = 0)]
    public class LocationUIDataSO : ScriptableObject
    {
        public string locationName;
        public Sprite directionImage;
    }
}