using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using YIS.Code.Skills;
using Button = UnityEngine.UIElements.Button;

namespace YIS.Code.Skills.Editor
{
    [CustomEditor(typeof(SkillDataSO))]
    public class SkillDataEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset view = default;

        private SkillDataSO _targetSO;
        private VisualElement _root;
        
        public override VisualElement CreateInspectorGUI()
        {
            _targetSO = target as SkillDataSO;
            _root = new VisualElement();
            
            InspectorElement.FillDefaultInspector(_root, serializedObject, this);

            view.CloneTree(_root);
            
            _root.Q<Button>("MakeBtn").clicked += () =>
            {
                serializedObject.ApplyModifiedProperties();
                
                EditorUtility.DisplayDialog("완료", _targetSO.ChainSkill(), "ok");
            };

            LoadSkillDropdown();

            return _root;
        }

        private void LoadSkillDropdown()
        {
            DropdownField skillDropdown = _root.Q<DropdownField>("TargetSkillDropdown");
            skillDropdown.choices.Clear();

            Type skillType = typeof(BaseSkill);
            List<string> loadSkillNames = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(skillType) && t != skillType)
                .Select(t => t.AssemblyQualifiedName)
                .ToList();
            
            skillDropdown.choices.AddRange(loadSkillNames);

            if (!skillDropdown.choices.Contains(_targetSO.className))
            {
                _targetSO.className =
                    skillDropdown.choices.Count > 0 ? skillDropdown.choices.First() : string.Empty;
                EditorUtility.SetDirty(_targetSO);
            }
            AssetDatabase.SaveAssetIfDirty(_targetSO);
        }
    }
}