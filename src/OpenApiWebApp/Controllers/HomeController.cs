﻿using OpenApiWebApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tavis.OpenApi;
using Tavis.OpenApi.Model;

namespace OpenApiWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Update(HomeViewModel model)
        {
            var openApiParser = new OpenApiParser();
            OpenApiDocument doc = null;
            try
            {
                doc = openApiParser.Parse(model.Input);
                model.Errors = String.Join("\r\n", openApiParser.ParseErrors);
            } catch (Exception ex)
            {
                model.Errors = ex.Message;
                model.Output = string.Empty;
            }
            if (doc != null)
            {
                var outputwriter = new OpenApiV3Writer(doc);
                var outputstream = new MemoryStream();
                outputwriter.Writer(outputstream);
                outputstream.Position = 0;

                model.Output = new StreamReader(outputstream).ReadToEnd();
            }
            return View("Index",model);

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
    }
}