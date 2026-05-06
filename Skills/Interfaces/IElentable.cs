using YIS.Code.Defines;
using YIS.Code.Skills.Modules;

namespace YIS.Code.Skills.Interfaces
{
    public interface IElementalable
    {
        ElementalState CurrentElementalState { get; }
        void ElementalCal();
    }
}