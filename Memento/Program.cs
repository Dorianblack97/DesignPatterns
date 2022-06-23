//Client code
Originator originator = new Originator("Super-duper-super-puper-super.");
Caretaker caretaker = new Caretaker(originator);

caretaker.Backup();
originator.DoSomething();

caretaker.Backup();
originator.DoSomething();

caretaker.Backup();
originator.DoSomething();

Console.WriteLine();
caretaker.ShowHistory();

Console.WriteLine("\nClient: Now, let's rollback!\n");
caretaker.Undo();

Console.WriteLine("\n\nClient: Once more!\n");
caretaker.Undo();

Console.WriteLine();

// The Originator holds some important state that may change over time. It
// also defines a method for saving the state inside a memento and another
// method for restoring the state from it.
class Originator
{
    // For the sake of simplicity, the originator's state is stored inside a
    // single variable.
    private string _state;

    public Originator(string state)
    {
        _state = state;
        Console.WriteLine($"Originator: My initial state is: {state}");
    }

    // The Originator's business logic may affect its internal state.
    // Therefore, the client should backup the state before launching
    // methods of the business logic via the save() method.
    public void DoSomething()
    {
        Console.WriteLine("Originator: I'm doing something important");
        _state = GenerateRandomString(30);
        Console.WriteLine($"Originator: and my state has changed to: {_state}");
    }

    private string GenerateRandomString(int length = 30)
    {
        string allowedSymbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string result = string.Empty;

        while(length > 0)
        {
            result += allowedSymbols[new Random().Next(0, allowedSymbols.Length)];

            Thread.Sleep(12);

            length--;
        }
        return result;
    }

    // Saves the current state inside a memento.
    public IMemento Save() => new ConcreteMemento(_state);

    // Restores the Originator's state from a memento object.
    public void Restore(IMemento memento)
    {
        if(!(memento is ConcreteMemento))
        {
            throw new Exception($"Unknow memento class {memento.ToString()}");
        }
        _state = memento.GetState();
        Console.WriteLine($"Originator: My state has restored to: {_state}");
    }
}

// The Memento interface provides a way to retrieve the memento's metadata,
// such as creation date or name. However, it doesn't expose the
// Originator's state.
public interface IMemento
{
    string GetName();
    string GetState();
    DateTime GetDate();
}

// The Concrete Memento contains the infrastructure for storing the
// Originator's state.
record ConcreteMemento : IMemento
{
    private string _state { get; set; }
    private DateTime _date { get; set; }

    public ConcreteMemento(string state)
    {
        _state = state;
        _date = DateTime.Now;
    }

    // The Originator uses this method when restoring its state.
    public string GetState() => _state;

    // The rest of the methods are used by the Caretaker to display
    // metadata.
    public string GetName() => $"{_date} / ({_state.Substring(0, 9)})...";

    public DateTime GetDate() => _date;
}

// The Caretaker doesn't depend on the Concrete Memento class. Therefore, it
// doesn't have access to the originator's state, stored inside the memento.
// It works with all mementos via the base Memento interface.
class Caretaker
{
    private List<IMemento> _mementos = new List<IMemento>();
    private Originator _originator = null;

    public Caretaker(Originator originator)
    {
        _originator = originator;
    }

    public void Backup()
    {
        Console.WriteLine("\nCaretaker: Saving Originator's state...");
        _mementos.Add(_originator.Save());
    }

    public void Undo()
    {
        if(_mementos.Count == 0) return;

        var memento = _mementos.Last();
        _mementos.Remove(memento);

        Console.WriteLine($"Caretaker: Restoring state to: {memento.GetName()}");

        try
        {
            _originator.Restore(memento);
        }
        catch (Exception)
        {
            Undo();
        }
    }

    public void ShowHistory()
    {
        Console.WriteLine("Caretaker: Here's the list of mementos:");

        _mementos.ForEach(memento => Console.WriteLine(memento.GetName()));
    }
}