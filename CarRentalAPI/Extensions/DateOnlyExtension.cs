namespace CarRentalAPI.Extensions
{
    public static class DateOnlyExtensions
    {
        /// <summary>
        /// Checks whether given date is between two other dates
        /// </summary>
        /// <param name="now"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>true if given date happens to be in range of two other dates</returns>
        public static bool IsInRange(this DateOnly now, DateOnly start, DateOnly end)
        {
            return now >= start && now <= end;
        }
    }
}
