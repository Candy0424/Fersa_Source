using UnityEngine;

namespace YIS.Code.Combat
{
    [CreateAssetMenu(fileName = "New UIPanelData", menuName = "SO/UI/PanelData", order = 0)]
    public class UIPanelDataSO : ScriptableObject
    {
        [field: SerializeField] public string PanelName { get; private set; } = "Panel";
    }
}