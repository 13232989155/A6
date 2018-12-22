using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

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
        /// <param name="pagedList"></param>
        public PageData( IPagedList pagedList)
        {

            PageCount = pagedList.PageCount;
            TotalItemCount = pagedList.TotalItemCount;
            PageNumber = pagedList.PageNumber;
            PageSize = pagedList.PageSize;
            HasPreviousPage = pagedList.HasPreviousPage;
            HasNextPage = pagedList.HasNextPage;
            IsFirstPage = pagedList.IsFirstPage;
            IsLastPage = pagedList.IsLastPage;
            FirstItemOnPage = pagedList.FirstItemOnPage;
            LastItemOnPage = pagedList.LastItemOnPage;
            Data = pagedList;
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
