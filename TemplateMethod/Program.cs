Console.WriteLine("Same client code can work with different subclasses:");
Client.ClientCode(new ConcreteClass1());

Console.WriteLine(Environment.NewLine);

Console.WriteLine("Same client code can work with different subclasses:");
Client.ClientCode(new ConcreteClass2());

// The Abstract Class defines a template method that contains a skeleton of
// some algorithm, composed of calls to (usually) abstract primitive
// operations.
//
// Concrete subclasses should implement these operations, but leave the
// template method itself intact.
abstract class AbstractClass
{
    // The template method defines the skeleton of an algorithm.
    public void TemplateMethod()
    {
        BaseOperation1();
        RequiredOperations1();
        BaseOperation2();
        Hook1();
        RequiredOperations2();
        BaseOperation3();
        Hook2();
    }

    // These operations already have implementations.
    protected void BaseOperation1() => Console.WriteLine("AbstractClass says: I am doing the bulk of the work");
    protected void BaseOperation2() => Console.WriteLine("AbstractClass says: But I let subclassess override some operations");
    protected void BaseOperation3() => Console.WriteLine("AbstractClass says: But I am doing the bulk of the work aniway");

    // These operations have to be implemented in subclasses.
    protected abstract void RequiredOperations1();
    protected abstract void RequiredOperations2();

    // These are "hooks." Subclasses may override them, but it's not
    // mandatory since the hooks already have default (but empty)
    // implementation. Hooks provide additional extension points in some
    // crucial places of the algorithm.
    protected virtual void Hook1() { }
    protected virtual void Hook2() { }
}

// Concrete classes have to implement all abstract operations of the base
// class. They can also override some operations with a default
// implementation.
class ConcreteClass1 : AbstractClass
{
    protected override void RequiredOperations1() => Console.WriteLine("ConcreteClass1 says: Implemented Operation1");
    protected override void RequiredOperations2() => Console.WriteLine("ConcreteClass1 says: Implemented Operation2");
}

// Usually, concrete classes override only a fraction of base class'
// operations.
class ConcreteClass2 : AbstractClass
{
    protected override void RequiredOperations1() => Console.WriteLine("ConcreteClass2 says: Implemented Operation1");
    protected override void RequiredOperations2() => Console.WriteLine("ConcreteClass2 says: Implemented Operation2");
    protected override void Hook1() => Console.WriteLine("ConcreteClass2 says: Overridden Hook1");
}

class Client
{
    // The client code calls the template method to execute the algorithm.
    // Client code does not have to know the concrete class of an object it
    // works with, as long as it works with objects through the interface of
    // their base class.
    public static void ClientCode(AbstractClass abstractClass)
    {
        // ...
        abstractClass.TemplateMethod();
        // ...
    }
}