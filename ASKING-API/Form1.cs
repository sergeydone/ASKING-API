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
using RestSharp.Authenticators;
using System.Xml.Linq;
using System.Threading;

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
            //// var myClient = new RestClient(address);
            //// var myRequest = new RestRequest();

            ////var modifiedBody = body.Replace("20240101", dateTimeReplacement);

            /*var myBody = new
            {
                body
            };*/

            ////var myBody = body;
            ////myRequest.AddJsonBody(myBody);

            //// var myResponse = myClient.Execute(myRequest);
            //// File.WriteAllText(filePath, myResponse.Content);

            var options = new RestClientOptions(address)
            {
                Authenticator = new HttpBasicAuthenticator("username", "password")
            };
            var client = new RestClient(options);
            var requestBody = new
            {
                key1 = "value1",
                key2 = "value2",
            };
            var request = new RestRequest().AddJsonBody(requestBody);
           
            var response = client.Execute(request);
           
            return true;

        }
    }

}
