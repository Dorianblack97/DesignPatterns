// The client code
var subject = new Subject();
var observerA = new ConcreteObserverA();
subject.Attach(observerA);

var observerB = new ConcreteObserverB();
subject.Attach(observerB);

subject.SomeBusinessLogic();
subject.SomeBusinessLogic();

subject.Detach(observerB);
subject.SomeBusinessLogic();

public interface IObserver
{
    // Receive update from subject
    void Update(ISubject subject);
}

public interface ISubject
{
    // Attach an observer to the subject.
    void Attach(IObserver observer);

    // Detach an observer to the subject.
    void Detach(IObserver observer);

    // Notify all observer about an event.
    void Notify();
}

// The subject owns some important state and notfies observers when the
// state changes.
public class Subject : ISubject
{
    // For the sake of simplicity, the Subject's state, essential to all
    // subscrivers, is stored in this variable.
    public int State { get; set; } = -0;

    // List of subscribers. In real life, the list of subscribers can be
    // stored more comprehensively (categorized by event type, etc..).
    public List<IObserver> _observers = new List<IObserver>();

    // The subscription management methods.
    public void Attach(IObserver observer)
    {
        Console.WriteLine("Subject: Attached an observer.");
        _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        Console.WriteLine("Subject: Detached an observer.");
        _observers.Remove(observer);
    }

    // Trigger an update in each subcriber.
    public void Notify()
    {
        Console.WriteLine("Subject: Notifying observers...");

        _observers.ForEach(observer => observer.Update(this));
    }

    // Usually, the subscription logic is only a fraction of what a Subject
    // can really do. Subjects commonly hold some important business logic,
    // that triggers a notification method whenever sometthin important is
    // about to happen (or after it).
    public void SomeBusinessLogic()
    {
        Console.WriteLine("\nSubject: I'm doing something important.");
        State = new Random().Next(0, 10);

        Thread.Sleep(15);

        Console.WriteLine("Subject: My state ha just changed to: " + State);
        Notify();
    }
}

// Concrete Observers react to the updates issued by the Subject they had
// been attached to.
class ConcreteObserverA : IObserver
{
    public void Update(ISubject subject)
    {
        if ((subject as Subject).State < 3) Console.WriteLine("ConcreteObserverA: Reacted to the event.");
    }
}

class ConcreteObserverB : IObserver
{
    public void Update(ISubject subject)
    {
        if ((subject as Subject).State == 0 || (subject as Subject).State >= 2) 
            Console.WriteLine("ConcreteObserverB: Reacted to the event"); 
    }
}