namespace TheEnchanter.Utils
{
    /// <summary>
    /// Validators used in the mod.
    /// </summary>
    public static class Validators
    {
        /// <summary>
        /// Checks if the object passed as as argument is a valid nuber for a level.
        /// </summary>
        /// <param name="level">object to test.</param>
        /// <returns>true if it's valid or false if it's not.</returns>
        public static bool IsAValidLevel(object level)
        {
            if (level is float || level is short || level is int || level is long || level is double) return true;
            return false;
        }
    }
}