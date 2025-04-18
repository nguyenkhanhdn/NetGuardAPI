using Microsoft.AspNetCore.Mvc;

namespace NetGuardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SEController : ControllerBase
    {
        //api/SE/Predict?text=a
        [HttpGet("Predict")]
        public async Task<ActionResult<SentimentPrediction>> Get(string text)
        {
            SentimentPrediction prediction = new SentimentPrediction();
            SentimentEngine sentimentEngine = new SentimentEngine();
            try
            {
                prediction = await Task.Run(() => sentimentEngine.Predict(text));
            }
            catch (FileNotFoundException fnf)
            {
                sentimentEngine.TrainingModel();
                prediction = sentimentEngine.Predict(text);
                Console.WriteLine(fnf.Message);
            }
            return Ok(prediction);
        }
        // GET api/<SEController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SEController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SEController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SEController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
