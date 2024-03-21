using BusinessCostPriceAPI.Client.Models;

namespace BusinessCostPriceWPF.Models
{
    public interface IIngredient
    {
        int Id { get; }
        string Name { get; }
        Unit Unit { get; }
        double UnitPrice { get; }
    }
}
