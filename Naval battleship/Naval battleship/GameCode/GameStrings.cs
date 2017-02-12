using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xengine;

namespace NavalBattleship.GameCode
{

    public class GameStrings
    {
        Language language;
        Dictionary<string, string> stringSpanish = new Dictionary<string, string>();
        Dictionary<string, string> stringEnglish = new Dictionary<string, string>();

        public Language Language
        {
            get { return language; }
            set { language = value; }
        }

        public string GetString(string key)
        {
            if (language == Language.English)
            {
                return stringEnglish[key];
            }
            else
            {
                return stringSpanish[key]; 
            }
        }

        /// <summary>
        /// This function builds the strings according to the language chosen by the user
        /// </summary>
        public void CreateStrings()
        {
            //create english strings
            stringEnglish.Add("ExitGame", "Do you wish to leave?");
            stringEnglish.Add("Yes", "Yes");
            stringEnglish.Add("No", "No");
            stringEnglish.Add("NewGame", "New Game");  
            stringEnglish.Add("Rank", "Rank");
            stringEnglish.Add("SupCom", "Supreme Comander");
            stringEnglish.Add("Comander", "Comander");
            stringEnglish.Add("FLieutenant", "First Lieutenant");
            stringEnglish.Add("Lieutenant", "Lieutenant");
            stringEnglish.Add("Sergeant", "Sergeant");
            stringEnglish.Add("Soldier", "Soldier");
            stringEnglish.Add("Fleet", "Fleet");
            stringEnglish.Add("Aiming", "Aiming");
            stringEnglish.Add("Score", "Score");  
            stringEnglish.Add("Title", "TittleEnglish"); 
            stringEnglish.Add("WinMessage", "You have won");
            stringEnglish.Add("LooseMessage", "You have lost");
            stringEnglish.Add("Victory", "Victory");
            stringEnglish.Add("Defeat", "Defeat");
            stringEnglish.Add("CPU", "Computer");
            stringEnglish.Add("Acept", "Acept");
            stringEnglish.Add("InsertName", "Insert your Name:");
            stringEnglish.Add("CPUTurn", "CPU's chance");
            stringEnglish.Add("PlayerTurn", "Player's chance");
            stringEnglish.Add("Menu", "Menu");
            stringEnglish.Add("Enter", "Enter");
            stringEnglish.Add("Start", "Start");
            stringEnglish.Add("Exit", "Quit");
            stringEnglish.Add("Credits", "Credits");
            stringEnglish.Add("Resume", "Resume");
            stringEnglish.Add("Main", "Main");
            stringEnglish.Add("DefaultName", "Anonymous");
            stringEnglish.Add("Setup", "Setup");
            stringEnglish.Add("None", "None");
            stringEnglish.Add("PlayerName", "Player Name: ");
            stringEnglish.Add("SelectedShip", "Selected Ship: ");
            stringEnglish.Add("EnemyFleet", "Enemy Fleet");
            stringEnglish.Add("Carrier", "Carrier");
            stringEnglish.Add("Cruiser", "Cruiser");
            stringEnglish.Add("Frigate", "Frigate");
            stringEnglish.Add("Destroyer", "Destroyer");
            stringEnglish.Add("Submarine", "Submarine");
            stringEnglish.Add("BattleShip", "Naval BattleShip 3D v 1.0 2009");
            stringEnglish.Add("AirCamera", "Air Camera");
            stringEnglish.Add("FreeCamera", "Free Camera");
            stringEnglish.Add("SelectLanguage", "Select your language:");
            stringEnglish.Add("ChangesMade", "Los cambios se veran reflejados la próxima vez que inicie el juego");
            stringEnglish.Add("MisileOn", "Misile Camera ON");
            stringEnglish.Add("MisileOff", "Misile Camera OFF");
            stringEnglish.Add("SoundsOn", "Sounds ON");
            stringEnglish.Add("SoundsOff", "Sounds OFF");
            stringEnglish.Add("English", "English");
            stringEnglish.Add("Spanish", "Español");
            stringEnglish.Add("Tip", "Tip: You can rearrange the ships by pressing the Setup button");
            
            //create spanish strings
            stringSpanish.Add("ExitGame", "¿Desea salir del juego?");
            stringSpanish.Add("Yes", "Si");
            stringSpanish.Add("No", "No");
            stringSpanish.Add("NewGame", "Nuevo juego"); 
            stringSpanish.Add("Rank", "Rango");
            stringSpanish.Add("SupCom", "Comandante supremo");
            stringSpanish.Add("Comander", "Comandante");
            stringSpanish.Add("FLieutenant", "Primer teniente");
            stringSpanish.Add("Lieutenant", "Teniente");
            stringSpanish.Add("Sergeant", "Sargento");
            stringSpanish.Add("Soldier", "Soldado");
            stringSpanish.Add("Fleet", "Flota");
            stringSpanish.Add("Aiming", "Punteria");
            stringSpanish.Add("Score", "Puntuación");  
            stringSpanish.Add("Title", "TittleSpanish");
            stringSpanish.Add("WinMessage", "Has Ganado");
            stringSpanish.Add("LooseMessage", "Has perdido");
            stringSpanish.Add("Victory", "Victoria");
            stringSpanish.Add("Defeat", "Derrota");
            stringSpanish.Add("CPU", "Computadora");
            stringSpanish.Add("Acept", "Acepto");
            stringSpanish.Add("InsertName", "Inserte su nombre:");
            stringSpanish.Add("CPUTurn", "Turno de la computadora");
            stringSpanish.Add("PlayerTurn", "Tu turno");
            stringSpanish.Add("Menu", "Menú");
            stringSpanish.Add("Enter", "Entrar");
            stringSpanish.Add("Start", "Comenzar");
            stringSpanish.Add("Exit", "Salir");
            stringSpanish.Add("Credits", "Créditos");
            stringSpanish.Add("Resume", "Resumir");
            stringSpanish.Add("Main", "Principal");
            stringSpanish.Add("DefaultName", "Anónimo");
            stringSpanish.Add("Setup", "Organizar");
            stringSpanish.Add("None", "Ninguno");
            stringSpanish.Add("PlayerName", "Nombre del jugador: ");
            stringSpanish.Add("SelectedShip", "Barco Seleccionado: ");
            stringSpanish.Add("EnemyFleet", "Flota Enemiga");
            stringSpanish.Add("Carrier", "Portavión");
            stringSpanish.Add("Cruiser", "Crucero");
            stringSpanish.Add("Frigate", "Fragata");
            stringSpanish.Add("Destroyer", "Destructor");
            stringSpanish.Add("Submarine", "Submarino");
            stringSpanish.Add("BattleShip", "Batalla Naval 3D v 1.0 2009");
            stringSpanish.Add("AirCamera", "Cámara Aérea");
            stringSpanish.Add("FreeCamera", "Cámara Libre");
            stringSpanish.Add("SelectLanguage", "Seleccione su lenguaje:");
            stringSpanish.Add("ChangesMade", "Changes will be reflected the next time you start the game");
            stringSpanish.Add("MisileOn", "Cámara del misil activada");
            stringSpanish.Add("MisileOff", "Cámara del misil desactivada");
            stringSpanish.Add("SoundsOn", "Sonidos activados");
            stringSpanish.Add("SoundsOff", "Sonidos desactivados");
            stringSpanish.Add("English", "English");
            stringSpanish.Add("Spanish", "Español");
            stringSpanish.Add("Tip", "Consejo: Puedes reorganizar los barcos apretando el boton Organizar");
        }

        /// <summary>
        /// This function reads the language set on the config file
        /// </summary>
        public void ReadLanguage()
        {
            try
            {
                string[] lines = File.ReadAllLines("config.ini");
                if (lines[0].IndexOf("English") != -1)
                {
                    Language = Language.English;
                }
                else
                {
                    Language = Language.Spanish;
                }
            }
            catch (Exception)
            {
            }
        }
        
        /// <summary>
        /// This function select a language a write it on the config file
        /// </summary>
        public void SelectLanguage(Language language)
        {
            string[] config = new string[1];
            config[0] = "Language = " + language;
            File.WriteAllLines("config.ini", config);
        }
    }
}
