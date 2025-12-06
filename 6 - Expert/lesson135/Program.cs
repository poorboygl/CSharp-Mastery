using System;

public class Program
{
    static void Main()
    {
        Console.WriteLine("====================================================");
        Console.WriteLine("              CIRCUIT BREAKER DEMO");
        Console.WriteLine("====================================================\n");

        // ======================================================================
        // CIRCUIT BREAKER CONFIGURATION
        // ======================================================================
        var circuitBreaker = new CircuitBreaker(
            failureThreshold: 3,
            cooldownPeriod: TimeSpan.FromSeconds(5)
        );

        var service = new ExternalService(circuitBreaker);

        // ======================================================================
        // DEMO – EXECUTE MULTIPLE REQUESTS
        // ======================================================================
        for (int i = 1; i <= 10; i++)
        {
            Console.WriteLine($"\n--- Request #{i} ---");
            service.MakeRequest();
            Thread.Sleep(800); // Delay between requests for visibility
        }

        Console.WriteLine("\n====================== END =========================");
        Console.ReadKey();
    }
}

// ======================================================================
// ENUM: CIRCUIT BREAKER STATES
// ======================================================================
public enum CircuitBreakerState
{
    Closed,
    Open,
    HalfOpen
}

// ======================================================================
// CIRCUIT BREAKER IMPLEMENTATION
// ======================================================================
public class CircuitBreaker
{
    private CircuitBreakerState _state;
    private int _failureCount;
    private readonly int _failureThreshold;
    private readonly TimeSpan _cooldownPeriod;
    private DateTime _lastFailureTime;

    public CircuitBreaker(int failureThreshold, TimeSpan cooldownPeriod)
    {
        _state = CircuitBreakerState.Closed;
        _failureThreshold = failureThreshold;
        _cooldownPeriod = cooldownPeriod;
        _failureCount = 0;
    }

    public void Execute(Action action)
    {
        if (_state == CircuitBreakerState.Open)
        {
            if (DateTime.UtcNow - _lastFailureTime > _cooldownPeriod)
            {
                _state = CircuitBreakerState.HalfOpen;
                Console.WriteLine("[Circuit] Transition --> HALF-OPEN.");
            }
            else
            {
                Console.WriteLine("[Circuit] OPEN --> Request BLOCKED.");
                return;
            }
        }

        try
        {
            action.Invoke();
            Reset();
            Console.WriteLine("[Circuit] Request SUCCEEDED.");
        }
        catch (Exception)
        {
            HandleFailure();
        }
    }

    private void Reset()
    {
        _failureCount = 0;
        _state = CircuitBreakerState.Closed;
        Console.WriteLine("[Circuit] CLOSED --> Failure count reset.");
    }

    private void HandleFailure()
    {
        _failureCount++;
        Console.WriteLine($"[Circuit] Request FAILED. Failure count: {_failureCount}");

        if (_state == CircuitBreakerState.HalfOpen || _failureCount >= _failureThreshold)
        {
            _state = CircuitBreakerState.Open;
            _lastFailureTime = DateTime.UtcNow;
            Console.WriteLine("[Circuit] OPEN --> Requests will be blocked.");
        }
    }
}

// ======================================================================
// EXTERNAL SERVICE SIMULATION
// ======================================================================
public class ExternalService
{
    private readonly CircuitBreaker _circuitBreaker;
    private readonly Random _random = new();

    public ExternalService(CircuitBreaker circuitBreaker)
    {
        _circuitBreaker = circuitBreaker;
    }

    public void MakeRequest()
    {
        _circuitBreaker.Execute(() =>
        {
            SimulateRequest();
        });
    }

    private void SimulateRequest()
    {
        if (_random.Next(0, 2) == 0) // 50% chance failure
            throw new Exception("External service FAILED.");

        Console.WriteLine("[Service] Response received successfully.");
    }
}

/*
 ====================================================
              CIRCUIT BREAKER DEMO
====================================================


--- Request #1 ---
[Service] Response received successfully.
[Circuit] CLOSED --> Failure count reset.
[Circuit] Request SUCCEEDED.

--- Request #2 ---
[Service] Response received successfully.
[Circuit] CLOSED --> Failure count reset.
[Circuit] Request SUCCEEDED.

--- Request #3 ---
[Circuit] Request FAILED. Failure count: 1

--- Request #4 ---
[Service] Response received successfully.
[Circuit] CLOSED --> Failure count reset.
[Circuit] Request SUCCEEDED.

--- Request #5 ---
[Service] Response received successfully.
[Circuit] CLOSED --> Failure count reset.
[Circuit] Request SUCCEEDED.

--- Request #6 ---
[Service] Response received successfully.
[Circuit] CLOSED --> Failure count reset.
[Circuit] Request SUCCEEDED.

--- Request #7 ---
[Circuit] Request FAILED. Failure count: 1

--- Request #8 ---
[Circuit] Request FAILED. Failure count: 2

--- Request #9 ---
[Service] Response received successfully.
[Circuit] CLOSED --> Failure count reset.
[Circuit] Request SUCCEEDED.

--- Request #10 ---
[Circuit] Request FAILED. Failure count: 1

====================== END =========================
 */

/*
* 1.CircuitBreaker: Manages the circuit state, tracking failures and transitioning based on the failure count and cooldown period.

* 2.ExternalService: Uses the circuit breaker to control service requests, simulating random failures and logging results.
 */