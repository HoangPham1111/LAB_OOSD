namespace eSHOPPING.Models
{
    public class DeliveryArea
    {
        public int DeliveryAreaId { get; set; }
        public string AreaName { get; set; }

        public override string ToString()
        {
            return AreaName;
        }
    }
}