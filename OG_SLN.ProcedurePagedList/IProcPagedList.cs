using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OG_SLN.ProcedurePagedList
{
    public interface IProcPagedList<out T> : IProcPagedList, IEnumerable<T>
    {
        T this[int index] { get; }

        int Count { get; }

        IProcPagedList GetMetaData();
    }

    public interface IProcPagedList
    {
        int PageCount { get; }

        int TotalItemCount { get; }

        int PageNumber { get; }

        int PageSize { get; }

        bool HasPreviousPage { get; }

        bool HasNextPage { get; }

        bool IsFirstPage { get; }

        bool IsLastPage { get; }

        int FirstItemOnPage { get; }

        int LastItemOnPage { get; }
    }
}
