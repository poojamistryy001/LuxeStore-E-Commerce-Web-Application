namespace LuxeStore.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCustomers { get; set; }

        // ORDER STATUS
        public int PendingOrders { get; set; }
        public int ProcessingOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int CancelledOrders { get; set; }

        public IEnumerable<RecentOrderRow> RecentOrders { get; set; } = new List<RecentOrderRow>();
        public List<TopProductRow> TopProducts { get; set; } = new();
    }

    public class RecentOrderRow
    {
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }

    public class TopProductRow
    {
        public string Name { get; set; } = "";
        public int SoldQuantity { get; set; }
    }
}