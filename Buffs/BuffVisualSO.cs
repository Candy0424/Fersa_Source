using CSH.Scripts.Attributes;
using UnityEngine;

namespace YIS.Code.Modules
{
    [CreateAssetMenu(fileName = "New Buff VisualData", menuName = "SO/Buff/VisualData", order = 0)]
    public class BuffVisualSO : ScriptableObject
    {
        public string buffName;
        [SpritePreview(50)] public Sprite buffIcon;
        [TextArea] public string buffDescription;
    }
}