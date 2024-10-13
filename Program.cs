// See https://aka.ms/new-console-template for more information
using TokenBucketRateLimiter;

// Create a rate limiter that allows 5 requests every 10 seconds
int capacity = 5;
TimeSpan timeSpan = TimeSpan.FromSeconds(2); // Time to refill one token

RateLimiter rateLimiter = new RateLimiter(capacity, timeSpan);

Console.WriteLine("Press Enter to send a request. Ctrl+C to exit.\n");

// Simulate sending requests
for (int i = 0; i < 10; i++) // Simulate making 10 requests
{
    Console.WriteLine($"Attemptting request {i  + 1}");
    rateLimiter.AllowRequest();
    Thread.Sleep(1000);// Wait 1 second between requests
}

Console.WriteLine("Finished sending requests.");