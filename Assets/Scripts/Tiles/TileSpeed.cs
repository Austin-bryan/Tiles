// SwipeSpeed: 0.7, OBstructSpeed: 0.4

public struct TileSpeed
{
    public float CurrentSpeed { get; private set; }
    public void UpdateObstruction(bool isObstructed) => CurrentSpeed = isObstructed ? obstructedSwipeSpeed : swipeSpeed;
    private const float swipeSpeed = 0.7f, obstructedSwipeSpeed = 0.4f;
}
