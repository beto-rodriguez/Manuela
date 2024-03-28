namespace Manuela.Things;

public struct Region(int _X, int _Y, int _Width, int _Height)
{
    public int X = _X;
    public int Y = _Y;
    public int Width = _Width;
    public int Height = _Height;

#if WINDOWS
    public readonly Windows.Graphics.RectInt32 ToRectInt32() => new(X, Y, Width, Height);
#endif
}
