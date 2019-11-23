namespace QuanLyBanHangCore.Models
{
    public class DetailOrder
    {
        public ulong Gia { get; set; }
        public uint SoLuong { get; set; }
        public int OrderID { get; set; }
        public Order Order { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}