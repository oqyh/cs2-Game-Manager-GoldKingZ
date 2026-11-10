namespace Game_Manager_GoldKingZ;
internal static class CommandRules
{
    internal static bool Matches(string command, IReadOnlyList<string> prefixes, bool prefixIgnoreCase,
        IReadOnlyList<string> fragments, bool fragmentIgnoreCase)
    {
        var prefixComparison = prefixIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        var fragmentComparison = fragmentIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        for (var i = 0; i < prefixes.Count; i++) if (command.StartsWith(prefixes[i], prefixComparison)) return true;
        for (var i = 0; i < fragments.Count; i++) if (command.Contains(fragments[i], fragmentComparison)) return true;
        return false;
    }
}
