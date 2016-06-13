using System;
using System.Collections;
using System.Collections.Generic;

namespace OG_SLN.ProcedurePagedList
{
    [Serializable]
    public abstract class BaseProcPagedList<T> : ProcPagedListMetadata, IProcPagedList<T>
    {
        /// <summary>
        /// The subset of the items contained only within this one page of the superset.
        /// </summary>
        protected readonly List<T> Subset = new List<T>();

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        protected internal BaseProcPagedList()
        { }

        /// <summary>
        /// Initializes a new instance of a type deriving from ... and sets properties needed to calculate 
        /// position and size data on the subset and superset.
        /// </summary>
        /// <param name="pageNumber">The one-based index of the subset of objects contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <param name="totalItemCount">The size of the superset</param>
        protected internal BaseProcPagedList(int pageNumber, int pageSize, int totalItemCount)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "PageNumber cannot be below 1.");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "PageSize cannot be less than 1.");

            TotalItemCount = totalItemCount;
            PageSize = pageSize;
            PageNumber = pageNumber;
            PageCount = TotalItemCount > 0
                ? (int)Math.Ceiling(TotalItemCount / (double)PageSize)
                : 0;
            HasPreviousPage = PageNumber > 1;
            HasNextPage = PageNumber < PageCount;
            IsFirstPage = PageNumber == 1;
            IsLastPage = PageNumber >= PageCount;
            FirstItemOnPage = (PageNumber - 1) * PageSize + 1;
            var numberOfLastItemOnPage = FirstItemOnPage + PageSize - 1;
            LastItemOnPage = numberOfLastItemOnPage > TotalItemCount
                                ? TotalItemCount
                                : numberOfLastItemOnPage;
        }

        #region IProcPagedList<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return Subset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T this[int index]
        {
            get { return Subset[index]; }
        }

        public int Count
        {
            get { return Subset.Count; }
        }

        public IProcPagedList GetMetaData()
        {
            return new ProcPagedListMetadata(this);
        }

        #endregion
    }
}
