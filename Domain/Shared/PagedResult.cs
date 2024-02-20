using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared
{
    public class PagedResult<T>
    {
        public List<T> Data { get; set; }
        public int TotalItems { get; set; }

        public PagedResult(List<T> data, int totalItems)
        {
            Data = data;
            TotalItems = totalItems;
        }
    }
}
