using System.Collections;

// The client code may or may not know about the Concrete Iterator
// or Collection classes, depending on the level of indirection you
// want to keep in your program.
var collection = new WordsCollection();
collection.AddItem("First");
collection.AddItem("Second");
collection.AddItem("Third");

Console.WriteLine("Straight traversal:");

foreach (var word in collection)
{
    Console.WriteLine(word);
}

Console.WriteLine("\nReverse traversal:");

collection.ReverseDirection();

foreach (var word in collection)
{
    Console.WriteLine(word);
}

abstract class Iterator : IEnumerator
{
    object IEnumerator.Current => Current();

    //Returns the key of the current element
    public abstract int Key();

    //Returns the current element
    public abstract object Current();

    //Move forward to next element
    public abstract bool MoveNext();

    //Rewinds the Iterator to the first element
    public abstract void Reset();
}

abstract class IteratorAggregate : IEnumerable
{
    // Returns an Iterator or another IteratorAggregate for the implementing
    // object.
    public abstract IEnumerator GetEnumerator();
}

// Concrete Iterators implement various traversal algorithms. These classes
// store the current traversal position at all times.
class AlphabeticalOrderIterator : Iterator
{
    private WordsCollection _collection;

    // Stores the current traversal position. An iterator may have a lot of
    // other fields for storing iteration state, especially when it is
    // supposed to work with a particular kind of collection.
    private int _position = -1;
    private bool _reverse = false;

    public AlphabeticalOrderIterator(WordsCollection collection, bool reverse = false)
    {
        _collection = collection;
        _reverse = reverse;

        if (_reverse) _position = collection.getItems().Count;
    }

    public override object Current() => _collection.getItems()[_position];

    public override int Key() => _position;

    public override bool MoveNext()
    {
        int updatePosition = _position + (_reverse ? -1 : 1);

        if (updatePosition >= 0 && updatePosition < _collection.getItems().Count)
        {
            _position = updatePosition;
            return true;
        }
        else return false;
    }

    public override void Reset()
    {
        _position = _reverse ? _collection.getItems().Count - 1 : 0;
    }
}

// Concrete Collections provide one or several methods for retrieving fresh
// iterator instances, compatible with the collection class.
class WordsCollection : IteratorAggregate
{
    List<string> _collection = new List<string>();
    bool _direction = false;

    public void ReverseDirection() => _direction = !_direction;

    public List<string> getItems() => _collection;

    public void AddItem(string item) => _collection.Add(item);

    public override IEnumerator GetEnumerator() => new AlphabeticalOrderIterator(this, _direction);
}