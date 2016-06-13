using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OG_SLN.ProcedurePagedList
{
    [Serializable]
    public class ProcPagedList<T> : BaseProcPagedList<T>
    {
        public ProcPagedList(IQueryable<T> superset, int pageNumber, int pageSize, int totalItemCount)
        {
            if (pageNumber < 1)
				throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "PageNumber cannot be below 1.");
			if (pageSize < 1)
				throw new ArgumentOutOfRangeException("pageSize", pageSize, "PageSize cannot be less than 1.");

            // set source to blank list if superset is null to prevent exceptions
            TotalItemCount = superset == null ? 0 : totalItemCount;
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

            // add items to internal list
            if (superset != null && TotalItemCount > 0)
                Subset.AddRange(pageNumber == 1
                    ? superset.Skip(0).Take(pageSize).ToList()
                    //: superset.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList()
                    : superset.Skip(0).Take(pageSize).ToList()
                );
        }

        public ProcPagedList(IEnumerable<T> superset, int pageNumber, int pageSize, int totalItemCount)
			: this(superset.AsQueryable<T>(), pageNumber, pageSize, totalItemCount)
		{
		}
    }
}
