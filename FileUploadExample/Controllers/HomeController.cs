using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Areas.Admin.Data;
using WebApplication1.Areas.Admin.Models;
using WebApplication1.Helpers;
using WebApplication1.Models;

namespace FileUploadExample.Controllers;

public class HomeController : Controller
{
    private readonly AdminDbContext _context;
    private readonly IWebHostEnvironment _env;
    public HomeController(AdminDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public IActionResult Index()
    {

        List<SliderModel> models = _context.Slider.ToList();

        foreach(SliderModel i in models)
        {
            i.ImagePath = Utils.ConvertAbsToURI(_env.WebRootPath, i.ImagePath);
        }


        return View(models);
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}