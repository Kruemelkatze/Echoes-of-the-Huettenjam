namespace Extensions
{
    public static class ScriptExtensions
    {
        public static bool RoughlyEquals(this string text1, string text2)
        {
            return text1?.ToLower().Replace(" ", "").Trim() == text2?.ToLower().Replace(" ", "").Trim();
        }
    }
}