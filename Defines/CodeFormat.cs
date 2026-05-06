namespace YIS.Code.Defines
{
    public static class CodeFormat
    {
        public static readonly string EnumFormat =
            @"
namespace Work.CSH.Scripts.FSMSystem
{{
    public enum {0}
    {{
        {1}
    }}
}}
";

        public static readonly string SkillEnumFormat =
            @"
namespace Work.YIS.Code.Skills
{{
    public enum {0}
    {{
        {1}
    }}
}}
";

        public static readonly string BuffFormat =
            @"
namespace Work.YIS.Code.Buffs
{{
    public enum {0}
    {{
        {1}
    }}
}}
";

        public static readonly string CSHEnumsFormat =
            @"
namespace Work.CSH.Scripts.Enums
{{
    public enum {0}
    {{
        {1}
    }}
}}
";


    }
}