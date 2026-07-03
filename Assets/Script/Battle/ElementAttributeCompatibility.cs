public static class ElementAttributeCompatibility
{
    public static bool IsWeak(ElementAttribute attacker, ElementAttribute defender)
    {
        return (attacker == ElementAttribute.Fire && defender == ElementAttribute.Wood)
            || (attacker == ElementAttribute.Water && defender == ElementAttribute.Fire)
            || (attacker == ElementAttribute.Wood && defender == ElementAttribute.Water);
    }

    public static bool IsResist(ElementAttribute attacker, ElementAttribute defender)
    {
        return (attacker == ElementAttribute.Fire && defender == ElementAttribute.Water)
            || (attacker == ElementAttribute.Water && defender == ElementAttribute.Wood)
            || (attacker == ElementAttribute.Wood && defender == ElementAttribute.Fire);
    }
}
