using System;
using System.Collections.Generic;
using NavalBattleship.Models;
using System.IO;
using NavalBattleship.GameCode;

namespace NavalBattleship
{
    public class Game
    {
        #region Game Constants

        const int boardSeparation = 11; 
        const float boardAltitude = 0.10f;
        const float gravity = 9.8f;

        #endregion

        #region Private atributes

        string playerName;
        string finishMessage = "";
        Player currentPlayer = Player.Person;
        Player winner = Player.None;
        AI computerAi;
        Board[] board = new Board[2];
        Ship[] shipsPlayer = new Ship[6]; 
        Ship[] shipsComputer = new Ship[6];
        static GameStrings gameStrings = new GameStrings();
        static Random randomizer = new Random();
        Escena escena;
        static bool finished;
    
        #endregion

        #region Properties

        public static bool Finished
        {
            get { return Game.finished; }
            set { Game.finished = value; }
        }

        public AI ComputerAi
        {
            get { return computerAi; }
            set { computerAi = value; }
        } 

        public Player CurrentPlayer
        {
            get { return currentPlayer; }
            set { currentPlayer = value; }
        }

        public string FinishMessage
        {
            get { return finishMessage; }
            set { finishMessage = value; }
        }

        public Player Winner
        {
            get { return winner; }
            set { winner = value; }
        }  

        public Board[] BoardGame
        {
            get { return board; }
            set { board = value; }
        }

        public Ship[] ShipsPlayer
        {
            get { return shipsPlayer; }
            set { shipsPlayer = value; }
        }

        public Ship[] ShipsComputer
        {
            get { return shipsComputer; }
            set { shipsComputer = value; }
        }

        public void PlayerAllocation()
        {
            RandomAllocation(0); 
        }

        static public int BoardSeparation
        {
            get { return boardSeparation; }
        }

        static public float BoardAltitude
        {
            get { return boardAltitude; }
        }

        static public float Gravity
        {
            get { return gravity; }
        } 

        public string PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }

        static public GameStrings GameStrings
        {
            get { return gameStrings; }
        }

        public Escena Escena
        {
            set { escena = value; }
        }


        #endregion

        public Game()
        {
            board[0] = new Board();
            board[0].Player = Player.Person;
            board[1] = new Board();
            board[1].Player = Player.Computer;
            computerAi = new AI(this);
            gameStrings.ReadLanguage();  
            gameStrings.CreateStrings();   
        }

        public Coordenada2D GetLaunchCoord()
        {
            if (GetShipOverallState(Player.Computer) != 0)
            {
                int number = randomizer.Next(6);
                while (shipsPlayer[number].GetLife == 0)
                {
                    number = randomizer.Next(6);
                }
                return shipsPlayer[number].Coordenada;
            }
            Coordenada2D coord = new Coordenada2D(-1, 0);
            return coord;
        }

        public int GetShipState(string AccesibleName, int player)
        {
            if (player == 0)
            {
                foreach (var item in ShipsPlayer)
                {
                    if (item.AccesibleName == AccesibleName)
                    {
                        return item.GetLife;
                    }
                }
            }
            else
            {
                foreach (var item in ShipsComputer)
                {
                    if (item.AccesibleName == AccesibleName)
                    {
                        return item.GetLife;
                    }
                }
            }
            return -1;
        }

        public int GetShipOverallState(Player player)
        {
            int total = 0;
            if (player == Player.Person)
            {
                int count = this.ShipsPlayer.Length;
                foreach (var item in ShipsPlayer)
                {
                    total += item.GetLife;
                }
                return total / count;
            }
            else
            {
                int count = this.ShipsComputer.Length;
                foreach (var item in ShipsComputer)
                {
                    total += item.GetLife;
                }
                return total / count;
            }
        }

        public void CreateShips()
        {
            shipsComputer[0] = new Portavion(Player.Computer);
            shipsComputer[1] = new Crucero(Player.Computer);
            shipsComputer[2] = new Fragata(Player.Computer);
            shipsComputer[3] = new Destroyer(Player.Computer);
            shipsComputer[4] = new Submarine(Player.Computer);
            shipsComputer[5] = new Submarine(Player.Computer);

            shipsPlayer[0] = new Portavion(Player.Person);
            shipsPlayer[1] = new Crucero(Player.Person);
            shipsPlayer[2] = new Fragata(Player.Person);
            shipsPlayer[3] = new Destroyer(Player.Person);
            shipsPlayer[4] = new Submarine(Player.Person);
            shipsPlayer[5] = new Submarine(Player.Person);
        }

        static public int Alternator()
        {
            return (int)Math.Pow((-1), randomizer.Next());
        }

        public int GetCoordenada(int x, int y, int player)
        {
            return board[player].BoardArray[x + 1, y + 1];
        }

        public void SetCoordenada(int x, int y, int value, int player)
        {
            board[player].BoardArray[x + 1, y + 1] = value;
        }

        public void ShowComputerShips(bool show)
        {
            for (int i = 0; i < ShipsComputer.Length; i++)
            {
                ShipsComputer[i].Visible = show;
            }
        }

        bool Free(int x, int y, int size, int pos, int player)
        {
            return board[player].Free(x, y, size, pos);
        }

