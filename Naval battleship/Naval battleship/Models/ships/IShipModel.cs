using System;
using System.Collections.Generic;
using System.Text;

namespace NavalBattleship.Models
{
    public interface IShipModel
    {
        void UpdateModel();

        void DrawName();
        
        void DrawModel();
    }
}
