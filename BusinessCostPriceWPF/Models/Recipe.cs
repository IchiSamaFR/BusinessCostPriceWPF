﻿using BusinessCostPriceWPF.Resources;
using BusinessCostPriceWPF.Services;
using BusinessCostPriceWPF.Services.API;
using BusinessCostPriceWPF.ViewModels.Pages.Dashboard;
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
    public partial class Recipe : ObservableObject, IIngredient
    {
        [JsonIgnore]
        public string IconString { get; } = "Notebook24";

        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private Unit _unit;
        [ObservableProperty]
        private double _recipeQuantity;
        [ObservableProperty]
        private double _charges;


        [ObservableProperty]
        private double _unitPrice;
        [ObservableProperty]
        private double _recipePriceNoFee;
        [ObservableProperty]
        private double _recipePrice;


        [ObservableProperty]
        private List<RecipeIngredient> _recipeIngredients;

        public Recipe Fill(RecipeDTO recipe)
        {
            Id = recipe.Id;
            Name = recipe.Name;
            Unit = recipe.Unit;
            RecipeQuantity = recipe.RecipeQuantity;
            Charges = recipe.Charges;
            UnitPrice = recipe.UnitPrice;
            RecipePriceNoFee = recipe.RecipePriceNoFee;
            RecipePrice = recipe.RecipePrice;
            return this;
        }
        public static Recipe Build(RecipeDTO recipe)
        {
            return new Recipe().Fill(recipe);
        }
        public static RecipeDTO BuildDTO(Recipe recipe)
        {
            return new RecipeDTO()
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Unit = recipe.Unit,
                RecipeQuantity = recipe.RecipeQuantity,
                Charges = recipe.Charges,
                UnitPrice = recipe.UnitPrice,
                RecipePriceNoFee = recipe.RecipePriceNoFee,
                RecipePrice = recipe.RecipePrice,
            };
        }
    }
}
