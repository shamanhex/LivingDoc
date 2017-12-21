namespace LivingDoc.MDMerge
{
    public enum MDMergeStrategy : int
    {
        /// <summary>
        /// Union all paragraphs.
        /// </summary>
        Union = 0,
        /// <summary>
        /// Only first paragraph will included in result document. All other will ignored.
        /// </summary>
        First = 1,
        /// <summary>
        /// Only first paragraph will included. Second paragraph with same header will throw exception.
        /// </summary>
        SingleOnly = 2
    }
}