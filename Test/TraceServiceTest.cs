using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Test
{
    [TestClass]
    public class TraceServiceTest
    {
        private const string BASE_URL = "http://localhost:5001/api";

        [TestMethod]
        public void GetAll()
        {
            using (var client = new HttpClient())
            {
                var res = client.GetAsync(BASE_URL + "/traces").Result;

                Assert.IsNotNull(res, "La response è null");
                Assert.IsTrue(res.IsSuccessStatusCode, "Status code: " + res.StatusCode);

                string s = res.Content.ReadAsStringAsync().Result;
                List<Trace> traces = JsonConvert.DeserializeObject<List<Trace>>(s);

                Assert.IsNotNull(traces, "traces è null");
                Assert.IsTrue(traces.Count > 0, "nessun trace restituito");
            }

        }

        [TestMethod]
        public void GetByRangeTest()
        {

            var t = new Trace()
            {
                CorrelationId = System.Guid.NewGuid().ToString(),
                Description = "Test",
                Details = "Test",
                Level = 0,
                Module = "TraceServiceTest",
                Operation = "GetByRangeTest",
                Origin = "Test",
                TraceDate = new DateTime(2011,2,20)

            };

            t.TraceId = Create(t);

            var t2 = new Trace()
            {
                CorrelationId = System.Guid.NewGuid().ToString(),
                Description = "Test",
                Details = "Test",
                Level = 0,
                Module = "TraceServiceTest",
                Operation = "GetByRangeTest",
                Origin = "Test",
                TraceDate = new DateTime(2011,12,9)

            };

            t2.TraceId = Create(t2);

            var t3 = new Trace()
            {
                CorrelationId = System.Guid.NewGuid().ToString(),
                Description = "Test",
                Details = "Test",
                Level = 0,
                Module = "TraceServiceTest",
                Operation = "GetByRangeTest",
                Origin = "Test",
                TraceDate = new DateTime(2011,12,31)

            };

            t3.TraceId = Create(t3);

            using (var client = new HttpClient())
            {
                string origin="Test";
                string fromDate = "2011-12-01";
                string toDate = "2011-12-31";
                var res = client.GetAsync(BASE_URL + $"/traces/{origin}/{fromDate}/{toDate}").Result;

                Assert.IsNotNull(res, "La response è null");
                Assert.IsTrue(res.IsSuccessStatusCode, "Status code: " + res.StatusCode);

                string s = res.Content.ReadAsStringAsync().Result;
                List<Trace> traces = JsonConvert.DeserializeObject<List<Trace>>(s);

                Assert.IsNotNull(traces, "traces è null");
                Assert.IsTrue(traces.Count == 2, "traces count = " + traces.Count);
                Assert.AreEqual(t2.TraceId, traces[0].TraceId, "verifica id trace 1");
                Assert.AreEqual(t3.TraceId, traces[1].TraceId, "verifica id trace 2");

                Delete(t2.TraceId);
                Delete(t3.TraceId);
            }
            
        }

        [TestMethod]
        public void CrudTest()
        {
            // trace da inserire
            var t = new Trace()
            {
                CorrelationId = System.Guid.NewGuid().ToString(),
                Description = "Test",
                Details = "Test",
                Level = 0,
                Module = "TraceServiceTest",
                Operation = "CrudTest",
                Origin = "UnitTest",
                TraceDate = System.DateTime.Now

            };



            // create
            int id = Create(t);

            // read
            t = Read(id);

            // update
            t = Update(t);

            // delete
            Delete(t.TraceId);

        }

        [TestMethod]
        public void MassiveTest()
        {
            int max = 1000;

            using (var client = new HttpClient())
            {
                for(int i = 0; i < max; i++)
                {
                    var t = new Trace()
                    {
                        CorrelationId = System.Guid.NewGuid().ToString(),
                        Description = "Massive insert",
                        Details = $"Massive insert # {i}",
                        Level = 0,
                        Module = "TraceServiceTest",
                        Operation = "MassiveTest",
                        Origin = "AlphaTest",
                        TraceDate = System.DateTime.Now

                    };

                    // serializza in json
                    string s = JsonConvert.SerializeObject(t);

                    // content da inviare
                    var content = new StringContent(s, System.Text.Encoding.UTF8, "application/json");

                    //client.PostAsync(BASE_URL + "/traces", content);

                    
                    // esegua la chiamata POST e recupera la response                    
                    HttpResponseMessage res = client.PostAsync(BASE_URL + "/traces", content).Result;

                    // assertion
                    Assert.IsNotNull(res, "La response è null");
                    Assert.IsTrue(res.IsSuccessStatusCode, "Status code: " + res.StatusCode);
                    

                }

            }
        }
        private int Create(Trace t)
        {

            using (var client = new HttpClient())
            {

                // serializza in json
                string s = JsonConvert.SerializeObject(t);

                // content da inviare
                var content = new StringContent(s, System.Text.Encoding.UTF8, "application/json");

                // esegua la chiamata POST e recupera la response
                HttpResponseMessage res = client.PostAsync(BASE_URL + "/traces", content).Result;

                // assertion
                Assert.IsNotNull(res, "La response è null");
                Assert.IsTrue(res.IsSuccessStatusCode, "Status code: " + res.StatusCode);

                // recupera il contenuto dalla response
                s = res.Content.ReadAsStringAsync().Result;

                // deseriallizza la response
                Trace t2 = JsonConvert.DeserializeObject<Trace>(s);

                // assertion
                Assert.IsNotNull(t2, "la response non contiene l'oggetto trace");
                Assert.IsTrue(t2.TraceId > 0, "TraceId non valorizzato");


                return t2.TraceId;
            }
        }

        private Trace Read(int traceId)
        {
            using (var client = new HttpClient())
            {
                var res = client.GetAsync(BASE_URL + "/traces/" + traceId).Result;

                Assert.IsNotNull(res);
                Assert.IsTrue(res.IsSuccessStatusCode);

                string s = res.Content.ReadAsStringAsync().Result;
                Trace trace = JsonConvert.DeserializeObject<Trace>(s);

                Assert.IsNotNull(trace, "la response non contiene l'oggetto trace");

                return trace;
            }

        }

        private Trace Update(Trace t)
        {
            using (var client = new HttpClient())
            {
                t.Description = "updated";

                // serializza in json
                string s = JsonConvert.SerializeObject(t);

                // content da inviare
                var content = new StringContent(s, System.Text.Encoding.UTF8, "application/json");

                // esegua la chiamata PUT e recupera la response
                HttpResponseMessage res = client.PutAsync(BASE_URL + "/traces", content).Result;

                // assertion
                Assert.IsNotNull(res, "La response è null");
                Assert.IsTrue(res.IsSuccessStatusCode, "Status code: " + res.StatusCode);

                // recupera il contenuto dalla response
                s = res.Content.ReadAsStringAsync().Result;

                // deseriallizza la response
                Trace t2 = JsonConvert.DeserializeObject<Trace>(s);

                // assertion
                Assert.IsNotNull(t2, "la response non contiene l'oggetto trace");
                Assert.AreEqual(t.TraceId, t2.TraceId, "TraceID non corretto");
                Assert.AreEqual(t.Description, t2.Description, "Description non corretta");

                return t2;
            }

        }

        private void Delete(int traceId)
        {
            using (var client = new HttpClient())
            {
                // esegua la chiamata DEL e recupera la response
                HttpResponseMessage res = client.DeleteAsync(BASE_URL + "/traces/" + traceId).Result;

                // assertion
                Assert.IsNotNull(res, "La response è null");
                Assert.IsTrue(res.IsSuccessStatusCode, "Status code: " + res.StatusCode);
            }

        }
    }
}
