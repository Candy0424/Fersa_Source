using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using YIS.Code.Defines;

namespace YIS.Code.Skills.Editor
{
    [CustomEditor(typeof(SkillDataListSO))]
    public class SkillDataListEditor : UnityEditor.Editor
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
            SkillDataListSO list = target as SkillDataListSO;

            int index = 0;
            string enumString = string.Join(", ", list.skills.Select(so =>
            {
                so.index = index;
                EditorUtility.SetDirty(so);
                return $"{so.skillName.ToUpper().Replace(' ', '_')} = {index++}";
            }));
            
            string codePath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));
            string dirName = Path.GetDirectoryName(codePath);
            DirectoryInfo parentDirectory = Directory.GetParent(dirName);
            string path = parentDirectory.FullName;
            string code = string.Format(CodeFormat.SkillEnumFormat, list.enumName, enumString);
            
            File.WriteAllText($"{path}/{list.enumName}.cs", code);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}