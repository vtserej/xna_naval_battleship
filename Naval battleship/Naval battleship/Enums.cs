using System;
using System.Collections.Generic;
using System.Text;

namespace NavalBattleship
{
    public enum Player : int
    { Person = 0, Computer = 1, None = 3 };

    public enum EScreen : int
    { MainScreen = 0, GameScreen = 1, CreditsScreen = 2, SetupScreen = 3, 
      MenuScreen = 4, WinScreen = 5};

    public enum DScreen : int
    {LeaveScreen = 0, NewGame =1 };

    public enum Position : int
    {Vertical = 0, Horizontal = 1 };
}
