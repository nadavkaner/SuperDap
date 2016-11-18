using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;


namespace FinalProject.Controllers
{
    public class BuyController : Controller
    {
        public class EbayItem
        {
            public string name { get; set; }
            public string url { get; set; }
            public string price { get; set; }
        }

        // Makes an http get request and returns the result.
        string GET(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }

        public ActionResult Index(int? id)
        {



            string EbayRequest = "http://svcs.ebay.com/services/search/FindingService/v1?OPERATION-NAME=findItemsByKeywords&SERVICE-VERSION=1.0.0&SECURITY-APPNAME=YakirKad-Kadkoda-PRD-645e91339-73768646&GLOBAL-ID=EBAY-US&RESPONSE-DATA-FORMAT=JSON&REST-PAYLOAD&keywords=" +
               "Computer Programming Books" + "&paginationInput.entriesPerPage=20";

            // parse the string from the request into a json object
            var result = JObject.Parse(GET(EbayRequest));

            int numberOfResults = Convert.ToInt32(result["findItemsByKeywordsResponse"][0]["searchResult"][0]["@count"].ToString());
            List<EbayItem> items = new List<EbayItem>();

            // Create from a json a list of ebay recomandation
            for (int i = 0; i < numberOfResults; i++)
            {
                EbayItem item = new EbayItem();
    
                item.name = result["findItemsByKeywordsResponse"][0]["searchResult"][0]["item"][i]["title"][0].ToString();
                item.url = result["findItemsByKeywordsResponse"][0]["searchResult"][0]["item"][i]["viewItemURL"][0].ToString();
                item.price = result["findItemsByKeywordsResponse"][0]["searchResult"][0]["item"][i]["sellingStatus"][0]["currentPrice"][0]["__value__"].ToString();

                items.Add(item);
            }

            ViewBag.ebayResuls = items;

            return View();
        }
    }
}