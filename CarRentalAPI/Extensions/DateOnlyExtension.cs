namespace CarRentalAPI.Extensions
{
    public static class DateOnlyExtensions //Add to Extensions folder in CarRentalAPI!
    {
        public static bool IsInRange(this DateOnly now, DateOnly start, DateOnly end)
        {
            return now >= start && now <= end;
        }
    }
}
