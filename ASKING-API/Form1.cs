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
            var user = XMLReader.ReadTagValue(configFilePath, "Config", "Header", "User");
            var pass = XMLReader.ReadTagValue(configFilePath, "Config", "Header", "Pathword");
            var body = XMLReader.ReadTagValue(configFilePath, "Config", "Body", "Body");
            var method = XMLReader.ReadTagValue(configFilePath, "Config", "Body", "Method");
            var date = dateTimePicker1.Value;
            var dateTime = date.ToString("yyyyMMdd");
            
            informer = informer +
            "\n sending query to: " + address +
            "\n with username: " + user +
            "\n with body: " + body +
            "\n and will save result into : " + resultFilePath;
            label1.Text = informer;

            var query = RestQuery.Get(address, user, pass, body, method, dateTime, resultFilePath);
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
        public static string Get(string address, string user, string pass,  string body, string method, string dateTimeReplacement, string filePath)
        {
            var client = new RestClient(address);

            if (user.Length > 2)
            {
                var options = new RestClientOptions(address)
                {
                    Authenticator = new HttpBasicAuthenticator(user, pass)
                };
                client = new RestClient(options);
            }

            var request = new RestRequest();

            if (method.ToUpper() == "POST") request = new RestRequest("", Method.Post);
            if (method.ToUpper() == "GET") request = new RestRequest("", Method.Get);

            if (body.Length > 1)
            {
                var modifiedBody = body.Replace("20240101", dateTimeReplacement);
               
                request.AddStringBody(modifiedBody, DataFormat.Json);
            }

            var response = client.Execute(request);

            File.WriteAllText(filePath, response.Content);

            var result = "";

            if (response.ErrorMessage != null) result = result + response.ErrorMessage.ToString();
            else result = result + response.StatusCode.ToString();
            return result;
        }
    }


}
