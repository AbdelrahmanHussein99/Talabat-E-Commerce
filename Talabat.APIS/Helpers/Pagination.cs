﻿using Talabat.Core.Entities;

namespace Talabat.APIS.Helpers
{
    public class Pagination<T> 
    {

        public Pagination(int pageIndex, int pageSize,int count,IReadOnlyList<T>data) 
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Count = count;
            Data = data;



        }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }
}
