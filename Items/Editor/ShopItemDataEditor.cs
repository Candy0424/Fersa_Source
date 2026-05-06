using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using YIS.Code.Defines;

namespace YIS.Code.Items.Editor
{
    [CustomEditor(typeof(ShopItemDataSO))]
    public class ShopItemDataEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset view = default;
        
        private ShopItemDataSO _target; 
        private VisualElement _root;
        private VisualElement _itemInfoBox;
        private VisualElement _skillInfoBox;
        private VisualElement _hpInfoBox;
        
        public override VisualElement CreateInspectorGUI()
        {
            _target = target as ShopItemDataSO;
            _root = new VisualElement();
            
            InspectorElement.FillDefaultInspector(_root, serializedObject, this);

            view.CloneTree(_root);
            _itemInfoBox = _root.Q<VisualElement>("ItemInfoBox");
            _skillInfoBox = _root.Q<VisualElement>("SkillInfoBox");
            _hpInfoBox = _root.Q<VisualElement>("HpInfoBox");

            RegisterEvent();

            return _root;
        }

        private void RegisterEvent()
        {
            EnumField enumField = _root.Q<EnumField>("ShopItemEnum");

            enumField.RegisterValueChangedCallback(itemType => UpdateField());
        }

        private void UpdateField()
        {
            switch (_target.shopItemType)
            {
                case ShopItemType.Item:
                    _itemInfoBox.style.display = DisplayStyle.Flex;
                    _skillInfoBox.style.display = DisplayStyle.None;
                    _hpInfoBox.style.display = DisplayStyle.None;
                    break;
                case ShopItemType.Skill:
                    _itemInfoBox.style.display = DisplayStyle.None;
                    _skillInfoBox.style.display = DisplayStyle.Flex;
                    _hpInfoBox.style.display = DisplayStyle.None;
                    break;
                case ShopItemType.Hp:
                    _itemInfoBox.style.display = DisplayStyle.None;
                    _skillInfoBox.style.display = DisplayStyle.None;
                    _hpInfoBox.style.display = DisplayStyle.Flex;
                    break;
            }
            
            AssetDatabase.SaveAssetIfDirty(_target);
        }
    }
}