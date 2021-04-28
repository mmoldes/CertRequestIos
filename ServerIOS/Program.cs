using System;
using System.Threading;
using EmbedIO;
namespace ServerIOS
{
    internal class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            var url = "http://localhost:9696/";
            if (args.Length > 0)
                url = args[0];

            // Create Webserver and attach LocalSession and Static
            // files module and CORS enabled
            WebServer server = new WebServer(o => o
             .WithUrlPrefix(url)
             .WithMode(HttpListenerMode.EmbedIO))
             .WithLocalSessionManager()
             .WithAction("/test", HttpVerbs.Any, ctx =>
             {
                 Console.WriteLine("Request received");
                 return ctx.SendDataAsync(new { mensaje = "Hola Mundo!" });
             }

         );

            var cts = new CancellationTokenSource();
            var task = server.RunAsync(cts.Token);

            Console.ReadKey(true);
            cts.Cancel();

            // Wait before dispose server
            task.Wait();
            server.Dispose();
        }
    }
}
