using net_registri_log.Shared.ExtensionMethods;
using System.Collections.Generic;
using System.Linq;

namespace net_registri_log.Shared.Models
{
    public class PagedList<T>
	{
		public PaginationMetadata Pagination { get; private set; }
		public IEnumerable<T> Data { get; private set; }

		public PagedList(IEnumerable<T> data, QueryParameters queryParameters, int count)
		{
			Pagination = new PaginationMetadata(
				queryParameters.Page,
				(int)queryParameters.GetTotalPages(count),
				queryParameters.PageCount,
				count);

			Data = data;
		}

		public static PagedList<T> ToPagedList(IQueryable<T> source, QueryParameters queryParameters)
		{
			int count = source.Count();
			IEnumerable<T> items = source
					.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
					.Take(queryParameters.PageCount)
					.ToList();

			return new PagedList<T>(items, queryParameters, count);
		}
	}

	public class PaginationMetadata
	{
		public PaginationMetadata(int currentPage, int totalPages, int pageSize, int totalCount)
		{
			CurrentPage = currentPage;
			TotalPages = totalPages;
			PageSize = pageSize;
			TotalCount = totalCount;
		}

		public int CurrentPage { get; private set; }
		public int TotalPages { get; private set; }
		public int PageSize { get; private set; }
		public int TotalCount { get; private set; }

		public bool HasPrevious => CurrentPage > 1;
		public bool HasNext => CurrentPage < TotalPages;
	}
}
