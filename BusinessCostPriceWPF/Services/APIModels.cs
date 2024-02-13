using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCostPriceWPF.Services.API
{
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.3.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class AuthenticateDTO
    {
        [Newtonsoft.Json.JsonProperty("email", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Email { get; set; }

        [Newtonsoft.Json.JsonProperty("password", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Password { get; set; }

        [Newtonsoft.Json.JsonProperty("token", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Token { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.3.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class AuthenticateUpdateDTO
    {
        [Newtonsoft.Json.JsonProperty("password", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Password { get; set; }

        [Newtonsoft.Json.JsonProperty("newPassword", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string NewPassword { get; set; }

        [Newtonsoft.Json.JsonProperty("token", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Token { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.3.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class FurnitureDTO
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("name", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("unit", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public Unit Unit { get; set; }

        [Newtonsoft.Json.JsonProperty("unitPrice", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double UnitPrice { get; set; }

        [Newtonsoft.Json.JsonProperty("stockQuantity", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double StockQuantity { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.3.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class FurnitureStockInfoDTO
    {
        [Newtonsoft.Json.JsonProperty("furnitureId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int FurnitureId { get; set; }

        [Newtonsoft.Json.JsonProperty("date", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset Date { get; set; }

        [Newtonsoft.Json.JsonProperty("stockQuantity", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double StockQuantity { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.3.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class IIngredient
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("name", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("unit", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public Unit Unit { get; set; }

        [Newtonsoft.Json.JsonProperty("unitPrice", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double UnitPrice { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.3.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class IngredientDTO
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("name", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("unit", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public Unit Unit { get; set; }

        [Newtonsoft.Json.JsonProperty("unitPrice", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double UnitPrice { get; set; }

        [Newtonsoft.Json.JsonProperty("stockQuantity", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double StockQuantity { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.3.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class IngredientPriceInfoDTO
    {
        [Newtonsoft.Json.JsonProperty("ingredientId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int IngredientId { get; set; }

        [Newtonsoft.Json.JsonProperty("date", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset Date { get; set; }

        [Newtonsoft.Json.JsonProperty("unitPrice", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double UnitPrice { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.3.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class IngredientStockInfoDTO
    {
        [Newtonsoft.Json.JsonProperty("ingredientId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int IngredientId { get; set; }

        [Newtonsoft.Json.JsonProperty("date", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset Date { get; set; }

        [Newtonsoft.Json.JsonProperty("stockQuantity", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double StockQuantity { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.3.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class RecipeDTO
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("name", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("unit", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public Unit Unit { get; set; }

        [Newtonsoft.Json.JsonProperty("recipeQuantity", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double RecipeQuantity { get; set; }

        [Newtonsoft.Json.JsonProperty("charges", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double Charges { get; set; }

        [Newtonsoft.Json.JsonProperty("unitPrice", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double UnitPrice { get; set; }

        [Newtonsoft.Json.JsonProperty("recipePriceNoFee", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double RecipePriceNoFee { get; set; }

        [Newtonsoft.Json.JsonProperty("recipePrice", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double RecipePrice { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.3.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class RecipeIngredientDTO
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("quantity", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double Quantity { get; set; }

        [Newtonsoft.Json.JsonProperty("recipeId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int RecipeId { get; set; }

        [Newtonsoft.Json.JsonProperty("ingredientRecipeId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int IngredientRecipeId { get; set; }

        [Newtonsoft.Json.JsonProperty("ingredientId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int IngredientId { get; set; }

        [Newtonsoft.Json.JsonProperty("iIngredient", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public IIngredient IIngredient { get; set; }

        [Newtonsoft.Json.JsonProperty("price", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double Price { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.3.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum Unit
    {

        [System.Runtime.Serialization.EnumMember(Value = @"kilogram")]
        Kilogram = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"liter")]
        Liter = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"piece")]
        Piece = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"dozen")]
        Dozen = 3,
    }
}
