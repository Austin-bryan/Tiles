﻿public interface IPlayerTile
{
    void FinishSwipe(bool isNewTile);
    void Activate(bool wasPlayerActivated);
    void BeginSwipe(Direction direction, bool playerTriggered);
    void Swipe(Direction direction, bool? playerTriggered, bool shouldSpawnOpposite, bool wasObstructed);
    void AllMovesFinished();
}