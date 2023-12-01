using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QRCoder;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Text.Json;
using TechStoreBL.EmailSenderProcess;
using TechStoreBL.InterfacesOfManagers;
using TechStoreDL.ImplementationOfRepos;
using TechStoreDL.InterfaceofRepos;
using TechStoreEL.Entities;
using TechStoreEL.IdentityModels;
using TechStoreEL.ResultModels;
using TechStoreEL.ViewModels;
using TechStorePL.Models;
using TechStoreUL;

namespace TechStorePL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductManager _productManager;
        private readonly IProductDiscountManager _productDiscountManager;
        private readonly IProductPictureManager _productPictureManager;
        private readonly ICategoryProductPropertyManager _categoryProductPropertyManager;
        private readonly IOrderManager _orderManager;
        private readonly IOrderDetailManager _orderDetailManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IEmailManager _emailManager;

        public HomeController(ILogger<HomeController> logger, IProductManager productManager, IProductDiscountManager productDiscountManager, IProductPictureManager productPictureManager, ICategoryProductPropertyManager categoryProductPropertyManager, IOrderManager orderManager, IOrderDetailManager orderDetailManager, UserManager<AppUser> userManager, ICategoryManager categoryManager, IEmailManager emailManager)
        {
            _logger = logger;
            _productManager = productManager;
            _productDiscountManager = productDiscountManager;
            _productPictureManager = productPictureManager;
            _categoryProductPropertyManager = categoryProductPropertyManager;
            _orderManager = orderManager;
            _orderDetailManager = orderDetailManager;
            _userManager = userManager;
            _categoryManager = categoryManager;
            _emailManager = emailManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddtoCart(int id)
        {
            try
            {
                var product = _productManager.GetByCondition(x => x.Id == id).Data;
                if (product != null)
                {
                    var cardlist = new List<ProductDTO>();

                    if (HttpContext.Session.GetString("cardlist") != null)
                    {
                        cardlist = System.Text.Json.JsonSerializer.Deserialize<List<ProductDTO>>(HttpContext.Session.GetString("cardlist"));
                        if (cardlist != null)
                        {
                            //ürün sepete ilk kez ekleniyorsa CardQuntity=1
                            if (cardlist.Count(x => x.Id == product.Id) == 0)
                            {
                                product.CardQuantity = 1;
                                cardlist.Add(product);
                            }
                            else
                            {
                                //eğer ürün sepette zaten varsa CardQuntity++
                                var p = cardlist.FirstOrDefault(x => x.Id == product.Id);
                                p.CardQuantity++;

                            }


                        }
                    }
                    else
                    {
                        product.CardQuantity = 1;
                        cardlist.Add(product);
                    }

                    foreach (var item in cardlist)
                    {
                        item.ProductDiscount = _productDiscountManager.GetByConditionWithoutJoin(x => x.ProductId == item.Id && !x.IsFinished).Data;
                    }
                    HttpContext.Session.SetString("cardlist", System.Text.Json.JsonSerializer.Serialize(cardlist));

                }
                return View("Index", "Home");

                //  HttpContext.Session.SetString("cartItems", itemsList);
            }
            catch (Exception ex)
            {

                return View("Index", "Home");
            }
        }

        public IActionResult Cart()
        {
            try
            {

                if (HttpContext.Session.GetString("cardlist") != null)
                {
                    var cardlist = System.Text.Json.JsonSerializer.Deserialize<List<ProductDTO>>(HttpContext.Session.GetString("cardlist"));
                    foreach (var item in cardlist)
                    {
                        item.ProductDiscount = _productDiscountManager.GetByConditionWithoutJoin(x => x.ProductId == item.Id && !x.IsFinished).Data;

                        item.ProductPictures = _productPictureManager.GetAllWithoutJoin(x => x.ProductId == item.Id && !x.IsDeleted).Data.ToList();
                    }

                    return View(cardlist);

                }
                return View(new List<ProductDTO>());
            }
            catch (Exception ex)
            {
                // ex loglanacak
                return View(new List<ProductDTO>());
            }
        }

        [Authorize]
        public IActionResult CreateOrder()
        {
            try
            {
                var username = User.Identity.Name;
                var user = _userManager.FindByNameAsync(username).Result;

                //sepetteki ürünler
                var cardlist = System.Text.Json.JsonSerializer.Deserialize<List<ProductDTO>>(HttpContext.Session.GetString("cardlist"));
                decimal totalPrice = 0;
                foreach (var item in cardlist)
                {
                    item.ProductDiscount = _productDiscountManager.GetByConditionWithoutJoin(x => x.ProductId == item.Id && !x.IsFinished).Data;
                    if (item.ProductDiscount != null)
                    {
                        totalPrice += (item.UnitPrice - (item.UnitPrice * item.ProductDiscount.Discount / 100)) * item.CardQuantity;

                    }
                    else
                    {
                        totalPrice += item.UnitPrice * item.CardQuantity;
                    }
                }

                //Order 
                OrderDTO order = new OrderDTO
                {
                    CreatedDate = DateTime.Now,
                    IsCanceled = false,
                    IsCompleted = false,
                    IsDeleted = false,
                    UserId = user.Id,
                    TotalPrice = totalPrice,
                    OrderNo = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{user.Id.Substring(0, 7)}"
                };
                var result = _orderManager.Add(order);
                if (result.IsSuccess)
                {
                    //order details
                    foreach (var item in cardlist)
                    {
                        OrderDetailDTO order_detail = new OrderDetailDTO()
                        {
                            CreatedDate = DateTime.Now,
                            IsCanceled = false,
                            IsDeleted = false,
                            OrderId = result.Data.Id,
                            ProductId = item.Id,
                            ProductPrice = item.UnitPrice,
                            Quantity = item.CardQuantity,
                            Discount = item.ProductDiscount == null ? 0 : item.ProductDiscount.Discount
                        };
                        _orderDetailManager.Add(order_detail);
                    }

                    HttpContext.Session.SetString("cardlist", System.Text.Json.JsonSerializer.Serialize(new List<ProductDTO>()));

                    var url = Url.Action("Order", "Home", new { o = result.Data.Id },
                          protocol: Request.Scheme);


                    #region QRCODE_CREATE
                    QRCodeGenerator QRGenerator = new QRCodeGenerator();
                    QRCodeData QRData = QRGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrcode = new QRCode(QRData);
                    Bitmap qrBitmap = qrcode.GetGraphic(60);
                    byte[] bitmapArray = BitmapToByteArray(qrBitmap);

                    #region Order_Email

                    string bodymessage = $"Merhaba {user.Name} {user.Surname} <br/><br/>" +
          $"{cardlist.Sum(x => x.CardQuantity)} adet ürünlerinizin siparişini aldık.<br/><br/>" +
          $"Toplam Tutar:{cardlist.Sum(x => x.UnitPrice).ToString()} ₺ <br/> <br/>" +
          $"<table><tr><th>Ürün Adı</th><th>Adet</th><th>Birim Fiyat</th><th>Toplam</th></tr>";
                    foreach (var item in cardlist)
                    {
                        bodymessage += $"<tr><td>{item.ProductName}</td><td>{item.UnitPrice}</td><td>{item.UnitPrice}</td></tr>";
                    }


                    bodymessage += "</table><br/>Siparişinize ait QR kodunuz aşağıdadır. <br/><br/>";

                    _emailManager.SendMail(bitmapArray, new EmailMessageModel()
                    {
                        Subject = "TechStore - Sipariş Detayları",
                        To = user.Email,
                        Body = bodymessage
                    });

                    #endregion

                    #endregion
                }


                //tüm siparişleri
                var data = _orderManager.GetAll(x => x.UserId == user.Id).Data;
                return View(data);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", Utilities.GeneralErrorMessage);
                //ex log
                return View(new List<OrderDTO>());
            }
        }

        [Authorize]
        public IActionResult OrderHistory()
        {
            try
            {
                var username = User.Identity.Name;
                var user = _userManager.FindByNameAsync(username).Result;

                //tüm siparişleri
                var data = _orderManager.GetAll(x => x.UserId == user.Id).Data;
                return View(data);
            }
            catch (Exception ex)
            {
                //ex log
                return View(new List<OrderDTO>());
            }

        }
        public IActionResult Product(int id)
        {
            try
            {
                var product = _productManager.GetbyId(id).Data;

                if (product != null)
                {
                    product.ProductPictures = _productPictureManager.GetAll(x => x.ProductId == id).Data.ToList();
                    product.CategoryProductProperties = _categoryProductPropertyManager.GetAll(x => x.ProductId == id).Data.ToList();

                    return View(product);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Beklenmedik bir hata oluştu!{ex.Message}");
                return View(new List<Product>());
            }
        }


        public JsonResult GetSubCategories(long id)
        {
            try
            {
                if (id == 12 || id == 13)
                {

                }
                if (id <= 0)
                {
                    _logger.LogError($"HATA: Home/GetSubCategories id= {id} değeri sıfırdan küçük olamaz!");
                    return Json(new { issuccess = false, message = $"id değeri sıfırdan küçük olamaz!" });
                }

                var data = _categoryManager.GetAllWithoutJoin(x => !x.IsDeleted
                && x.MainCategoryId == id).Data;


                _logger.LogInformation($"Home/GetSubCategories  geldi!");
                return Json(new { issuccess = true, message = $"Alt kategoriler geldi!", data });


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"HATA: Home/GetSubCategories id={id}");
                return Json(new { issuccess = false, message = $"Beklenmedik bir hata oluştu!" });
            }

        }



        [HttpGet]
        public IActionResult Login(string? email)
        {
            TempData["login"] = "login";
            return View("Login", email);
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("", "Lütfen gerekli alanları dolduurnuz!");
                    return View("Login", email);
                }

                using (WebClient client = new WebClient())
                {
                    #region GET YAPILIYOR
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    string url = "http://localhost:5224/api/Home/login";
                    //appsettings json dosyasından postakoduapi urli alalım

                    TechStoreEL.ViewModels.LoginVM l = new TechStoreEL.ViewModels.LoginVM()
                    {
                        Username = email,
                        Password = password
                    };

                    var dataString = JsonConvert.SerializeObject(l);
                    //  var JSONData = new { email = email, psw= password };
                    //  var dataString = JsonConvert.SerializeObject(JSONData);
                    ////  string myParameters = $"email={email}&psw={password}";

                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    string resultJSON = client.UploadString(url, "POST", dataString);

                    #endregion

                    #region GET NETİCESİNDE ÇIKTI ALINIYOR
                    //Elimdeki json'i objelere aktarmaliyim.
                    var response = JsonConvert.DeserializeObject<DataResult<object>>(resultJSON);
                    #endregion
                }


                return View(); // fake


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik bir sorun oldu!");
                //ex loglanacak
                return View("Login", email);
            }
        }


        public IActionResult ResultPage(string message)
        {
            return View(message);
        }


        [NonAction]// NoAction web sayfaso olarak açılmasını istemediğmiz metotlarda geçerlidir
        public static byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }


        public IActionResult Order(long o)
        {
            try
            {
                //o parametresi kontrol
                var order = _orderManager.GetbyId(o).Data;

                order.AppUser = _userManager.FindByIdAsync(order.UserId).Result;

                order.OrderDetails = _orderDetailManager.GetAll(x => x.OrderId == order.Id).Data.ToList();

                return View(order);
            }
            catch (Exception ex)
            {
                //ex log
                ModelState.AddModelError("", "Beklenmedik bir sorun oluştu!");
                var order = new OrderDTO()
                {
                    OrderDetails = new List<OrderDetailDTO>(),
                    AppUser = new AppUser()
                };
                return View(Order);

            }
        }

    }
}