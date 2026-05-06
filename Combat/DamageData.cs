using YIS.Code.Defines;

namespace YIS.Code.Combat
{
    public struct DamageData
    {
        public float Damage;
        public Elemental ElementalType;

        public DamageData(float damage, Elemental elementalType = Elemental.Normal)
        {
            Damage = damage;
            ElementalType = elementalType;
        }
    }
}