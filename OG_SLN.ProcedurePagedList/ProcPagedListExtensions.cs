using System;
using System.Collections.Generic;
using System.Linq;

namespace OG_SLN.ProcedurePagedList
{
    public static class ProcPagedListExtensions
    {
        public static IProcPagedList<T> ToProcPagedList<T>(this IEnumerable<T> superset, int pageNumber, int pageSize, int totalItemCount)
        {
            return new ProcPagedList<T>(superset, pageNumber, pageSize, totalItemCount);
        }

        public static IProcPagedList<T> ToProcPagedList<T>(this IQueryable<T> superset, int pageNumber, int pageSize, int totalItemCount)
        {
            return new ProcPagedList<T>(superset, pageNumber, pageSize, totalItemCount);
        }
    }
}
