using Newtonsoft.Json;
namespace YawnMassage.Platform.Domain.Documents.Shared
{
    /// <summary>
    /// Defines the <see cref="MobileNumber" />
    /// </summary>
    public class MobileNumber
    {
        /// <summary>
        /// Gets or sets the IddCode
        /// </summary>
        [JsonProperty("iddcode")]
        public string IddCode { get; set; }

        /// <summary>
        /// Gets or sets the Number
        /// </summary>
        [JsonProperty("number")]
        public string Number { get; set; }
    }
}
