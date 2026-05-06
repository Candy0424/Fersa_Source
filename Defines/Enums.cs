using System;

namespace YIS.Code.Defines
{
    [Flags]
    public enum CheckType
    {
        None = 0,
        Previous = 2,
        Next = 4
    }
    public enum SkillType
    {
        ATTACK, PARTS, ENCHANT
    }
    public enum Grade
    {
        Common, Uncommon, Rare, Epic, Legendary
    }

    public enum ShopItemType
    {
        Item, Skill, Hp
    }

    public enum DetailUIType
    {
        Quit, Restock, Sell
    }

    public enum Elemental
    {
        None, Normal, Pyro, Hydro, Anemo, Cryo, Electro, Dendro
    }
    // Normal : 노말(없음)
    // Pyro : 불
    // Hydro : 물
    // Anemo : 바람
    // Cryo : 얼음
    // Electro : 번개
    // Dendro : 풀

    public enum ItemType
    {
        None = 0, Coin = 1, BossCoin = 2, PP = 3, Item = 4
    }
    
}