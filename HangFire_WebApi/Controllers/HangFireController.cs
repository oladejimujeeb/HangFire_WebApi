using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangFire_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HangFireController : ControllerBase
    {

        [HttpPost]
        [Route("[Action]")]
        public IActionResult WelcomeMesaage()
        {//Fire and Forget
            var jobId = BackgroundJob.Enqueue(() => SendWelcomeEmail("Welcome to our App"));
            return Ok($"JobId: {jobId}, Welcome mail sent to user ");
        }
        public void SendWelcomeEmail(string mail)
        {
            Console.WriteLine(mail);
        }
        [HttpPost]
        [Route("[Action]")]
        public IActionResult Discount()
        {
            //schudule job
            int timeInSeconds = 30;
            var jobId = BackgroundJob.Schedule(() => SendWelcomeEmail("Discount for the month"), TimeSpan.FromSeconds(timeInSeconds));
            return Ok($"JobId: {jobId}, Discount mail will be sent to user in {timeInSeconds}secs ");
        }

        [HttpPost]
        [Route("[Action]")]
        public IActionResult DataBaseUpdate()
        {//reoccurrence job
             RecurringJob.AddOrUpdate(() => Console.WriteLine("DataBase Update"), Cron.Minutely);
             return Ok("Database Check job Initiated");
        }

        [HttpPost]
        [Route("[Action]")]
        public IActionResult Confirm()
        {//continuos jobs
            int timeInSeconds = 30;
            var parentJobId = BackgroundJob.Schedule(() => Console.WriteLine("You want to unsubscribe"), TimeSpan.FromSeconds(timeInSeconds));
            BackgroundJob.ContinueJobWith(parentJobId, () => Console.WriteLine("You are now unsubscribed"));
            return Ok("Confirmation Job Created");
        }
    }
}
 