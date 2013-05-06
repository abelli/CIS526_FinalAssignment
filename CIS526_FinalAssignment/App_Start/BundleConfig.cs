using System.Web;
using System.Web.Optimization;

namespace CIS526_FinalAssignment.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.9.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-1.10.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                        "~/Scripts/knockout-2.1.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/leaderboard").Include(
                        "~/Scripts/leaderboard.js"));

            bundles.Add(new ScriptBundle("~/bundles/Player").Include(
                        "~/Scripts/Player.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
        }
    }
}