namespace M03__Repository_Pattern.Model;

public class Product
{

    public Guid Id { get; set; }
    public required String Name { get; set; }
    
    public decimal Price{ get; set; }
}