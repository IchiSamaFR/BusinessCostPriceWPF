using BusinessCostPriceAPI.Client.Models;

namespace BusinessCostPriceWPF.Models
{
    public interface IStock
    {
        int Id { get; }
        string Name { get; }
        Unit Unit { get; }
        double UnitPrice { get; }
        double StockQuantity { get; set; }
    }
}
