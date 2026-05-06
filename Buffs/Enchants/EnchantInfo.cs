using YIS.Code.Defines;

namespace YIS.Code.Buffs.Enchants
{
    public class EnchantInfo
    {
        private Elemental _elemental;
        private int _duration;
        
        public bool IsEnd => _duration <= 0;
        public bool IsValid => _elemental != Elemental.None;
        public Elemental Elemental => _elemental;

        public EnchantInfo(Elemental elemental, int duration)
        {
            _elemental = elemental;
            _duration = duration;
        }

        public void EnchantAction(Elemental newElemental, int duration)
        {
            _duration = duration;
            _elemental = newElemental;
        }

        public void ResetEnchantInfo() => EnchantAction(Elemental.None, 0);

        public void UpdateEnchantInfo()
        {
            _duration--;
        }
    }
}