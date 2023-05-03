namespace ShoeApp.Models.Repositories;

public interface IShoeRepository
{
    List<Shoe> FindAll();
    Shoe? FindById(int id);
    bool AddShoes(string photoPath, string brand, string model, string size, string yearmodel);
    bool DeleteShoes(Shoe shoe);
    bool UpdateShoes(Shoe shoe);
}