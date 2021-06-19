using Tiles.Modules;
using ExtensionMethods;
using System.Collections.Generic;
using static PathDebugger;

public abstract class NotifyList 
{
    protected readonly List<TileModule> list = new List<TileModule>();

    public void Add(TileModule component)    => list.AddUnique(component);
    public void Remove(TileModule component) => list.Remove(component);

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