using System;
using System.Collections.Generic;
using System.Text;

namespace NavalBattleship
{
    public class AI //inteligencia artificial del juego, version 0.5
    {
        Game game;
        Random Randomizer = new Random();
        AICoordenada [] around = new AICoordenada[4];
        AICoordenada lastEfectiveShot = new AICoordenada();
        List<AICoordenada> shootlist = new List<AICoordenada>();
        int continuedShots;
        bool lasthit;
        bool sinking;

        public AI(Game Juego)
        {
            this.game = Juego;
            for (int i = 0; i < around.Length ; i++)
            {
                around[i] = new AICoordenada();  
            }
        }

        public void Reset()
        {
            shootlist.Clear();
            lastEfectiveShot = new AICoordenada();
            continuedShots = 0;
            lasthit = false;
            sinking = false; 
        }

        public void LastHit(bool value)
        {
                lasthit = value;
                if (lasthit)
                {
                    continuedShots++;
                    sinking = true;
                }
                else
                {
                    continuedShots = 0;
                }
        }

        public AICoordenada AICoordinate()
        {
            AICoordenada tiro = new AICoordenada();

            //si el tiro anterior no fue efectivo y no estoy hundiendo ningun barco
            if (lasthit == false && sinking == false)
            {
                tiro = CoordenadasRandom();
            }
            else
            {
                //si el ultimo tiro dio en el blanco tomo sus coordenadas
                if (continuedShots != 0)
                {
                    lastEfectiveShot = shootlist[shootlist.Count-1];
                }
                int x = lastEfectiveShot.x;
                int y = lastEfectiveShot.y;
              
                if (FiltradoCoordenadas(x, y,ref tiro) == false)
                {
                    tiro = CoordenadasRandom();
                    sinking = false; 
                }
            }
            shootlist.Add(tiro);
            return tiro;
        }

        AICoordenada CoordenadasRandom()
        {
            AICoordenada tiro = new AICoordenada();
            int x, y;
            x = Randomizer.Next(10);
            y = Randomizer.Next(15);
            int cheat = Randomizer.Next(6);
            if (cheat == 0)
            {
                while (game.GetCoordenada(x, y, 0) == 7 || game.GetCoordenada(x, y, 0) == 0 
                                                         || game.GetCoordenada(x, y, 0) == 9)
                {
                    x = Randomizer.Next(10);
                    y = Randomizer.Next(15);
                }
            }
            else
            {
                while (game.GetCoordenada(x, y, 0) == 7 || this.Hole(x, y) == true ||
                       game.GetCoordenada(x, y, 0) == 9 || this.Side(x, y) == true)
                {
                    x = Randomizer.Next(10);
                    y = Randomizer.Next(15);
                }
            }          
            tiro.x = x;
            tiro.y = y;
            return tiro;
        }

        void Around(int x, int y)
        {
            around[0].value = game.GetCoordenada(x, y - 1, 0);
            around[0].x = x;
            around[0].y = y - 1; 

            around[1].value = game.GetCoordenada(x + 1, y, 0);
            around[1].x = x + 1;
            around[1].y = y; 

            around[2].value = game.GetCoordenada(x, y + 1, 0);
            around[2].x = x;
            around[2].y = y + 1; 

            around[3].value = game.GetCoordenada(x - 1, y, 0);
            around[3].x = x - 1;
            around[3].y = y;
        }

        bool FiltradoCoordenadas(int x, int y,ref AICoordenada tiro)
        {            
            bool found = false;
            //tomo las coordenadas alrededor del punto
            this.Around(x, y);
            //busco si hay algun lugar para disparar
            foreach (AICoordenada  item in around)
            {
                if (item.value != 7 && item.value != 9 && item.value != 10)
                {
                    found = true;
                    break; 
                }
            }
            //si hay un lugar
            if (found)
            {
                int shotcount = 0;

                //busco si alrededor ya hay un disparo efectivo
                foreach (var item in around )
                {
                    if (item.value == 9)
                    {
                        shotcount++; 
                    }
                }
                //si lo hay busco su correspondiente(der-izqda , arriba-abajo) 
                if (shotcount == 1)
                {
                    return ChooseCorrespondiente(ref tiro); 
                }
                //si no escojo cualquiera
                else
                {
                    if (shotcount == 2)
                    {
                        return false;
                    }
                    ChooseAnyCoordenada(ref x, ref y, ref tiro);
                    return true;
                }
            }
            //si no retorno false
            else
            {
                return false;
            }
        }

        bool ChooseAnyCoordenada(ref int x, ref int y, ref AICoordenada tiro)
        {
            //escoje cualquier coordenada alrededor de un punto
            int j = this.Randomizer.Next(4);

            while (around[j].value == 7 || around[j].value == 9 || around[j].value == 10)
            {
                j = this.Randomizer.Next(4);
            }
            tiro.x = around[j].x;
            tiro.y = around[j].y;
            return true;
        }

        bool ChooseCorrespondiente(ref AICoordenada tiro)
        {
            int sign = 2;
            bool found = false;

            for (int i = 0; i < around.Length; i++)
            {
                if (i == 2)
                {
                    sign *= -1;
                }
                if (around[i].value == 9 && (around[i + sign].value != 7
                                             && around[i + sign].value != 10))
                {
                    tiro.x = around[i + sign].x;
                    tiro.y = around[i + sign].y;
                    found = true; 
                    break;
                }
            }
            return found;
        }

        bool Hole(int x, int y)
        {
            //rechaza un tiro si todo a su alrededor esta bombardeado
            bool hole = false;
            this.Around(x, y);
            foreach (var item in around)
            {
                if (item.value != 7)
                {
                    hole = false;
                }
            }
            return hole;
        }

        bool Side(int x, int y)
        {
            //rechaza un tiro si a su alrededor hay otro tiro
            bool side = false;
            this.Around(x, y);
            foreach (var item in around)
            {
                if (item.value == 9)
                {
                    side = true;
                }
            }
            return side;
        }
    }
}
  
