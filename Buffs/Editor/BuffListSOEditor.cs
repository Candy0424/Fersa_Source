using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using YIS.Code.Defines;

namespace YIS.Code.Modules.Editor
{
    [CustomEditor(typeof(BuffListSO))]
    public class BuffListSOEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset view = default;
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            view.CloneTree(root);

            root.Q<Button>("GenerateButton").clicked += HandleGenerateEnum;
            
            return root;
        }

        private void HandleGenerateEnum()
        {
            BuffListSO list = target as BuffListSO;

            int index = 0;
            string enumString = string.Join(", ", list.buffs.Select(so =>
            {
                so.index = index;
                EditorUtility.SetDirty(so);
                return $"{so.buffEnumName.ToUpper().Replace(' ', '_')} = {index++}";
            }));

            string codePath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));
            string dirName = Path.GetDirectoryName(codePath);
            DirectoryInfo parentDirectory = Directory.GetParent(dirName);
            string path = parentDirectory.FullName;
            string code = string.Format(CodeFormat.BuffFormat, list.enumName, enumString);
            
            File.WriteAllText($"{path}/{list.enumName}.cs", code);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}