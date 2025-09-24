using Newtonsoft.Json;


namespace Gxt.Net.Advisory
{
    using Newtonsoft.Json.Converters;
    using System.Collections.Generic;

    /// <summary>
    /// Simple meta data for an ID card.
    /// </summary>
    public class IdCard
    {
        /// <summary>
        /// The name the player wants to be displayed as.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Optional opaque data specific to the game.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic? Data { get; set; }
    }


    /// <summary>
    /// Represents a trade order consisting of multiple trade requests.
    /// </summary>
    public class TradeOrder
    {
        /// <summary>
        /// The trade requests contained in this order.
        /// </summary>
        public List<TradeRequest> Requests { get; set; } = new List<TradeRequest>();

        /// <summary>
        /// Whether all requests must be fulfilled together.
        /// </summary>
        public bool AllOrNothing { get; set; }

        /// <summary>
        /// Optional note for the trade order.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Note { get; set; }
    }

    /// <summary>
    /// Represents the response to a trade order.
    /// </summary>
    public class TradeResponse
    {
        /// <summary>
        /// The original trade order.
        /// </summary>
        public TradeOrder Order { get; set; }

        /// <summary>
        /// The trade requests that were executed.
        /// </summary>
        public List<TradeRequest> Trades { get; set; } = new List<TradeRequest>();

        /// <summary>
        /// Optional note explaining the response.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Note { get; set; }
    }

    /// <summary>
    /// Represents a single trade request, with the wanted and offered items.
    /// </summary>
    public class TradeRequest
    {
        /// <summary>
        /// A unique identifier of a trade request.
        /// This makes it easier to match fulfillments to requests.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The wanted items.
        /// </summary>
        public List<Item> Wanted { get; set; } = new List<Item>();

        /// <summary>
        /// The items offered for fulfilling the trade.
        /// </summary>
        public List<Item> Offered { get; set; } = new List<Item>();

        /// <summary>
        /// Optional opaque data specific to the game.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic? Data { get; set; }
    }

    /// <summary>
    /// A tradable item, such as gold, equipment or consumables.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Identifier for the item in the game.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The name of the item that should be shown to the player.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? DisplayName { get; set; }

        /// <summary>
        /// Description of the item.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Description { get; set; }

        /// <summary>
        /// The attributes of the item.
        /// </summary>
        public List<AttributeModifier> Attributes { get; set; } = new List<AttributeModifier>();

        /// <summary>
        /// Quantity of the item.
        /// </summary>
        public uint Amount { get; set; }

        /// <summary>
        /// Optional opaque data specific to the game.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic? Data { get; set; }
    }

    /// <summary>
    /// An attribute that is changed by using or equipping the item.
    /// </summary>
    public class AttributeModifier
    {
        /// <summary>
        /// Identifier for the Attribute in the game.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The name of the attribute that should be shown to the player.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
       public string? DisplayName { get; set; }

        /// <summary>
        /// Amount change for the attribute.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// How the amount should be applied.
        /// </summary>
        public ModifierKind Kind { get; set; }

        /// <summary>
        /// Optional opaque data specific to the game.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic? Data { get; set; }
    }

    /// <summary>
    /// What kind of attribute modifier it is.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ModifierKind
    {
        /// <summary>
        /// Flat increase.
        /// </summary>
        Flat,

        /// <summary>
        /// Percent increase.
        /// </summary>
        Percent
    }

}
