using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace YIS.Code.Items.Editor
{
    [CustomEditor(typeof(DetailDataSO))]
    public class DetailDataEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset view = default;

        private VisualElement _root;
        private VisualElement _priceInfoBox;
        
        private DetailDataSO _target;
        
        public override VisualElement CreateInspectorGUI()
        {
            _target = target as DetailDataSO;
            _root = new VisualElement();
            
            InspectorElement.FillDefaultInspector(_root, serializedObject, this);
            
            view.CloneTree(_root);
            
            _priceInfoBox = _root.Q<VisualElement>("PriceInfoBox");

            RegisterEvent();

            return _root;
        }

        private void RegisterEvent()
        {
            Toggle boolField = _root.Q<Toggle>("BoolField");

            boolField.RegisterValueChangedCallback(itemType => UpdateData());
        }

        private void UpdateData()
        {
            if (_target.useGold)
                _priceInfoBox.style.display = DisplayStyle.Flex;
            else
                _priceInfoBox.style.display = DisplayStyle.None;
            
            AssetDatabase.SaveAssetIfDirty(_target);
        }
    }
}