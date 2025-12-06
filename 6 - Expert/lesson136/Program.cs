using System;
using System.Collections.Generic;

public class Program
{
    static void Main()
    {
        // ======================================================================
        // DEMO: TWO PHASE COMMIT TRANSACTION
        // ======================================================================
        var participants = new List<ITransactionParticipant>
        {
            new PaymentService(),
            new InventoryService(),
            new NotificationService()
        };

        var coordinator = new TransactionCoordinator(participants);
        coordinator.ExecuteTransaction();

        Console.ReadKey();
    }
}

// ======================================================================
// INTERFACE: TRANSACTION PARTICIPANT
// ======================================================================
public interface ITransactionParticipant
{
    bool Prepare();   // Pha 1
    void Commit();    // Pha 2
    void Rollback();  // Rollback khi lỗi
}

// ======================================================================
// PAYMENT SERVICE
// ======================================================================
public class PaymentService : ITransactionParticipant
{
    public bool Prepare()
    {
        Console.WriteLine("PaymentService: Preparing transaction.");
        return true; // Luôn thành công cho demo
    }

    public void Commit()
    {
        Console.WriteLine("PaymentService: Committing transaction.");
    }

    public void Rollback()
    {
        Console.WriteLine("PaymentService: Rolling back transaction.");
    }
}

// ======================================================================
// INVENTORY SERVICE
// ======================================================================
public class InventoryService : ITransactionParticipant
{
    public bool Prepare()
    {
        Console.WriteLine("InventoryService: Preparing transaction.");
        return true;
    }

    public void Commit()
    {
        Console.WriteLine("InventoryService: Committing transaction.");
    }

    public void Rollback()
    {
        Console.WriteLine("InventoryService: Rolling back transaction.");
    }
}

// ======================================================================
// NOTIFICATION SERVICE
// ======================================================================
public class NotificationService : ITransactionParticipant
{
    public bool Prepare()
    {
        Console.WriteLine("NotificationService: Preparing transaction.");
        return true;
    }

    public void Commit()
    {
        Console.WriteLine("NotificationService: Committing transaction.");
    }

    public void Rollback()
    {
        Console.WriteLine("NotificationService: Rolling back transaction.");
    }
}

// ======================================================================
// TRANSACTION COORDINATOR (2PC)
// ======================================================================
public class TransactionCoordinator
{
    private readonly List<ITransactionParticipant> _participants;

    public TransactionCoordinator(List<ITransactionParticipant> participants)
    {
        _participants = participants;
    }

    public bool ExecuteTransaction()
    {
        Console.WriteLine("TransactionCoordinator: Beginning transaction.");

        // Phase 1: PREPARE
        foreach (var participant in _participants)
        {
            if (!participant.Prepare())
            {
                Console.WriteLine("TransactionCoordinator: Prepare failed, rolling back.");
                Rollback();
                return false;
            }
        }

        // Phase 2: COMMIT
        Commit();
        Console.WriteLine("TransactionCoordinator: Transaction committed successfully.");
        return true;
    }

    private void Commit()
    {
        foreach (var participant in _participants)
        {
            participant.Commit();
        }
    }

    private void Rollback()
    {
        foreach (var participant in _participants)
        {
            participant.Rollback();
        }
    }
}
/*
TransactionCoordinator: Beginning transaction.
PaymentService: Preparing transaction.
InventoryService: Preparing transaction.
NotificationService: Preparing transaction.
PaymentService: Committing transaction.
InventoryService: Committing transaction.
NotificationService: Committing transaction.
TransactionCoordinator: Transaction committed successfully.
 */

/*
* 1.ITransactionParticipant: Defines the contract for services participating in the transaction, including Prepare, Commit, and Rollback methods.

* 2.TransactionCoordinator: Manages the two-phase commit protocol, calling Prepare on each service in phase 1 and either Commit or Rollback in phase 2.

* 3.Services (PaymentService, InventoryService, NotificationService): Implement transaction logic, each with their own Prepare, Commit, and Rollback methods.
 */