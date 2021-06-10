using Tiles.Components;
using ExtensionMethods;
using System.Collections.Generic;
using static PathDebugger;

public abstract class NotifyList 
{
    protected readonly List<TileComponent> list = new List<TileComponent>();

    public void Add(TileComponent component)    => list.AddUnique(component);
    public void Remove(TileComponent component) => list.Remove(component);

    public abstract void Notify();
}
//public class GlobalMoveFinishedNotifyList : NotifyList
//{
//    public override void Notify() => list.ForEach(c => c.GlobalSwipeFinish());
//}
//public class AllMovesFinisedNotifyList : NotifyList
//{
//    //public override void Notify() => list.ForEach(c => c.AllMovesFinished());
//}