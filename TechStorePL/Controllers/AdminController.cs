using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Net;
using TechStoreBL.InterfacesOfManagers;
using TechStoreEL.Entities;
using TechStoreEL.IdentityModels;
using TechStoreUL;
using TechStoreEL.ViewModels;
using TechStorePL.Models;
using System.Text.Json.Serialization.Metadata;
using TechStoreEL.ResultModels;
using Azure;

namespace TechStorePL.Controllers
{
    public class AdminController : Controller
    {
        private readonly IOrderManager _orderManager;
        private readonly IOrderDetailManager _orderDetailManager;
        private readonly IProductManager _productManager;
        private ILogger<AccountController> _logger;
        private readonly ICategoryProductPropertyManager _categoryProductPropertyManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IProductPictureManager _productPictureManager;
        private readonly IProductPropertyManager _productPropertyManager;

        public AdminController(IOrderManager orderManager, IOrderDetailManager orderDetailManager, IProductManager productManager, ILogger<AccountController> logger, ICategoryProductPropertyManager categoryProductPropertyManager, ICategoryManager categoryManager, IProductPictureManager productPictureManager, IProductPropertyManager productPropertyManager)
        {
            _orderManager = orderManager;
            _orderDetailManager = orderDetailManager;
            _productManager = productManager;
            _logger = logger;
            _categoryProductPropertyManager = categoryProductPropertyManager;
            _categoryManager = categoryManager;
            _productPictureManager = productPictureManager;
            _productPropertyManager = productPropertyManager;
        }

        public IActionResult Index(int currentpage = 1)
        {

            ViewBag.Currentpage = currentpage;

            //Günlük Yeni Sipariş
            ViewBag.DailyOrderCount = _orderManager.GetAllWithoutJoin(x =>
            !x.IsCanceled && !x.IsDeleted && !x.IsCompleted
            && x.CreatedDate.Year == DateTime.Now.Year && x.CreatedDate.Month == DateTime.Now.Month
            && x.CreatedDate.Day == DateTime.Now.Day).Data.Count;

            //Toplam Yeni Sipariş
            ViewBag.TotalOrderCount = _orderManager.GetAllWithoutJoin(x =>
            !x.IsCanceled && !x.IsDeleted && !x.IsCompleted).Data.Count;

            //Aylık Ciro
            ViewBag.MonthlyTurnover = _orderManager.GetAllWithoutJoin(x =>
            !x.IsCanceled && !x.IsDeleted
            && x.CreatedDate.Year == DateTime.Now.Year
            && x.CreatedDate.Month == DateTime.Now.Month).Data.Sum(x => x.TotalPrice);


            return View();
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(ProductDTO model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var sameProduct = _productManager.GetAll(x => x.SerialNumber == model.SerialNumber).Data;
                if (sameProduct != null && sameProduct.Count > 0)
                {
                    ModelState.AddModelError("", "Bu ürün sistemde kayıtlıdır!");
                    return View(model);
                }

                ProductDTO prod = new ProductDTO()
                {
                    ProductName = model.ProductName,
                    SerialNumber = model.SerialNumber,
                    UnitPrice = model.UnitPrice,
                    StockQuantity = model.StockQuantity,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false

                };
                if (!_productManager.Add(prod).IsSuccess)
                {
                    ModelState.AddModelError("", "Yeni ürün kayıt edilemedi! Tekrar deneyiniz!");
                    return View(model);
                }
                return View(model);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik bir hata oluştu! Tekrar deneyiniz!");
                _logger.LogError(ex, "HATA: Admin/AddProduct");
                return View(model);
            }

        }

        public IActionResult AddCategory()
        {
            return View();
        }
        public IActionResult CategoryList()
        {
            return View();
        }


        public JsonResult GetDougnutData()
        {
            try
            {

                // hangi kategorilerden sipariş verilmiş

                var productData = _orderDetailManager.GetAllWithoutJoin(x => !x.IsCanceled && !x.IsDeleted).Data.GroupBy(x => x.ProductId).ToList();

                List<CategoryDTO> categorydata = new List<CategoryDTO>();
                foreach (var item in productData)
                {
                    var a = _categoryProductPropertyManager.GetByConditionWithoutJoin(x => x.ProductId == item.Key).Data.CategoryId;
                    var c = _categoryManager.GetByConditionWithoutJoin(x => x.Id == a).Data;

                    categorydata.Add(c);
                }

                var data = new List<DoughnutDataViewModel>();
                foreach (var item in categorydata)
                {
                    data.Add(new DoughnutDataViewModel()
                    {
                        Value = item.Id,
                        Color = "#F7464A"
                    });
                }

                return Json(new { issucces = true, message = "chart oluştu!", data });


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"HATA: Admin/GetDougnutData");
                return Json(new { issucces = false, message = ex.Message });

            }
        }


