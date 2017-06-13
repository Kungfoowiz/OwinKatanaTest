using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using Owin;
using Microsoft.Owin.Hosting;

using System.IO;

using System.Web.Http; 




namespace KatanaIntro
{


    // AppFunc alias. 
    using AppFunc = Func<IDictionary<string, object>, Task>; 


    class Program
    {
        static void Main(string[] args)
        {

            string Uri = "http://localhost:12345";

            using (WebApp.Start<Startup>(Uri))
            {
                Console.WriteLine("Web server started and listening ... ");
                Console.ReadKey();
                Console.WriteLine("Web server stopping ... "); 
            }


        }
    }





    public class Startup
    {


        public void Configuration(IAppBuilder App)
        {


            //// Return welcome page. 
            //App.UseWelcomePage();


            // Katana wrapper for OWIN. 
            //App.Run(
            //    // Use lambda to access the function. 
            //    ctx =>
            //    {
            //        // Access function data. 
            //        // Return a task. 
            //        // ctx.


            //        // Hello world. 
            //        return ctx.Response.WriteAsync("Hello world!");


            //    }
            //);




            // Test. 
            //App.Run(Context =>
            //{

            //    Context.Response.ContentType = "text/plain";
            //    return Context.Response.WriteAsync("Testing using content type ... ");

            //}
            //);



             


            // Dump environment. 
            //App.Use(async (ContextEnvironment, Next) =>
            //{

            //   foreach (var Item in ContextEnvironment.Environment)
            //       Console.WriteLine("{0}:{1}", Item.Key, Item.Value);

            //   await Next();


            //});



            // Dump request. 
            App.Use(async (ContextEnvironment, Next) =>
            {
                Console.WriteLine("Requesting: " + ContextEnvironment.Request.Path);
                await Next();

                //ContextEnvironment.Response.StatusCode = 404; 

                Console.WriteLine("Response: " + ContextEnvironment.Response.StatusCode); 
            });





            ConfigureWebApi(App); 



            // Use HelloWorldComponent. 
            App.Use<HelloWorldComponent>();

            // Syntactic sugar call. 
            //App.UseHelloWorld();




        }


        private void ConfigureWebApi(IAppBuilder App)
        {

            var Config = new HttpConfiguration();

            Config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );

            App.UseWebApi(Config); 

        }



    }




    // Syntactic sugar definition. 
    public static class AppBuilderExtensions
    {
        public static void UseHelloWorld(this IAppBuilder App)
        {
            App.Use<HelloWorldComponent>(); 
        }
    }




    public class HelloWorldComponent
    {

        AppFunc _NextComponent; 

        public HelloWorldComponent(AppFunc NextComponent)
        {
            _NextComponent = NextComponent;
        }


        //public Task Invoke(IDictionary<string, object> Environment)
        // public async Task Invoke(IDictionary<string, object> Environment)
        public Task Invoke(IDictionary<string, object> Environment)
        {
            //return null; 

            // await _NextComponent(Environment);  

            var Response = Environment["owin.ResponseBody"] as Stream;
            using (var Writer = new StreamWriter(Response))
                return Writer.WriteAsync("Hello using StreamWriter and owin.ResponseBody ... "); 

        }

    }



}
