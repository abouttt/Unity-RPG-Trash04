
public enum SlotType
{
    Item,
    Equipment,
}

public enum EquipmentType
{
    Helmet,
    Chest,
    Pants,
    Boots,
    Weapon,
    Shield,
}

public enum ItemType
{
    Equipment,
    Consumable,
    Miscellaneous,
}

public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
}

public enum UIType
{
    Subitem,
    Background,
    Auto,
    Fixed,
    Popup,
    Top = 1000,
}

public enum SoundType
{
    BGM,
    Effect,
    UI,
}

public enum SceneType
{
    Unknown,
    LoadingScene,
    SampleScene,
}

public enum AddressableLabel
{
    Default,
    Game_Prefab,
    Game_UI,
}
