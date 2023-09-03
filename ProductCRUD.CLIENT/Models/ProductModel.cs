namespace ProductCRUD.CLIENT.Models;

public class ProductModel
{
    public int tenantId { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public bool isAvailable { get; set; }
    public int id { get; set; }
}