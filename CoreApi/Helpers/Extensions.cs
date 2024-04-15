namespace CoreApi.Helpers
{

    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Append("Application-Error", message);
            response.Headers.Append("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Append("Access-Control-Allow-Origin", "*");
        }

        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            var camelCaseFormatter = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            response.Headers.Append("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
        }

        public static DateTime IndianTime(this DateTime value)
        {
            TimeZoneInfo INDIAN_ZONE = TZConvert.GetTimeZoneInfo("India Standard Time");
            return TimeZoneInfo.ConvertTime(value, TimeZoneInfo.Local, INDIAN_ZONE);
        }

        public static DateTime UTSToDateTime(this double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
    
    [Serializable]
    class CustomException : Exception
    {
        public CustomException() : base(string.Format("Error Occurred")) { }

        public CustomException(string message)
            : base(string.Format("Error: {0}", message))
        {

        }
    }
}