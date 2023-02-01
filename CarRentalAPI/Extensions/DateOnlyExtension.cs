namespace CarRentalAPI.Extensions
{
    public static class DateOnlyExtensions
    {
        public static bool IsInRange(this DateOnly now, DateOnly start, DateOnly end) // Is this date beetween 2 dates?
        {
            return now >= start && now <= end;
        }
    }
}
