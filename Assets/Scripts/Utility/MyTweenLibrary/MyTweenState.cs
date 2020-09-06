namespace Utility.MyTweenLibrary
{
    public abstract class MyTweenState
    {
        public bool IsTweening { get; protected set; }

        public abstract bool Tween();
    }
}
