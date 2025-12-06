using System;
using System.Collections.Generic;

public class Program
{
    static void Main()
    {
        Console.WriteLine("====================================================");
        Console.WriteLine("   MESSAGE PROCESSOR – RETRY + DEAD LETTER QUEUE");
        Console.WriteLine("====================================================\n");

        // ======================================================================
        // DEMO SETUP
        // ======================================================================
        var retryPolicy = new RetryPolicy(maxRetryCount: 3);
        var deadLetterQueue = new DeadLetterQueue();
        var processor = new MessageProcessor(retryPolicy, deadLetterQueue);

        // Message 1: Transient error → will succeed after retries
        var msg1 = new Message
        {
            Content = "Transient message",
            IsTransientFailure = true
        };

        // Message 2: Permanent error → will be moved to DLQ
        var msg2 = new Message
        {
            Content = "Permanent failure message",
            IsTransientFailure = false
        };

        // ======================================================================
        // DEMO PROCESSING
        // ======================================================================
        Console.WriteLine("\n--- Processing Message 1 (Transient Failure) ---");
        processor.ProcessMessage(msg1);

        Console.WriteLine("\n--- Processing Message 2 (Permanent Failure) ---");
        processor.ProcessMessage(msg2);

        // ======================================================================
        // SHOW DEAD LETTER QUEUE CONTENT
        // ======================================================================
        Console.WriteLine("\n================ DEAD LETTER QUEUE ================");
        foreach (var m in deadLetterQueue.GetAllMessages())
        {
            Console.WriteLine($"DLQ Item: {m.Content}, Attempts: {m.AttemptCount}");
        }

        Console.WriteLine("\n====================== END =========================");
        Console.ReadKey();
    }
}

// ======================================================================
// MESSAGE MODEL
// ======================================================================
public class Message
{
    public string? Content { get; set; }
    public int AttemptCount { get; set; }
    public bool IsTransientFailure { get; set; }
}

// ======================================================================
// INTERFACES
// ======================================================================
public interface IMessageProcessor
{
    void ProcessMessage(Message message);
}

public interface IRetryPolicy
{
    bool ShouldRetry(int attemptCount);
}

// ======================================================================
// RETRY POLICY
// ======================================================================
public class RetryPolicy : IRetryPolicy
{
    private readonly int _maxRetryCount;

    public RetryPolicy(int maxRetryCount)
    {
        _maxRetryCount = maxRetryCount;
    }

    public bool ShouldRetry(int attemptCount) => attemptCount < _maxRetryCount;
}

// ======================================================================
// DEAD LETTER QUEUE
// ======================================================================
public class DeadLetterQueue
{
    private readonly List<Message> _queue = new();

    public void AddToQueue(Message message)
    {
        _queue.Add(message);
        Console.WriteLine($"[DLQ] Moved to dead-letter queue --> {message.Content}");
    }

    public List<Message> GetAllMessages() => _queue;
}

// ======================================================================
// MESSAGE PROCESSOR
// ======================================================================
public class MessageProcessor : IMessageProcessor
{
    private readonly IRetryPolicy _retryPolicy;
    private readonly DeadLetterQueue _deadLetterQueue;

    public MessageProcessor(IRetryPolicy retryPolicy, DeadLetterQueue deadLetterQueue)
    {
        _retryPolicy = retryPolicy;
        _deadLetterQueue = deadLetterQueue;
    }

    public void ProcessMessage(Message message)
    {
        while (_retryPolicy.ShouldRetry(message.AttemptCount))
        {
            try
            {
                Console.WriteLine($"Processing message: {message.Content}, Attempt: {message.AttemptCount + 1}");

                if (message.IsTransientFailure && message.AttemptCount < 2)
                    throw new Exception("Transient failure.");

                if (!message.IsTransientFailure)
                    throw new Exception("Permanent failure.");

                Console.WriteLine("Message processed successfully.");
                return;
            }
            catch (Exception ex)
            {
                message.AttemptCount++;
                Console.WriteLine($"Error: {ex.Message} --> Retry {message.AttemptCount}");

                if (!_retryPolicy.ShouldRetry(message.AttemptCount))
                {
                    _deadLetterQueue.AddToQueue(message);
                    return;
                }
            }
        }
    }
}

/*
 ====================================================
   MESSAGE PROCESSOR - RETRY + DEAD LETTER QUEUE
====================================================


--- Processing Message 1 (Transient Failure) ---
Processing message: Transient message, Attempt: 1
Error: Transient failure. --> Retry 1
Processing message: Transient message, Attempt: 2
Error: Transient failure. --> Retry 2
Processing message: Transient message, Attempt: 3
Message processed successfully.

--- Processing Message 2 (Permanent Failure) ---
Processing message: Permanent failure message, Attempt: 1
Error: Permanent failure. --> Retry 1
Processing message: Permanent failure message, Attempt: 2
Error: Permanent failure. --> Retry 2
Processing message: Permanent failure message, Attempt: 3
Error: Permanent failure. --> Retry 3
[DLQ] Moved to dead-letter queue --> Permanent failure message

================ DEAD LETTER QUEUE ================
DLQ Item: Permanent failure message, Attempts: 3

====================== END =========================
 */

/*
* 1.RetryPolicy: Determines if a message should be retried based on the maximum retry count.

* 2.DeadLetterQueue: Stores messages that fail after the maximum retries.

* 3.MessageProcessor: Processes the message, applies retry logic, and moves messages to the dead-letter queue if retries are exhausted.
 */