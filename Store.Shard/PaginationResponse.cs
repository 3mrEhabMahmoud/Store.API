using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Shard
{
    public class PaginationResponse<TEntity>
    {
        public PaginationResponse(int pageIndex,int pageSize ,int count ,IEnumerable<TEntity> data)
        {
            pageIndex = pageIndex;
            pageSize = pageSize;
            Data = data;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IEnumerable<TEntity> Data { get; set; }

    }
}
