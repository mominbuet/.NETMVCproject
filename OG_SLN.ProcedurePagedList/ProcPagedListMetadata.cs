using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OG_SLN.ProcedurePagedList
{
    /// <summary>
    /// Non-enumerable version of the PagedList class
    /// </summary>
    [Serializable]
    public class ProcPagedListMetadata : IProcPagedList
    {
        /// <summary>
        /// Protected constructor that allows for instantiation without passing in a separate list.
        /// </summary>
        protected ProcPagedListMetadata()
        { }

        public ProcPagedListMetadata(IProcPagedList pagedList)
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
        }

        #region IPagedList Members

        public int PageCount { get; protected set; }

        public int TotalItemCount { get; protected set; }

        public int PageNumber { get; protected set; }

        public int PageSize { get; protected set; }

        public bool HasPreviousPage { get; protected set; }

        public bool HasNextPage { get; protected set; }

        public bool IsFirstPage { get; protected set; }

        public bool IsLastPage { get; protected set; }

        public int FirstItemOnPage { get; protected set; }

        public int LastItemOnPage { get; protected set; }

        #endregion
    }
}
