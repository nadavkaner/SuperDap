using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using FinalProject.Models;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;


namespace FinalProject.Controllers
{
    public class BuyController : Controller
    {
        public ActionResult Index(int? id)
        {
            string EbayRequest = "http://svcs.ebay.com/services/search/FindingService/v1?OPERATION-NAME=findItemsByKeywords&SERVICE-VERSION=1.0.0&SECURITY-APPNAME=YakirKad-Kadkoda-PRD-645e91339-73768646&GLOBAL-ID=EBAY-US&RESPONSE-DATA-FORMAT=JSON&REST-PAYLOAD&keywords=" +
                                 "Computer Programming Books" + "&paginationInput.entriesPerPage=20";

            var result = JObject.Parse(GET(EbayRequest));
            int numberOfResults = Convert.ToInt32(result["findItemsByKeywordsResponse"][0]["searchResult"][0]["@count"].ToString());
            var EbayListItem = new List<Buy.EbayItem>();

            for (int index = 0; index < numberOfResults; index++)
            {
                Buy.EbayItem item = new Buy.EbayItem();
    
                item.ProductName = result["findItemsByKeywordsResponse"][0]["searchResult"][0]["item"][index]["title"][0].ToString();
                item.URL         = result["findItemsByKeywordsResponse"][0]["searchResult"][0]["item"][index]["viewItemURL"][0].ToString();
                item.Price       = result["findItemsByKeywordsResponse"][0]["searchResult"][0]["item"][index]["sellingStatus"][0]["currentPrice"][0]["__value__"].ToString();

                EbayListItem.Add(item);
            }

            ViewBag.ebayResuls = EbayListItem;

            return View();
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
                }
                throw;
            }
        }
    }
}