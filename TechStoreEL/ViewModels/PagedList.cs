using DocumentFormat.OpenXml.Office2010.CustomUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreEL.ViewModels
{
    public class PagedList<T> : List<T>
    {
        private int _pageSize;
        private List<T> _list;
        public PagedList(int pageSize, List<T> data)
        {
            _pageSize = pageSize;
            _list = data;
            DataList = data;
            
        }


        public List<T> DataList { get; set; }
        public int TotalPages
        {
            get
            {
                return Convert.ToInt32(Math.Ceiling(DataList.Count / (decimal)_pageSize));
            }
        }

        public bool NextPage
        {
            get
            {
                return CurrentPage < TotalPages;
            }
        }
        public bool PreviousPage
        {
            get
            {
                return CurrentPage > 1;
            }
        }
        public int CurrentPage { get; set; }
        public List<T> CurrentDataList { get; set; }

        public void GetData(int currentPage)
        {

            if (currentPage > TotalPages) currentPage = TotalPages;
            if (currentPage < 1) currentPage = 1;


            CurrentPage = currentPage;
            CurrentDataList = _list
               .Skip((currentPage - 1) * _pageSize)
               .Take(_pageSize).ToList();

        }


    }
}
