using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using RestSharp;
using System.Xml.Linq;

namespace ASKING_API
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var informer = "";
            var configFilePath = @"config.xml";
            var resultFilePath = @"result.txt";
            var address = XMLReader.ReadTagValue(configFilePath, "Config", "Server", "Address");
            var header = XMLReader.ReadTagValue(configFilePath, "Config", "Header", "Authorization");
            var body = XMLReader.ReadTagValue(configFilePath, "Config", "Body", "Body");
            var date = dateTimePicker1.Value;
            var dateTime = date.ToString("yyyyMMbb");
            informer = informer +
            "\n sending query to: " + address +
            "\n with header: Authorization" + header +
            "\n with body: " + body +
            "\n saving result into: " + resultFilePath;
            label1.Text = informer;

            var query = RestQuery.Get(address, header, body, dateTime, resultFilePath);
            label1.Text = informer + "\n " + query.ToString();
        }
    }

    public class XMLReader
    {
        public static string ReadTagValue(string filePath, string myXMLTag, string myXMLAttribute, string myXMLElement)
        {

            var content = "";

            if (File.Exists(filePath))
            {
                XElement myXMLDoc = XElement.Load(filePath);

                IEnumerable<string> myData = from item in myXMLDoc.Descendants(myXMLTag) where (string)item.Attribute("Type") == myXMLAttribute select (string)item.Element(myXMLElement);

                foreach (var el in myData)
                {
                    content = content + el;
                }
            }
            return content;
        }
    }

    public class RestQuery
    {
        public static bool Get(string address, string au, string body, string dateTimeReplacement, string filePath)
        {
            // Create a RestClient object with the base URL
            var myClient = new RestClient(address);
            //myClient.Timeout = -1;

            // Create a RestRequest object with the endpoint and method
            var myRequest = new RestRequest();

            ////var modifiedBody = body.Replace("20240101", dateTimeReplacement);

            // Add body content as JSON
            /*var myBody = new
            {
                body
            };*/

            var myBody = body;

            ////myRequest.AddJsonBody(myBody);

            var param = new  { IntData = 1, StringData = "test123" };
            //myRequest.AddJsonBody(param);
            
            // Optionally, add headers
            // myRequest.AddHeader("Authorization", au);
            //myRequest.AddHeader("Content-Type", "application/json");

                        // Execute the request and get the response
                        var myResponse = myClient.Execute(myRequest);

                        // Output the response content
                        /*Console.WriteLine("sending query to: " + address);
                        Console.WriteLine("with header: Authorization" + au);
                        Console.WriteLine("with body: " + body);
                        Console.WriteLine("progress resuil is: ");
                        Console.WriteLine(myResponse.Content);
                        Console.WriteLine("saving result into: " + filePath);*/
                        File.WriteAllText(filePath, myResponse.Content);
            return true;

        }
    }

}
