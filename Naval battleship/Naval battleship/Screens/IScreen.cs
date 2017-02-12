using System;
using System.Collections.Generic;
using System.Text;
using NavalBattleship.Controls;
using Xengine.WindowsControls; 

namespace NavalBattleship.Screens
{
    public interface IScreen
    {        
        void Update(Cursor cursor);

        void Quit();

        void Draw();
    }
}
