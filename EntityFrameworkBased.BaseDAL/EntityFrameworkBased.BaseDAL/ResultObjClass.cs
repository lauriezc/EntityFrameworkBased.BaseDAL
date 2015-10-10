using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityFrameworkBased.BaseDAL
{
    public class ResultObjClass
    {
        public ResultObjClass()
        {
            PageSize = 15;
            PageIndex = 1;
        }

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { set; get; }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageIndex { set; get; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int PageTotalCount { set; get; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (PageTotalCount <= 0) return 0;
                return PageTotalCount % PageSize == 0
                    ? PageTotalCount / PageSize
                    : (int)Math.Ceiling(((float)PageTotalCount / PageSize));
            }
        }

        public object PageObject { set; get; }
    }
}
