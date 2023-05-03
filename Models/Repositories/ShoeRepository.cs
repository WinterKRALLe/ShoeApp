namespace ShoeApp.Models.Repositories;

public class ShoeRepository : IShoeRepository
{
    private Context.AppContext _dbContext;
    
    public ShoeRepository(Context.AppContext dbContext)
    {
        _dbContext = dbContext;
    }
    public List<Shoe> FindAll()
    {
        // vyhledání všech filmů, je třeba explicitně načíst relaci Genre, jinak bude null
        return _dbContext.Shoes.ToList();
    }
    public Shoe? FindById(int id)
    {
        // vyhledání jedné entity z databáze pomocí podmínky WHERE
        return _dbContext.Shoes.Where(m => m.Id == id).FirstOrDefault();
    }
    public bool AddShoes(string photoPath, string brand, string model, string size, string yearmodel)
    {
        // "zaevidování" do DBContextu, aby věděla o našem nově vytvořeném objektu
        _dbContext.Shoes.Add(new Shoe()
        {
            Photo = photoPath,
            Brand = brand,
            Model = model,
            Size = size,
            YearModel = yearmodel
        });
        
        // spustí samotné SQL pro uložení do databáze
        if (_dbContext.SaveChanges() == 1)
        {
            return true;
        }

        return false;
    }
    public bool DeleteShoes(Shoe shoe)
    {
        // "oznámení" kontextu o smazání boty
        _dbContext.Remove(shoe);
        // provede samotný SQL příkaz pro smazání
        _dbContext.SaveChanges();

        return true;
    }
    public bool UpdateShoes(Shoe shoe)
    {
        _dbContext.SaveChanges();

        return true;
    }
}