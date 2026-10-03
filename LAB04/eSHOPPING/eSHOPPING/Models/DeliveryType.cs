namespace eSHOPPING.Models
{
    public class DeliveryType
    {
        public int DeliveryTypeId { get; set; }
        public string DeliveryTypeName { get; set; }

        public override string ToString()
        {
            return DeliveryTypeName;
        }
    }
}