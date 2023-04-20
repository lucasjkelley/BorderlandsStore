using Microsoft.AspNetCore.Mvc;
using BorderlandsStore.DATA.EF.Models;
using Microsoft.AspNetCore.Identity;
using BorderlandsStore.UI.MVC.Models;
using Newtonsoft.Json;

namespace BorderlandsStore.UI.MVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        //Fields/Props
        private readonly BorderlandsStoreContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingCartController(BorderlandsStoreContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            var sessionCart = HttpContext.Session.GetString("cart");

            Dictionary<int, CartItemViewModel> shoppingCart = null;

            if (sessionCart == null || sessionCart.Count() == 0)
            {
               
                shoppingCart = new Dictionary<int, CartItemViewModel>();

                ViewBag.Message = "There are no items in your cart.";
            }
            else
            {
                ViewBag.Message = null;

                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            }

            return View(shoppingCart);
        }

        public IActionResult AddToCart(int id)
        {
            
            Dictionary<int, CartItemViewModel> shoppingCart = null;
                        
            var sessionCart = HttpContext.Session.GetString("cart");

            if (String.IsNullOrEmpty(sessionCart))
            {
                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }
            else
            {                
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            }

            Weapon weapon = _context.Weapons.Find(id);

            CartItemViewModel civm = new CartItemViewModel(1, weapon);

            if (shoppingCart.ContainsKey(weapon.WeaponId))
            {
                shoppingCart[weapon.WeaponId].Qty++;
            }
            else
            {
                shoppingCart.Add(weapon.WeaponId, civm);
            }

            string jsonCart = JsonConvert.SerializeObject(shoppingCart);

            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("Index");

        }

        public IActionResult RemoveFromCart(int id)
        {
            var sessionCart = HttpContext.Session.GetString("cart");

            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            shoppingCart.Remove(id);

            if (shoppingCart.Count == 0)
            {
                HttpContext.Session.Remove("cart");
            }
            else
            {
                string jsonCart = JsonConvert.SerializeObject(shoppingCart);
                HttpContext.Session.SetString("cart", jsonCart);
            }

            return RedirectToAction("Index");


        }

        public IActionResult UpdateCart(int weaponId, int qty)
        {
            var sessionCart = HttpContext.Session.GetString("cart");

            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            shoppingCart[weaponId].Qty = qty;

            string jsonCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("Index");
        }

    }
}
