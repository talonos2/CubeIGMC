
using System;

internal abstract class Mover
{
    internal abstract bool GetInput(MoverCommand command);
    

    internal abstract void Tick(bool justExitedMenu);
}