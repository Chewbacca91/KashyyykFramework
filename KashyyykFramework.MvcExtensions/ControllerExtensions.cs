using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Web.Routing;
using System.Web.Mvc;

namespace KashyyykFramework.MvcExtensions
{
    public static class ControllerExtensions
    {
        public static string RenderRazorViewToString(this Controller controller, string viewName, object model)
        {
            if (string.IsNullOrWhiteSpace(viewName)) viewName = controller.ControllerContext.RouteData.GetRequiredString("action");
            if (model != null) controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public static string RenderRazorViewToString(this Controller controller, string viewName)
        {
            return ControllerExtensions.RenderRazorViewToString(controller, viewName, null);
        }
    }
}
