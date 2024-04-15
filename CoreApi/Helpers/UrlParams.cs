namespace CoreApi.Helpers
{

    public class UrlParams
    {
        private const int MaxPageSize = 60;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 30;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        public int OrderBy { get; set; } = 0;

    }
}