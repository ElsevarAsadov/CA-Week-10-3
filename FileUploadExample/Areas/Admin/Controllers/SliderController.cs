using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Text;
using WebApplication1.Areas.Admin.Data;
using WebApplication1.Areas.Admin.Logic;
using WebApplication1.Areas.Admin.Models;
using WebApplication1.Helpers;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AdminDbContext _adminDbContext;
        private readonly IWebHostEnvironment _hostEnviroment;
        public SliderController(AdminDbContext adminDbContext, IWebHostEnvironment hostEnvironment)
        {
            _adminDbContext = adminDbContext;
            _hostEnviroment = hostEnvironment;
        }
        [HttpGet]
        public IActionResult Show()
        {
            List<SliderModel> models = _adminDbContext.Slider.ToList();

            foreach(SliderModel model in models)
            {
                model.ImagePath = Utils.ConvertAbsToURI(_hostEnviroment.WebRootPath, model.ImagePath) ;
            }



            return View(models);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            SliderModel? model = _adminDbContext.Slider.FirstOrDefault(x => x.Id == id);

            if (model == null) return NotFound();

            _adminDbContext.Slider.Remove(model);

            try
            {
                FormFileManager.DeleteFile(model.ImagePath);
            }
            catch(Exception exp)
            {
                Console.WriteLine($"Exception occured when deleting slider image.Exception: {exp.Message}");
                return RedirectToAction("Show");
            }
            _adminDbContext.SaveChanges();

            return RedirectToAction("Show");
        }

        [HttpPost]
        public IActionResult Create(SliderModel sliderModel)
        {

            if (ModelState.IsValid)
            {
                bool isValid = true;

                if (!new string[] {"image/jpeg", "image/png" }.Contains(sliderModel.ImageFile.ContentType))
                {
                    ModelState.AddModelError("ImageFile", "Only jpg and png formats supported");
                    isValid = false;
                }
                //byte to mb
                if(! (Utils.ByteToMb(sliderModel.ImageFile.Length) > 2) ) {
                    ModelState.AddModelError("ImageFile", "File size is exceeded 2mb limit");
                    isValid = false;
                }


                if (!isValid)
                {
                    return View();
                }

                //MAX_FILE_SIZE:20 + HELPER_CHARS:4 + EXTENTION_LENGTH:any +  GUID_SIZE:36 -> =~ 63(if extension length = 3)
                string guidName = FormFileManager.ConvertUniqueName(sliderModel.ImageFile.FileName, 20);
                string absPath = Path.Combine(_hostEnviroment.WebRootPath, "images", guidName);
                FormFileManager.SaveFile(sliderModel.ImageFile, absPath, FileMode.Create) ;

                sliderModel.ImagePath = absPath;
                _adminDbContext.Slider.Add(sliderModel);
                _adminDbContext.SaveChanges();
                
                
                return RedirectToAction("Show");
            }

            else
            {
                return View();
            }



        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            SliderModel? model = _adminDbContext.Slider.FirstOrDefault(x=>x.Id == id);

            if(model == null)
            {
                return NotFound();
            };

            return View(model);

        }

        [HttpPost]
        public IActionResult Update(SliderModel? newModel)
        {
            if (newModel == null)
            {
                return View();
            }

            SliderModel? oldModel = _adminDbContext.Slider.FirstOrDefault(x => x.Id == newModel.Id);

            if (oldModel == null)
            {
                return View();
            }


            if(newModel.ImageFile == null)
            {
                return View();
            }



            //MAX_FILE_SIZE:20 + HELPER_CHARS:4 + EXTENTION_LENGTH:any +  GUID_SIZE:36 -> =~ 63(if extension length = 3)
            string guidName = FormFileManager.ConvertUniqueName(newModel.ImageFile.FileName, 20);
            string absPath = Path.Combine(_hostEnviroment.WebRootPath, "images", guidName);

            newModel.ImagePath = absPath;


            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                FormFileManager.DeleteFile(oldModel.ImagePath);
            }
            catch (Exception exp)
            {
                //can be server logger
                Console.WriteLine($"Exception occured when deleting file from server.Exception: {exp.Message}");
                return StatusCode(505);
            }


            try
            {
                FormFileManager.SaveFile(newModel.ImageFile, absPath, FileMode.Create);
            }
            catch (Exception exp)
            {
                //can be server logger
                Console.WriteLine($"Exception occured when saving file from server.Exception: {exp.Message}");
                return StatusCode(505);
            }

            oldModel.UpperText = newModel.UpperText;
            oldModel.BottomText = newModel.BottomText;
            oldModel.ButtonText = newModel.ButtonText;
            oldModel.ImagePath = newModel.ImagePath;


            _adminDbContext.SaveChanges();

            return RedirectToAction("Show");


            

        }
    }
}