        void RandomShip(int size, int type, int board, int ship)
        {
            /* Size: Tamaño del barco
               board: Tablero en el cual se va a posicionar
               Ship: Lugar del barco en el arreglo de posiciones de barcos
               Type: Tipo de barco
            */
            int pos = randomizer.Next(2);
            int x = 0, y = 0;
            bool answer = false; ;

            if (pos == 1)//Si esta acostado el barco
            {
                while (answer == false)
                {
                    x = randomizer.Next(10) - size;
                    y = randomizer.Next(15);
                    while (x < 1)
                    {
                        x = randomizer.Next(10) - size;
                    }
                    answer = Free(x, y, size, 1, board);
                }
                for (int i = 0; i < size; i++)
                {
                    this.SetCoordenada(x + i, y, type, board);
                }
                if (board == 0)
                {
                    shipsPlayer[ship].Posicion = Position.Horizontal;
                    shipsPlayer[ship].PosXY(x, y);

                }
                else
                {
                    shipsComputer[ship].Posicion = Position.Horizontal;
                    shipsComputer[ship].PosXY(x, y);
                }
            }
            else
            {
                while (answer == false) // si esta parado el barco
                {
                    x = randomizer.Next(10);
                    y = randomizer.Next(15) - size;
                    while (y < 1)
                    {
                        y = randomizer.Next(15) - size;
                    }
                    answer = Free(x, y, size, 0, board);
                }
                for (int i = 0; i < size; i++)
                {
                    this.SetCoordenada(x, y + i, type, board);
                }
                if (board == 0)
                {
                    shipsPlayer[ship].Posicion = Position.Vertical;
                    shipsPlayer[ship].PosXY(x, y);
                }
                else
                {
                    shipsComputer[ship].Posicion = Position.Vertical;
                    shipsComputer[ship].PosXY(x, y);
                }
            }
        }

        public void RandomAllocation(int player)
        {
            board[player] = new Board();
            RandomShip(5, 6, player, 0);
            RandomShip(4, 5, player, 1);
            RandomShip(3, 4, player, 2);
            RandomShip(3, 3, player, 3);
            RandomShip(2, 2, player, 4);
            RandomShip(2, 1, player, 5);
        }

        public static int Convertir(int value)
        {
            value -= 6;                 //este algoritmo transforma el valor de un barco en el
            value = Math.Abs(value);    //tablero en su posicion dentro del arreglo de barcos
            return value;
        }

        void SetAroundHits(Coordenada2D location, int size, Position pos)
        {
            List<Coordenada2D> around = null;
            around = board[1].Around(location.X + 1, location.Y + 1, size, pos);
            SetCoordenada(location.X, location.Y, 11, 1);
            foreach (var item in around)
            {
                escena.AddModel(new ExplosionSquare(item.X - 1, item.Y - 1, 2, Player.Computer));
            }   
        }

        public void SetHit(Player player, int shipPos)
        {
            shipPos = Game.Convertir(shipPos);
            switch (player)
            {
                case Player.Person:
                    {
                        ShipsPlayer[shipPos].SetHit();
                        break;
                    }
                case Player.Computer:
                    {
                        ShipsComputer[shipPos].SetHit();
                        if (shipsComputer[shipPos].GetLife == 0)// if the boat was sinked
                        {
                            Ship ship = shipsComputer[shipPos];
                            SetAroundHits(ship.Coordenada, ship.Size, ship.Posicion);
                        }
                        break;
                    }
            }
        }

        public int RemainingShips(Player player)
        {
            //Devuelve los barcos restantes de cada jugador
            int count = 0;
            if (player == Player.Person)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (ShipsPlayer[i].Sinked == false && ShipsPlayer[i].Sinking == false)
                    {
                        count++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    if (ShipsComputer[i].Sinked == false && ShipsComputer[i].Sinking == false)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        void ResetShips()
        {
            foreach (var item in shipsComputer)
            {
                item.ResetShip();  
            }
            foreach (var item in shipsPlayer)
            {
                item.ResetShip();  
            }
        }

        public void Reset()
        {
            finished = false; 
            this.currentPlayer = Player.Person;  
            this.Winner = Player.None;
            this.RandomAllocation(0);
            this.ResetShips();
            this.ComputerAi.Reset();  
        }

        public string Accuracy(Player player)
        {
            if (player == Player.Person)
            {
                return Convert.ToString((int)board[1].Accuracy()) + " %";
            }
            else
            {
                return Convert.ToString((int)board[0].Accuracy()) + " %";
            }
        }

        public float AccuracyF(Player player)
        {
            if (player == Player.Person)
            {
                return board[1].Accuracy();
            }
            else
            {
                return board[0].Accuracy();
            }
        }

        public string Rank(Player player)
        {
            float accuracy = AccuracyF(player); 
            if (accuracy == 100)
            {
                return GameStrings.GetString("SupCom");
            }
            if (accuracy >= 90)
            {
                return GameStrings.GetString("Comander");
            }
            if (accuracy < 90 && accuracy >= 70)
            {
                return GameStrings.GetString("FLieutenant");
            }
            if (accuracy < 70 && accuracy >= 50)
            {
                return GameStrings.GetString("Lieutenant");
            }
            if (accuracy < 50 && accuracy >= 30)
            {
                return GameStrings.GetString("Sergeant");
            }
            return GameStrings.GetString("Soldier");
        }

        public int Score(Player player)
        {
            float accuracy = AccuracyF(player);
            float fleet = GetShipOverallState(player);  
            return (int)(accuracy * (fleet + 5) * 50);
        }
    }
}
