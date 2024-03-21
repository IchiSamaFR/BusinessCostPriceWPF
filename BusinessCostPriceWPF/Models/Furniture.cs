using BusinessCostPriceAPI.Client.Models;
using Newtonsoft.Json;

namespace BusinessCostPriceWPF.Models
{
    public partial class Furniture : ObservableObject, IStock
    {
        [JsonIgnore]
        public string IconString { get; } = "Box24";

        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private Unit _unit;
        [ObservableProperty]
        private double _unitPrice;
        [ObservableProperty]
        private double _stockQuantity;

        public Furniture Fill(FurnitureDTO furniture)
        {
            Id = furniture.Id;
            Name = furniture.Name;
            Unit = furniture.Unit;
            UnitPrice = furniture.UnitPrice;
            StockQuantity = furniture.StockQuantity;
            return this;
        }
        public static Furniture Build(FurnitureDTO furniture)
        {
            return new Furniture().Fill(furniture);
        }
        public static FurnitureDTO BuildDTO(Furniture furniture)
        {
            return new FurnitureDTO()
            {
                Id = furniture.Id,
                Name = furniture.Name,
                Unit = furniture.Unit,
                UnitPrice = furniture.UnitPrice,
                StockQuantity = furniture.StockQuantity,
            };
        }
    }
}
