public static class ItemTierStringParser
{
    public static string TierToString(ItemTier tier)
    {
        string tierString = "";
        
        switch (tier)
        {
            case ItemTier.Normal:
                tierString = "<color=#FFFFFF>일반</color>";
                break;
            case ItemTier.Rare:
                tierString = "<color=#00FFFF>레어</color>";
                break;
            case ItemTier.Epic:
                tierString = "<color=#8A2BE2>에픽</color>";
                break;
            case ItemTier.Legendary:
                tierString = "<color=#FFD700>레전더리</color>";
                break;
        }
        
        return tierString;
    }
}
