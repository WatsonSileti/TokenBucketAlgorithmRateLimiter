using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenBucketRateLimiter
{
    public class Ratelimiter
    {
        private readonly int _capacity; // Max tokens in the bucket
        private readonly TimeSpan _refillInterval; // Time between each token refill
        private int _tokens; // Current number of tokens in the bucket
        private DateTime _lastRefill; // Last time tokens were refilled
        private readonly object _lock = new object(); // To make thread-safe

        public Ratelimiter(int capacity, TimeSpan refillInterval) 
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
                // Calculate how many tokens to add since the last request
                var now = DateTime.UtcNow;
                var timeElapsed = now - _lastRefill;

                int refillTokens = (int)(timeElapsed.TotalSeconds/ _refillInterval.TotalSeconds);
                if (refillTokens > 0)
                {
                    _tokens = Math.Min(_capacity, _tokens + refillTokens);
                    _lastRefill = now;
                }

                if (_tokens > 0)
                {
                    _tokens--; // Consume a token
                    return true; // Request is allowed
                }

                return false; // Request is denied (bucket is empty)
            }
        }
    }
}
