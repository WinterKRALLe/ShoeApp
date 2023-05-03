using Microsoft.AspNetCore.Mvc;
using ShoeApp.Models.Repositories;

namespace ShoeApp.Controllers;

public class ShoeController : Controller
{
    private readonly IShoeRepository _shoeRepository;
    
    public ShoeController(IShoeRepository shoeRepository)
    {
        _shoeRepository = shoeRepository;
    }
    public IActionResult Index()
    {
        // vyhledání botů v databázi a předání do pohledu
        ViewData["Shoes"] = _shoeRepository.FindAll();
        
        // pokud neuvedeme explicitně název pohledu, bude se hledat v Views/{Controller}/{Action}.cshtml
        // tedy Views/Shoe/Index.cshmtl
        return View("Index");
    }
    // Metoda pro přidání bot
    // nikoliv zpracování formuláře, nýbrž pouze pro zobrazení stránky s vytvářecím formulářem
    public IActionResult Add()
    {
        return View("ShoeForm");
    }
    // Metoda pro editaci filmu, má jako argument id filmu, který cheme editovat
    public IActionResult Edit(int id)
    {
        // potřebujeme předvyplnit data do formuláře, tudíž si jej potřebujeme vyhledat
        // správně bychom měli i ošetřit, jestli vůbec bota s daným ID funguje
        ViewData["Shoe"] = _shoeRepository.FindById(id);

        return View("ShoeForm");
    }
    // Metoda pro zpracování formuláře pro přidání filmu
    public IActionResult AddShoes()
    {
        // získání dat z formuláře
        // správně by měla proběhnout ještě validace
        var brand = (string)Request.Form["brand"];
        var model = (string)Request.Form["model"];
        var size = (string)Request.Form["size"];
        var yearmodel = (string)Request.Form["yearmodel"];
        var file = Request.Form.Files[0];
        // cesta kam budeme ukládat nahraný soubor
        // dáváme do složky wwwroot/ která je přístupná "z venku" a dostaneme se k obrázku i z prohlížeče
        var uploadPath = Path.GetRelativePath(".", "wwwroot/upload/" + file.FileName);

        // using automaticky zavolá dispose a uvolní prostředky - netřeba se tím nyní zabývat
        using (var stream = System.IO.File.Create(uploadPath))
        {
            // zkopírování dočasného souboru na námi určené místo
            file.CopyTo(stream);
        }

        // přes repozitář přidáme novou botu
        _shoeRepository.AddShoes(file.FileName, brand, model, size, yearmodel);
        
        // nyní nevracíme pohled(ViewResult), nicméně Result, který provede přesměrování na daný controller a routu
        return new RedirectToRouteResult(new { controller = "Shoe", action = "Index"});
    }
    // metoda pro editaci bot 
    public IActionResult EditShoes()
    {
        var id = int.Parse(Request.Form["id"]);
        var shoe = _shoeRepository.FindById(id);
        if (shoe == null)
            return new RedirectToRouteResult(new { controller = "Home", action = "Index" });
        
        var brand = (string)Request.Form["brand"];
        var model = (string)Request.Form["model"];
        var size = (string)Request.Form["size"];
        var yearmodel = (string)Request.Form["yearmodel"];
        var file = Request.Form.Files?.FirstOrDefault();
        

        shoe.Brand = brand;
        shoe.Model = model;
        shoe.Size = size;
        shoe.YearModel = yearmodel;
        if (file != null)
            shoe.Photo = file.FileName;
        
        _shoeRepository.UpdateShoes(shoe);
        return new RedirectToRouteResult(new { controller = "Shoe", action = "Index" });
    }
    // metoda pro smazání bot
    public IActionResult Delete(int id)
    {
        var shoe = _shoeRepository.FindById(id);

        if (shoe != null) {
            _shoeRepository.DeleteShoes(shoe);
        }

        return new RedirectToRouteResult(new { controller = "Shoe", action = "Index" });
    }
}