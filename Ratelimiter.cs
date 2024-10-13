using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenBucketRateLimiter
{
    public class RateLimiter
    {
        private readonly int _capacity; // Max tokens in the bucket
        private readonly TimeSpan _refillInterval; // Time between each token refill
        private int _tokens; // Current number of tokens in the bucket
        private DateTime _lastRefill; // Last time tokens were refilled
        private readonly object _lock = new object(); // To make thread-safe

        public RateLimiter(int capacity, TimeSpan refillInterval) 
        { 
            _capacity = capacity;
            _refillInterval = refillInterval;
            _tokens = capacity; // Start with a full bucket
            _lastRefill = DateTime.UtcNow;
        }

        public bool AllowRequest()
        {
            lock (_lock)
            {
                RefillTokens(); // Refill tokens before checking availability

                if (_tokens > 0)
                {
                    _tokens--; // Consume one token
                    Console.WriteLine($"Request allowed. Remaining tokens: {_tokens}");
                    return true;
                }
                else
                {
                    Console.WriteLine("Too many requests. Please try again later.");
                    return false;
                }
            }
        }

        // Refill tokens based on the time elapsed since the last refill
        private void RefillTokens()
        {
            var now = DateTime.UtcNow;
            var timeSinceLastRefill = now - _lastRefill;

            // Calculate how many tokens should be refilled
            int refillCount = (int)(timeSinceLastRefill.TotalSeconds / _refillInterval.TotalSeconds);

            if (refillCount > 0)
            {
                _tokens = Math.Min(_capacity, _tokens + refillCount); // Refill tokens, but don't exceed capacity
                _lastRefill = now; // Update the last refill time
            }
        }
    }
}
