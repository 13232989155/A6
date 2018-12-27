namespace Api.Models
{
    /// <summary>
    /// 分页数据
    /// </summary>
    public class PageData 
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="pageData"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalItemCount"></param>
        public PageData( dynamic pageData, int pageNumber, int pageSize, int totalItemCount)
        {
            Data = pageData;
            TotalItemCount = totalItemCount;
            PageNumber = pageNumber;
            PageSize = pageSize;

            if (pageSize > 0 && pageNumber > 0 && totalItemCount > 0)
            {
                PageCount = (totalItemCount + pageSize - 1) / pageSize;

                if (pageNumber > 1 && PageCount > 2)
                {
                    HasPreviousPage = true;
                }

                if (pageNumber < PageCount)
                {
                    HasNextPage = true;
                }

                if (pageNumber == 1)
                {
                    IsFirstPage = true;
                }

                if (pageNumber == PageCount)
                {
                    IsLastPage = true;
                }

                FirstItemOnPage = ((pageNumber - 1) * pageSize + 1);
                LastItemOnPage = IsLastPage ? totalItemCount : (pageNumber * pageSize);

            }     
        }

        /// <summary>
        /// 
        /// </summary>
        public int PageCount { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalItemCount { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public int PageNumber { get; set ; }

        /// <summary>
        /// 
        /// </summary>
        public int PageSize { get; set ; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasPreviousPage { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasNextPage { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFirstPage { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsLastPage { get; set ; }

        /// <summary>
        /// 
        /// </summary>
        public int FirstItemOnPage { get; set ; }

        /// <summary>
        /// 
        /// </summary>
        public int LastItemOnPage { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public dynamic Data { set; get; }
    }
}
