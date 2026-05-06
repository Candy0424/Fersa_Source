using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace YIS.Code.Items.Editor
{
    [CustomEditor(typeof(ItemVisualDataSO))]
    public class ItemVisualEditor : UnityEditor.Editor
    {
        private SerializedProperty _itemName;
        private ItemVisualDataSO _itemData;
        private void OnEnable()
        {
            _itemName = serializedObject.FindProperty("itemName");
            _itemData = (ItemVisualDataSO)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUI.BeginChangeCheck();
            string newName = EditorGUILayout.DelayedTextField("itemName", _itemData.itemName);

            if (EditorGUI.EndChangeCheck())
            {
                _itemName.stringValue = newName;
                serializedObject.ApplyModifiedProperties();
                UpdateName();
            }

            DrawPropertiesExcluding(serializedObject, "m_Script", "itemName");
            
            if (GUI.changed) serializedObject.ApplyModifiedProperties();
            
            
        }

        private void UpdateName()
        {
            string path = AssetDatabase.GetAssetPath(_itemData);
            string oldFileName = System.IO.Path.GetFileNameWithoutExtension(path);
            string newFileName = _itemData.itemName;
            
            
            if (oldFileName != newFileName)
            {
                
                string result = AssetDatabase.RenameAsset(path, $"{newFileName}_visual");

                Debug.Assert(string.IsNullOrEmpty(result), $"아이템이름 변경 실패 : {oldFileName}->{newFileName}");
            }
        }

        public override VisualElement CreateInspectorGUI()
        {
            return base.CreateInspectorGUI();
        }
    }
}