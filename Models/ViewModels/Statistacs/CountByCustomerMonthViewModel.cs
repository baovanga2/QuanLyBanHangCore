using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class CountByCustomerMonthViewModel
    {
        [Display(Name = "Tháng")]
        public int Thang { get; set; }
        public List<Month> Months { get; set; }
        [Display(Name = "Năm")]
        public int Nam { get; set; }
        public List<int> Years { get; set; }
        public List<CustomerCount> Customers { get; set; }
        public CountByCustomerMonthViewModel()
        {
            Months = new List<Month>()
            {
                new Month {ID = 1, Ten = "Một"},
                new Month {ID = 2, Ten = "Hai"},
                new Month {ID = 3, Ten = "Ba"},
                new Month {ID = 4, Ten = "Bốn"},
                new Month {ID = 5, Ten = "Năm"},
                new Month {ID = 6, Ten = "Sáu"},
                new Month {ID = 7, Ten = "Bảy"},
                new Month {ID = 8, Ten = "Tám"},
                new Month {ID = 9, Ten = "Chín"},
                new Month {ID = 10, Ten = "Mười"},
                new Month {ID = 11, Ten = "Mười một"},
                new Month {ID = 12, Ten = "Mười hai"},
                new Month {ID = 0, Ten = "Cả năm"}
            };
            var nowy = DateTime.Today.Year;
            Years = new List<int>();
            for (int i = 0; i <= 10; i++)
            {
                Years.Add(nowy - i);
            }
        }
    }
}
