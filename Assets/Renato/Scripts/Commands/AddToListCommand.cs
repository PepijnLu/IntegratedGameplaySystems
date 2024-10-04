using System.Collections.Generic;

public class AddToListCommand<T, TIdentifiable> : ICommand where TIdentifiable : IIdentifiable
{
    private ConcreteComponentAdd<T> component;
    private readonly List<T> list;
    private readonly T t;

    public AddToListCommand
    (
        List<T> list,
        T t
    ) 
    {
        this.list = list;
        this.t = t;

        CustomAwake();
    }

    public void CustomAwake() 
    {
        component = new(list, t);
    }

    public void Execute()
    {
        component.AddToList();
    }

    public void Undo()
    {
        component.DropFromList();
    }
}
