using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Configuration;

namespace HelloWorldConsole
{

    class Program
    {
        // ... option for defining targets - not used here
        public enum WriteToTarget
        {
            Console,
            DB,
            Other

        }

        public abstract class ToTargetBase
        {
            public abstract void WriteTo(string message);
        }

        public class ConsoleWrite : ToTargetBase
        {
            public override void WriteTo(string message)
            {
                Console.WriteLine(message);
            }
        }
        public class DBWrite : ToTargetBase
        {
            // Get connection string from app.config ...
            static string connectionString = string.Empty;
            public override void WriteTo(string message)
            {
                // Write to database ...
            }
        }

        public static class WriteSelector
        {

            public static ToTargetBase myWriter = null;
            private static string deviceType = ConfigurationManager.AppSettings["deviceType"];
            public static void SelectWriter()
            {
                switch (deviceType)
                {
                    case "Console":
                        myWriter = new ConsoleWrite();
                        break;
                    case "DB":
                        myWriter = new DBWrite();
                        break;
                    default:
                        return;
                }
            }
        }


        static void Main(string[] args)
        {
            WriteSelector.SelectWriter();

            Task t = new Task(GetDataAsync);
            t.Start();
            Console.WriteLine("Retrieve string to write");
            Console.ReadLine();

        }

        static async void GetDataAsync()
        {
            // ... source of data 
            string dataAddress = "http://localhost:5225/api/hw/0";


            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(dataAddress))
            using (HttpContent content = response.Content)
            {
                // ... Read contents from response
                string result = await content.ReadAsStringAsync();

                // ... Write to console or write to DB
                if (result != null &&
                    result.Length >= 1)
                {
                    // ... validate data before writing
                    WriteSelector.myWriter.WriteTo(result);
                }
            }
        }
    }
}
