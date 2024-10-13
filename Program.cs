// See https://aka.ms/new-console-template for more information
using System;
using TokenBucketRateLimiter;

// Create a rate limiter that allows 5 requests every 10 seconds
int capacity = 5;
TimeSpan timeSpan = TimeSpan.FromSeconds(2); // Time to refill one token

RateLimiter rateLimiter = new RateLimiter(capacity, timeSpan);

Console.WriteLine("Press Enter to send a request. Ctrl+C to exit.\n");

// Simulate sending requests
//for (int i = 0; i < 10; i++) // Simulate making 10 requests
//{
//    Console.WriteLine($"Attemptting request {i  + 1}");
//    rateLimiter.AllowRequest();
//    Thread.Sleep(1000);// Wait 1 second between requests
//}

//Console.WriteLine("Finished sending requests.");

Task[] tasks = new Task[10];

for (int i = 0;i < tasks.Length; i++)
{
    int requestId = i + 1;

    tasks[i] = Task.Run(() =>
    {
        Console.WriteLine($"Request {requestId} attempting...");
        bool allowed = rateLimiter.AllowRequest();
        Console.WriteLine($"Request {requestId}: {(allowed ? "Allowed" : "Denied")}");
    });


    Thread.Sleep(500); // Wait 500ms before the next request (optional)
}

Task.WaitAll(tasks);
Console.WriteLine("Finished sending concurrent requests.");