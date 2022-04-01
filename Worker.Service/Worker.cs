using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Worker.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private int loop = 0;
        private static HttpClient _httpClient;
        private static HttpClient HttpClient => _httpClient ?? (_httpClient = new HttpClient());


        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork();
            }
        }

        public async Task DoWork()
        {
            Interlocked.Increment(ref loop);
            _logger.LogInformation($"Worker printing number: {loop}");
            HttpResponseMessage response = HttpClient.GetAsync("https://httpbin.org/get").Result;
            await Task.Delay(100 * 2);
            HttpResponseMessage response2 = HttpClient.PostAsync("https://httpbin.org/post", new StringContent("")).Result;
        }
    }
}
