using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;
using TreeView2.Models;

namespace TreeView2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<G_JSTree> GetAllNodes11(string id)
        {
            List<G_JSTree> G_JSTreeArray = new List<G_JSTree>();

            G_JSTree _G_JSTree = new G_JSTree();
            _G_JSTree.data = "x1";
            _G_JSTree.state = "closed";
            _G_JSTree.IdServerUse = 10;
            _G_JSTree.children = null;
            _G_JSTree.attr = new G_JsTreeAttribute { id = "10", selected = false };
            G_JSTreeArray.Add(_G_JSTree);

            G_JSTree _G_JSTree2 = new G_JSTree();
            var children =
                new G_JSTree[]
            {
                new G_JSTree { data = "x1-11", attr = new G_JsTreeAttribute { id = "201" } },
                new G_JSTree { data = "x1-12", attr = new G_JsTreeAttribute { id = "202" } },
                new G_JSTree { data = "x1-13", attr = new G_JsTreeAttribute { id = "203" } },
                new G_JSTree { data = "x1-14", attr = new G_JsTreeAttribute { id = "204" } },
            };
            _G_JSTree2.data = "x2";
            _G_JSTree2.IdServerUse = 20;
            _G_JSTree2.state = "closed";
            _G_JSTree2.children = children;
            _G_JSTree2.attr = new G_JsTreeAttribute { id = "20", selected = true };
            G_JSTreeArray.Add(_G_JSTree2);

            G_JSTree _G_JSTree3 = new G_JSTree();
            var children2 =
                new G_JSTree[]
            {
                new G_JSTree { data = "x2-11", attr = new G_JsTreeAttribute { id = "301" } },
                new G_JSTree { data = "x2-12", attr = new G_JsTreeAttribute { id = "302" },
                    children= new G_JSTree[]{new G_JSTree{data = "x2-21", attr = new G_JsTreeAttribute { id = "3011" }}} },
                new G_JSTree { data = "x2-13", attr = new G_JsTreeAttribute { id = "303" } },
                new G_JSTree { data = "x2-14", attr = new G_JsTreeAttribute { id = "304" } },
            };
            _G_JSTree3.data = "x3";
            _G_JSTree3.state = "closed";
            _G_JSTree3.IdServerUse = 30;
            _G_JSTree3.children = children2;
            _G_JSTree3.attr = new G_JsTreeAttribute { id = "30", selected = true };
            G_JSTreeArray.Add(_G_JSTree3);
            return G_JSTreeArray;
        }
    }
}