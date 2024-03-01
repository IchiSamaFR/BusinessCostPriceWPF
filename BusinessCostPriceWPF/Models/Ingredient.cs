using BusinessCostPriceWPF.Resources;
using BusinessCostPriceWPF.Services.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace BusinessCostPriceWPF.Models
{
    public partial class Ingredient : ObservableObject, IIngredient, IStock
    {
        [JsonIgnore]
        public string IconString { get; } = "DocumentOnePage24";

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

        public Ingredient Fill(IngredientDTO ingredient)
        {
            Id = ingredient.Id;
            Name = ingredient.Name;
            Unit = ingredient.Unit;
            UnitPrice = ingredient.UnitPrice;
            StockQuantity = ingredient.StockQuantity;
            return this;
        }
        public static Ingredient Build(IngredientDTO ingredient)
        {
            return new Ingredient().Fill(ingredient);
        }
        public static IngredientDTO BuildDTO(Ingredient ingredient)
        {
            return new IngredientDTO()
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Unit = ingredient.Unit,
                UnitPrice = ingredient.UnitPrice,
                StockQuantity = ingredient.StockQuantity
            };
        }
    }
}
