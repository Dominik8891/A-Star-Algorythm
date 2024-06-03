using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace A_Star_Algorythm
{
    internal class Program
    {
        static int row    = 50;
        static int column = row * 2;

        static int start_x   = 1;
        static int start_y   = row - 2;
        static int finish_x  = column - 2;
        static int finish_y  = 1;
        static int[] start   = { start_x   , start_y };
        static int[] finish  = { finish_x  , finish_y };

        static int count = 0;

        static char[,] coordinate_field = new char[column, row];

        static string output = "";
        static void Main(string[] args)
        {
            int sleep = 1;

            // erstelung der Listen zum speichern der felder
            List<Feld> known      = new List<Feld>();
            List<Feld> goal       = new List<Feld>();
            List<Feld> completed  = new List<Feld>();
            List<Feld> neighbors  = new List<Feld>();
            List<Feld> pathToGoal = new List<Feld>();

            // das Spielfeld zum Start nur einmal komplett erzeugen!!

            erzeugeStartfeld();

            /////////////////////////////////////////////////////////

            setzeStartUndZiel();
            genBlockade();

            //////////////////////////////////////////////////////////


            Console.CursorVisible = false;

            // erstellung des startfeldes sowie speicherung in liste known
            Feld start_field = new Feld();
            start_field.setPos(start[0], start[1]);
            start_field.setCostToDestination(finish);
            start_field.setCostUntilNow(0);
            start_field.setTotalCost();
            known.Add(start_field);

            bool running = true;
            while (running)
            {
                zeigeSpielfeld();

                // zählt die liste auf inhalte wenn mehr als einer vorhanden wird die liste entfernung zum zielpunkt sortiert
                if(known.Count() >= 2)
                {
                    known.Sort(delegate (Feld first_field, Feld second_field)
                    {
                        return first_field.getTotalCost().CompareTo(second_field.getTotalCost());
                    });
                }
                // entnimmt den ersten punkt aus der liste und erstellt ein objekt und entfernt es anschließend aus der liste
                Feld tmp_field = known[0];
                known.RemoveAt(0);
                

                // befüllen der liste neighbors mit den nachbarn des aktuell untersuchten feldes
                neighbors = checkNeighbors(tmp_field);
                List<int> toRemove = new List<int>();
                // überprüfen ob die nachbarn bereits in einer der beiden listen known oder completed vorhanden sind
                bool is_existing = false;
                // dieser teil zählt die anzahl der nachbarn des feldes
                int count = neighbors.Count(); 
                for (int i = 0; i < count; i++)
                {
                    // prüft ob sich ein nachbar bereits in der completed liste befindet
                    foreach (Feld completed_field in completed)
                    {
                        // wenn ja werden sie weiter untern entfernt
                        if (neighbors[i].getPos_x() == completed_field.getPos_x() && neighbors[i].getPos_y() == completed_field.getPos_y())
                        {
                            is_existing = true;
                            break;
                        }
                    }
                    if (is_existing)
                    {
                        is_existing = false;
                        neighbors.RemoveAt(i);
                        i--;
                        count--;
                    }
                }
                is_existing = false;
                count = 0;
                // hier wird geprüft ob sich die nachbarn bereits in der bekannten liste befinden
                foreach (Feld neighbor in neighbors) 
                {
                    int counter = 0;
                    foreach (Feld known_field in known)
                    {
                        // wenn sie sich bereits in der bekannten liste befinden wird der costuntilnow wert überprüft 
                        if(neighbor.getPos_x() == known_field.getPos_x() && neighbor.getPos_y() == known_field.getPos_y())
                        {
                            is_existing = true;
                            // wenn der costuntilnow wert des feldes in der nachbar liste kleiner als in der known liste ist wird 
                            // der wert in der known liste überschrieben
                            if(neighbor.getCostUntilNow() < known_field.getCostUntilNow())
                            {
                                known[counter].setCostUntilNow(neighbor); 
                            }
                            break;
                        }
                        counter++;
                        is_existing = false;
                    }
                    // und wenn er noch nicht existiert wird er in die bekannten liste aufgenommen
                    if (is_existing == false)
                    {
                        known.Add((Feld)neighbor);
                        is_existing = false;
                    }
                }
                // das aktuelle feld wird zur completed liste hinzugefügt
                completed.Add(tmp_field);

                // wenn die liste pathToGoal gefüllt ist wird das programm beendet
                if (pathToGoal.Count() >= 1)
                {
                    running = false;
                }

                // zur ausgabe der completed liste in der komandozeile
                foreach (Feld completed_field in completed)
                {
                   
                    if(coordinate_field[completed_field.getPos_x(), completed_field.getPos_y()] != 'X')
                    {
                        coordinate_field[completed_field.getPos_x(), completed_field.getPos_y()] = '+';
                    } 
                }

                // zur ausgabe der bekannten felder in der komandozeile
                foreach (Feld known_field in known)
                {
                    if (coordinate_field[known_field.getPos_x(), known_field.getPos_y()] != 'X')
                    {
                        coordinate_field[known_field.getPos_x(), known_field.getPos_y()] = 'o';
                    }
                }

                

                // wenn das ziel erreicht wurde wird die liste pathtogoal gefüllt
                if (tmp_field.getPos_x() == finish[0] && tmp_field.getPos_y() == finish[1])
                {
                    pathToGoal = showShortestPathField(tmp_field, completed);
                    foreach (Feld path_field in pathToGoal)
                    {
                        if (path_field.getPos_x() != null && path_field.getPos_y() != null)
                        {
                            coordinate_field[path_field.getPos_x(), path_field.getPos_y()] = 'X';
                        }

                    }
                }

                Console.Write(output);  // ausgabe in der Komandozeile
                Console.WriteLine();
                Console.WriteLine(tmp_field.getPos_x()); // ausgabe der x koordinate des aktuellen feldes
                Console.WriteLine(tmp_field.getPos_y()); // ausgabe der y koordinate des aktuellen feldes
                Console.WriteLine(tmp_field.getTotalCost()); // ausgabe der kompletten kosten des aktuellen feldes

                count++;

                Thread.Sleep(sleep);
            }
            Console.ReadKey();
        }
        
        // erzeugt ein startfeld und speichert es in einem array
        static void erzeugeStartfeld()
        {
            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < column; x++)
                {
                    // wenn wir in der ERSTEN oder in der LETZTEN Zeile sind >> alle Rauten machen
                    if (x == 0 || x == column - 1)
                    {
                        coordinate_field[x, y] = '#';
                    }
                    // wenn wir ZWISCHEN der ERSTEN und der LETZTEN Zeile sind >> erste und letzte Spalte eine Raute
                    else if (y == 0 || y == row -1)
                    {
                        coordinate_field[x, y] = '#';
                    }
                    // in ALLEN anderen fällen >> ein Leerzeichen
                    else
                    {
                        coordinate_field[x, y] = ' ';
                    }
                }
            }

        }

        // bringt den start und den zielpunkt aufs feld
        static void setzeStartUndZiel()
        {
            coordinate_field[start[0], start[1]] = 'X';
            coordinate_field[finish[0], finish[1]] = 'O';
        }

        // speichert die blockade im spielfeld array
        static void genBlockade()
        {
            for (int i = 1; i <= 6; i++)
            {
                int x = finish[0] - i;
                coordinate_field[x, 2] = '#';
            }
            coordinate_field[finish[0] - 1, 3] = '#';
            coordinate_field[finish[0] - 1, 4] = '#';


            for (int i = 1; i < 50; i++)
            {
                int x = 70 - i;
                coordinate_field[x, 15] = '#';
            }
            coordinate_field[50 - 1, 16] = '#';
            coordinate_field[50 - 1, 17] = '#';
        }

        // sucht die nachbarn des aktuellen feldes und gibt diese als liste zurück
        static List<Feld> checkNeighbors(Feld in_feld)
        {
            List<Feld> neighbors = new List<Feld>();
            for(int x = - 1; x <= 1; x++) 
            {  
                for(int y = - 1; y <= 1; y++)
                {
                    // hier wird das in_feld ausgeschlossen
                    if(x != 0 && y == 0 || x == 0 && y != 0 || x != 0 && y != 0)
                    {
                        // hier werden felder ausgeschlossen die auf dem spielfeld eine # sind
                        if (coordinate_field[in_feld.getPos_x() + x, in_feld.getPos_y() + y] != '#')
                        {
                            // zusammenrechnen der position des feldes worauf man sich befindet und der for schleifen werte -1 bis +1 auf der x und y achse zur ermittlung der nachbarn
                            int pos_x = in_feld.getPos_x() + x;
                            int pos_y = in_feld.getPos_y() + y;
                            // ausschließen des randes des arrays
                            if (pos_x > 0 && pos_y > 0 && pos_x < column - 1 && pos_y < row - 1)
                            {
                                // erstellen der neuen felder und speichern in der liste
                                Feld tmp_feld = new Feld();
                                tmp_feld.setPos(pos_x, pos_y);
                                tmp_feld.setCostUntilNow(in_feld);
                                tmp_feld.setCostToDestination(finish);
                                tmp_feld.setTotalCost();
                                neighbors.Add(tmp_feld);
                            }
                        }
                    }
                    
                }
            }
            return neighbors;
        }

        // zum erstellen einer liste mit den feldern vom ziel zum start mit dem geringsten totalcost wert
        static List<Feld> showShortestPathField(Feld in_field, List<Feld> in_list)
        {
            List<Feld> path_to_goal = new List<Feld>();
            Feld tmp_field = in_field;
            while (true) 
            { 
                
                if (tmp_field != null) 
                {
                    // fügt das aktuelle feld zur liste hinzu und ersetzt danach das feld mit dem vorgänger feld
                    // was im feld geschpeichert ist solange das feld nicht null ist und gibt es dann zurück
                    path_to_goal.Add(tmp_field);
                    tmp_field = tmp_field.getPredecessor(); 
                }
                else
                {
                    break;
                }
            }
            return path_to_goal;
        }

        static void zeigeSpielfeld()
        {
            Console.SetCursorPosition(0, 0);

            output = "";

            // Ausgabe des Spielfelds was im Array gespeichert ist
            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < column; x++)
                {
                    output = output + coordinate_field[x, y];
                }
                output += "\n";
            }
        }
        

    }
    
}
