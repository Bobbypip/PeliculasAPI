using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Helpers
{
    public static class HttpContextExtentions
    {
        public async static Task InsertPaginationParameters<T>(this HttpContext httpContext,
            IQueryable<T> queryable,
            int recordsQuantityPerPage
            )
        {
            double quantity = await queryable.CountAsync();
            double pagesQuantity = Math.Ceiling(quantity / recordsQuantityPerPage);
            httpContext.Response.Headers.Add("pagesQuantity", pagesQuantity.ToString());
        }
    }
}
