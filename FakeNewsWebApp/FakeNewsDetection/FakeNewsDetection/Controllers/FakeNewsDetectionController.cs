using FakeNewsDetection.DTO;
using FakeNewsDetection.Entities;
using FakeNewsDetection.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace FakeNewsDetection.Controllers
{
    public class FakeNewsDetectionController : Controller
    {
        // GET: FakeNewsDetection
        public ActionResult Index()
        {
            NewsData emptyModel = new NewsData();
            return View(emptyModel);
        }

        public async Task<ActionResult> PredictInput(NewsData newsData)
        {
            if (newsData.Type == TextType.Article)
            {
                return await PredictInputArticle(newsData);
            }
            else if (newsData.Type == TextType.Tweet)
            {
                return await PredictInputTweet(newsData);
            }
            else
            {
                newsData.Result = "Invalid text type";
                return View("Index", newsData);
            }
        }

        public async Task<ActionResult> PredictInputArticle(NewsData newsData)
        { 
            if(ModelState.IsValid)
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri("http://127.0.0.1:5000");
                NewsDataToSendDTO request = new NewsDataToSendDTO
                {
                    Title = newsData.Title ?? String.Empty,
                    Text = newsData.Text ?? String.Empty
                };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.PostAsync("/predictArticle", jsonContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResult = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeAnonymousType(jsonResult, new { label = "" });
                        newsData.Result = result.label ?? "Label not found";
                    }
                    else
                    {
                        var jsonResult = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeAnonymousType(jsonResult, new { error = "" });
                        newsData.Result = "An error has occured";
                    }
                }
                catch (Exception e)
                {
                    newsData.Result = "An error has occured";
                    return View("Index", newsData);
                }
                
            }
            return View("Index", newsData);
        }

        public async Task<ActionResult> PredictInputTweet(NewsData newsData)
        {
            if (ModelState.IsValid)
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri("http://127.0.0.1:5000");
                NewsDataToSendDTO request = new NewsDataToSendDTO
                {
                    Text = newsData.Text ?? String.Empty
                };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                try
                {
                    var response = await client.PostAsync("/predictTweet", jsonContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResult = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeAnonymousType(jsonResult, new { label = "" });
                        newsData.Result = result.label ?? "Label not found";
                    }
                    else
                    {
                        var jsonResult = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeAnonymousType(jsonResult, new { error = "" });
                        newsData.Result = "An error has occured";
                    }
                }
                catch (Exception e)
                {
                    newsData.Result = "An error has occured";
                    return View("Index", newsData);
                }   
            }
            return View("Index", newsData);
        }
    }
}