        // günler enumı oluşturulabilir
        public JsonResult GetLineChartData()
        {
            try
            {

                LineandBarChartViewModel data = new LineandBarChartViewModel();
                Dictionary<string, int> dailyordercount = new Dictionary<string, int>();

                //ben hangi gündeyim
                var date = DateTime.Now;
                DateTime beginDate = DateTime.Now;
                DateTime endDate = DateTime.Now;

                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        endDate = DateTime.Now;
                        beginDate = DateTime.Now.AddDays(-7);
                        break;
                    case DayOfWeek.Monday:
                        beginDate = DateTime.Now;
                        endDate = DateTime.Now.AddDays(7);
                        break;
                    case DayOfWeek.Tuesday:
                        beginDate = DateTime.Now.AddDays(-1);
                        endDate = DateTime.Now.AddDays(6);
                        break;
                    case DayOfWeek.Wednesday:
                        beginDate = DateTime.Now.AddDays(-2);
                        endDate = DateTime.Now.AddDays(5);
                        break;
                    case DayOfWeek.Thursday:
                        beginDate = DateTime.Now.AddDays(-3);
                        endDate = DateTime.Now.AddDays(4);
                        break;
                    case DayOfWeek.Friday:
                        beginDate = DateTime.Now.AddDays(-4);
                        endDate = DateTime.Now.AddDays(2);
                        break;
                    case DayOfWeek.Saturday:
                        beginDate = DateTime.Now.AddDays(-5);
                        endDate = DateTime.Now.AddDays(1);
                        break;
                    default:
                        break;
                }

                var orders = _orderManager.GetAllWithoutJoin(x => !x.IsDeleted && !x.IsCanceled
               && x.CreatedDate >= beginDate && x.CreatedDate <= endDate).Data;

                //1.yol
                for (int i = 0; i < 7; i++)
                {

                    // uzun yol
                    if (i == 1)
                    {
                        dailyordercount.Add("Pazartesi", orders.Where(x => x.CreatedDate.DayOfWeek == DayOfWeek.Monday).Count());
                    }
                    else if (i == 2)
                    {
                        dailyordercount.Add("Salı", orders.Where(x => x.CreatedDate.DayOfWeek == DayOfWeek.Tuesday).Count());
                    }
                    else if (i == 3)
                    {
                        dailyordercount.Add("Çarşamba", orders.Where(x => x.CreatedDate.DayOfWeek == DayOfWeek.Wednesday).Count());
                    }
                    else if (i == 4)
                    {
                        dailyordercount.Add("Perşembe", orders.Where(x => x.CreatedDate.DayOfWeek == DayOfWeek.Thursday).Count());
                    }
                    else if (i == 5)
                    {
                        dailyordercount.Add("Cuma", orders.Where(x => x.CreatedDate.DayOfWeek == DayOfWeek.Friday).Count());
                    }
                    else if (i == 6)
                    {
                        dailyordercount.Add("Cumartesi", orders.Where(x => x.CreatedDate.DayOfWeek == DayOfWeek.Saturday).Count());
                    }
                    else if (i == 7)
                    {
                        dailyordercount.Add("Pazar", orders.Where(x => x.CreatedDate.DayOfWeek == DayOfWeek.Sunday).Count());
                    }
                }

                ////2.yol ingilzice gelecek for ile yazıldı

                //for (int i = 0; i < 7; i++)
                //{

                //    dailyordercount.Add(
                //        Enum.GetName(typeof(DayOfWeek), i).ToString(), // saturday 
                //        orders.Where(x => (int)x.CreatedDate.DayOfWeek == i).Count() // sayı
                //        );
                //}

                ////2.yol ingilzice gelecek foreach ile yazıldı
                //foreach (var item in Enum.GetValues(typeof(DayOfWeek)))
                //{
                //    dailyordercount.Add(item.ToString(), // saturday 
                //        orders.Where(x => (int)x.CreatedDate.DayOfWeek == (int)item).Count() // sayı
                //        );
                //}



                data.Labels = dailyordercount.Keys.ToArray();
                data.LabelCount = dailyordercount.Values.ToArray();

                return Json(new { issucces = true, message = "chart oluştu!", data });


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"HATA: Admin/GetLineChartData");
                return Json(new { issucces = false, message = ex.Message });

            }
        }



        public JsonResult GetBarChartData()
        {
            try
            {

                LineandBarChartViewModel data = new LineandBarChartViewModel();
                Dictionary<string, int> dailyordercount = new Dictionary<string, int>();

                var orders = _orderManager.GetAllWithoutJoin(x => !x.IsDeleted && !x.IsCanceled
                && x.CreatedDate.Year == DateTime.Now.Year).Data;

                foreach (var item in Enum.GetValues(typeof(AllMonths)))
                {
                    dailyordercount.Add(item.ToString(), // ekim 
                        orders.Where(x => (int)x.CreatedDate.Month == (int)item).Count() // sayı
                        );
                }
                data.Labels = dailyordercount.Keys.ToArray();
                data.LabelCount = dailyordercount.Values.ToArray();

                return Json(new { issucces = true, message = "chart oluştu!", data });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"HATA: Admin/GetBarChartData");
                return Json(new { issucces = false, message = ex.Message });

            }
        }



        public IActionResult GetProductsNoProps()
        {
            try
            {
                #region 1YOL
                //tüm ürün listesi
                var allproducts = _productManager.GetAllWithoutJoin(x => !x.IsDeleted).Data;

                var productHasprops = _categoryProductPropertyManager.GetAllWithoutJoin(x => !x.IsDeleted).Data.Select(x => x.ProductId).
                    Distinct().ToList();

                var productsHaspics = _productPictureManager.GetAllWithoutJoin(x => !x.IsDeleted).Data
                    .Select(x => x.ProductId).
                    Distinct().ToList();

                // iki listeyi birleştirelim 
                var union_props = productHasprops.Union(productsHaspics);

                //tüm ürün listesinde olup unionlu listesinde olmayan ürünler
                var data = allproducts.Where(x => !union_props.Any(y => y == x.Id)).ToList();
                #endregion


                //#region 2YOL
                //////tüm ürün listesi
                //var allproducts = _productManager.GetAllWithoutJoin(x => !x.IsDeleted).Data.
                //    Select(x => x.Id).ToList(); ;
                //var productHasprops = _categoryProductPropertyManager.GetAllWithoutJoin(x => !x.IsDeleted).Data.Select(x => x.ProductId).
                //    Distinct().ToList();

                //var productsHaspics = _productPictureManager.GetAllWithoutJoin(x => !x.IsDeleted).Data
                //    .Select(x => x.ProductId).
                //    Distinct().ToList();
                //// iki listeyi birleştirelim 
                //var union_props = productHasprops.Union(productsHaspics);

                //var idList= allproducts.Except(union_props).ToList();
                ////2. yol best practice değildir! çünkü bulduüğunuz idlere ait productların bilgileri çekmeniz için tekrar bir satır daha yazmanız gerekecek.
                //var data = _productManager.GetAllWithoutJoin(x => idList.Contains(x.Id)).Data.ToList();

                //#endregion


                return View(data);
            }
            catch (Exception ex)
            {
                //loglanmalı
                return View(new List<ProductDTO>());
            }
        }


        public IActionResult AddProductProps(int id)
        {
            try
            {
                var product = _productManager.GetbyId(id).Data;
                if (product == null)
                {
                    ModelState.AddModelError("", "Ürün bulunamadı!");
                    return View(new NewAddedProductViewModel());
                }

                NewAddedProductViewModel model = new NewAddedProductViewModel()
                {
                    Product = product
                };

                ViewBag.AllCategories = _categoryManager.GetAllWithoutJoin(x => !x.IsDeleted
                && x.MainCategoryId != null).Data;
                return View(model);

            }
            catch (Exception ex)
            {
                //loglanmalı
                return View(new NewAddedProductViewModel());
            }
        }


        [HttpPost]
        public IActionResult AddProductProps(NewAddedProductViewModel model)
        {
            try
            {
                if (model.CategoryId <= 0)
                {

                }
                var product = _productManager.GetbyId(model.Product.Id).Data;
                if (product == null)
                {
                    //error eklenmedli
                    //log atılmalı
                    return View(model);
                }

                //ürüne ait bilgiler
                product.ProductName = model.Product.ProductName;
                product.SerialNumber = model.Product.SerialNumber;
                product.UnitPrice = model.Product.UnitPrice;
                product.StockQuantity = model.Product.StockQuantity;

                _productManager.Update(product);
                //ürün resim
                if (model.ProductPicsFiles != null)
                {
                    foreach (var item in model.ProductPicsFiles)
                    {
                        if (item != null && item.ContentType.Contains("image") && item.Length > 0)
                        {
                            string fileName = $"{item.FileName.Substring(0, item.FileName.IndexOf('.'))}-{Guid.NewGuid().ToString().Replace("-", "")}";

                            string uzanti = Path.GetExtension(item.FileName);

                            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/ProductPictures/{fileName}{uzanti}");

                            string directoryPath =
                               Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/ProductPictures/");

                            if (!Directory.Exists(directoryPath))
                                Directory.CreateDirectory(directoryPath);

                            using var stream = new FileStream(path, FileMode.Create);

                            item.CopyTo(stream);
                            ProductPictureDTO pictureDTO = new ProductPictureDTO()
                            {
                                CreatedDate = DateTime.Now,
                                PicturePath = $"/ProductPictures/{fileName}{uzanti}",
                                ProductId = product.Id,
                                IsDeleted = false
                            };

                            _productPictureManager.Add(pictureDTO);
                        }

                    }

                }

                //üürn url resim
                if (model.ProductPicsURLS != null)
                {
                    var urls = model.ProductPicsURLS.Split(",");
                    foreach (var item in urls)
                    {
                        ProductPictureDTO pictureDTO = new ProductPictureDTO()
                        {
                            CreatedDate = DateTime.Now,
                            PicturePath = item,
                            ProductId = product.Id,
                            IsDeleted = false
                        };

                        _productPictureManager.Add(pictureDTO);
                    }
                }

                //ürün özellik
                foreach (var item in model.ProductProps)
                {
                    //id:45_val:Beyaz
                    var alttireIndex = item.IndexOf("_");
                    var propid = item.Substring(3, alttireIndex - 3);

                    var propvalIndex = item.IndexOf("val:");
                    var propval = item.Substring(propvalIndex + 4);
                    CategoryProductPropertyDTO cpp = new CategoryProductPropertyDTO()
                    {
                        CreatedDate = DateTime.Now,
                        ProductId = product.Id,
                        CategoryId = model.CategoryId,
                        IsDeleted = false,
                        ProductPropertyId = Convert.ToInt32(propid),
                        Value = propval
                    };
                    _categoryProductPropertyManager.Add(cpp);
                }



                return RedirectToAction("GetProductsNoProps", "Admin");

            }
            catch (Exception ex)
            {
                //loglanmalı
                return RedirectToAction("GetProductsNoProps", "Admin");
            }
        }

        public JsonResult GetProps(string pvalue)
        {
            try
            {
                var allprops = _productPropertyManager.GetAllWithoutJoin(x =>
                !x.IsDeleted && x.Name.Contains(pvalue)).Data;

                return Json(new { issucces = true, message = "özellikler bulundu!", allprops });


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"HATA: Admin/GetProps pvalue={pvalue}");
                return Json(new { issucces = false, message = $"Beklenmedik bir hata oluştu!" });
            }
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string psw)
        {
            try
            {
               var response = new DataResult<LoginAPIResponseVM>();
               var url = "http://localhost:5224/api/Home/login";
                using (WebClient client = new WebClient())
                {
                    #region POST YAPILIYOR
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    // client.Headers["Content-Type"] = "application/json";
                    LoginVM model = new LoginVM
                    {
                        Username = username,
                        Password = psw,
                    };
                    var dataString = JsonConvert.SerializeObject(model);


                    var result = client.UploadString(url, "post", dataString);
                    #endregion
                    #region POST NETİCESİNDE ÇIKTI ALINIYOR
                    // json ile donustur
                    response = JsonConvert.DeserializeObject<DataResult<LoginAPIResponseVM>>(result);
                    #endregion   
                } // using client bitti

              

                if (response.Data != null) // token kontrol
                {
                    //token gelirse reports apisinden gerekli bilgileri alacağız
                    if (response.Data.Token != null && response.Data.Expiration <= DateTime.Now)
                    {

                        using (WebClient cclient = new WebClient())
                        {
                            #region GET YAPILIYOR
                             url = "http://localhost:5224/api/Home/dor/";
                            //appsettings json dosyasından postakoduapi urli alalım

                            cclient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                            cclient.Headers.Add("Authorization", "Bearer " + response.Data.Token);
                            var res = cclient.DownloadString(url);
                            #endregion

                            #region GET NETİCESİNDE ÇIKTI ALINIYOR
                            //Elimdeki json'i objelere aktarmaliyim.
                            var result = JsonConvert.DeserializeObject<DataResult<OrderReportAPIVM>>(res);
                            return RedirectToAction("Reports", "Admin", new { ordercount=result.Data.OrderCount, orderamount = result.Data.OrderAmount });


                            #endregion

                        }
                    }
                }

                ModelState.AddModelError("", "kullanıcı adı veya şifreyi doğru yazdığınıza emin olunuz!");
                return View();

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("(401) Unauthorized"))
                {
                    ModelState.AddModelError("", "username ve şifreyi doğru giridğinize emin olunuz!!");
                    return View();

                }
                ModelState.AddModelError("", "Beklenmedik sorun oluştu!");
                return View();
            }
        }


        public IActionResult Reports(int ordercount , decimal orderamount)
        {
            
            return View(new OrderReportAPIVM()
            {
                OrderAmount = orderamount,
                OrderCount = ordercount
            });
        }
    }
}
