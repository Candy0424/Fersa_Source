using YIS.Code.Defines;

namespace YIS.Code.Skills.Modules
{
    public class ElementalState
    {
        private Elemental _baseElemental;
        public Elemental CurrentElemental { get; private set; }
        public Elemental BaseElemental => _baseElemental;

        public ElementalState(Elemental elemental)
        {
            _baseElemental = elemental;
            SetElemental(elemental);
        }
        
        public void SetElemental(Elemental elemental) => CurrentElemental = elemental;

        public void ResetElemental() => CurrentElemental = _baseElemental;
    }
}