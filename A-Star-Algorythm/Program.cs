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
        static int zeilen = 50;
        static int spalten = zeilen * 2;

        static int start_x = 1;
        static int start_y = zeilen - 2;
        static int ziel_x = spalten - 2;
        static int ziel_y = 1;
        static int[] start = { start_x, start_y };
        static int[] ziel = { ziel_x, ziel_y };

        static int count = 0;

        static char[,] spielfeld = new char[spalten, zeilen];

        static string output = "";
        static void Main(string[] args)
        {
            // erstelung der Listen zum speichern der felder
            List<Feld> known      = new List<Feld>();
            List<Feld> goal       = new List<Feld>();
            List<Feld> completed  = new List<Feld>();
            List<Feld> neighbors  = new List<Feld>();
            List<Feld> pathToGoal = new List<Feld>();

            // das Spielfeld zum Start nur einmal komplett erzeugen!!

            erzeugeStartfeld();

            /////////////////////////////////////////////////////////

            // zufällig Sterne auf Spielfeld verteilen


            setzeStartUndZiel();
            genBlockade();

            //////////////////////////////////////////////////////////


            Console.CursorVisible = false;

            // erstellung des startfeldes sowie speicherung in liste known
            Feld start_feld = new Feld();
            start_feld.setPos(start[0], start[1]);
            start_feld.setCostToDestination(ziel);
            start_feld.setCostUntilNow(0);
            start_feld.setTotalCost();
            known.Add(start_feld);

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
                Feld tmp_feld = known[0];
                known.RemoveAt(0);
                

                // befüllen der liste neighbors mit den nachbarn des aktuell untersuchten feldes
                neighbors = checkNeighbors(tmp_feld);
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
                        // wenn sie sich bereits in der bekannten liste befinden wird der g (kosten von start zum aktuellen knoten) wert überprüft 
                        if(neighbor.getPos_x() == known_field.getPos_x() && neighbor.getPos_y() == known_field.getPos_y())
                        {
                            is_existing = true;
                            if(neighbor.getCostUntilNow() < known_field.getCostUntilNow())
                            {
                                known[counter].setCostUntilNow(neighbor.getCostUntilNow()); 
                                
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

                completed.Add(tmp_feld);


                foreach(Feld feld in completed)
                {
                   
                    if(spielfeld[feld.getPos_x(), feld.getPos_y()] != 'X')
                    {
                        spielfeld[feld.getPos_x(), feld.getPos_y()] = '+';
                    } 
                }

                foreach (Feld feld in known)
                {
                    if (spielfeld[feld.getPos_x(), feld.getPos_y()] != 'X')
                    {
                        spielfeld[feld.getPos_x(), feld.getPos_y()] = 'o';
                    }
                }
                if (pathToGoal.Count() >= 1)
                {
                    running = false;
                }

                if (tmp_feld.getPos_x() == ziel[0] && tmp_feld.getPos_y() == ziel[1])
                {
                    pathToGoal = showShortestPathField(tmp_feld, completed);
                    foreach (Feld path_field in pathToGoal)
                    {
                        if (path_field.getPos_x() != null && path_field.getPos_y() != null)
                        {
                            spielfeld[path_field.getPos_x(), path_field.getPos_y()] = 'X';
                        }

                    }
                    if (spielfeld[pathToGoal.Last().getPos_x(), pathToGoal.Last().getPos_y()] == 'X')
                    {
                        //running = false;
                    }
                }

                

                Console.Write(output);
                Console.WriteLine();
                Console.WriteLine(tmp_feld.getPos_x());
                Console.WriteLine(tmp_feld.getPos_y());
                Console.WriteLine(tmp_feld.getTotalCost());

                

                count++;

                Thread.Sleep(1);
            }
            Console.ReadKey();
        }
        
        static void erzeugeStartfeld()
        {
            for (int y = 0; y < zeilen; y++)
            {
                for (int x = 0; x < spalten; x++)
                {
                    // wenn wir in der ERSTEN oder in der LETZTEN Zeile sind >> alle Rauten machen
                    if (x == 0 || x == spalten - 1)
                    {
                        spielfeld[x, y] = '#';
                    }
                    // wenn wir ZWISCHEN der ERSTEN und der LETZTEN Zeile sind >> erste und letzte Spalte eine Raute
                    else if (y == 0 || y == zeilen -1)
                    {
                        spielfeld[x, y] = '#';
                    }
                    // in ALLEN anderen fällen >> ein Leerzeichen
                    else
                    {
                        spielfeld[x, y] = ' ';
                    }
                }
            }

        }

        static void setzeStartUndZiel()
        {
            spielfeld[start[0], start[1]] = 'X';
            spielfeld[ziel[0], ziel[1]] = 'O';
        }

        static void genBlockade()
        {
            for (int i = 1; i <= 6; i++)
            {
                int x = ziel[0] - i;
                spielfeld[x, 2] = '#';
            }
            spielfeld[ziel_x - 1, 3] = '#';
            spielfeld[ziel_x - 1, 4] = '#';
        }

        static void search(List<Feld> in_list)
        {

        }

        static List<Feld> checkNeighbors(Feld in_feld)
        {
            List<Feld> neighbors = new List<Feld>();
            for(int x = - 1; x <= 1; x++) 
            {  
                for(int y = - 1; y <= 1; y++)
                {
                    if(x != 0 && y == 0 || x == 0 && y != 0 || x != 0 && y != 0)
                    {
                        if (spielfeld[in_feld.getPos_x() + x, in_feld.getPos_y() + y] != '#')
                        {
                            int pos_x = in_feld.getPos_x() + x;
                            int pos_y = in_feld.getPos_y() + y;
                            if (pos_x > 0 && pos_y > 0 && pos_x < spalten - 1 && pos_y < zeilen - 1)
                            {
                                Feld tmp_feld = new Feld();
                                tmp_feld.setPos(pos_x, pos_y);
                                tmp_feld.setCostUntilNow(in_feld);
                                tmp_feld.setCostToDestination(ziel);
                                tmp_feld.setTotalCost();
                                neighbors.Add(tmp_feld);
                            }
                        }
                    }
                    
                }
            }
            return neighbors;
        }

        static List<Feld> showShortestPathField(Feld in_field, List<Feld> in_list)
        {
            List<Feld> path_to_goal = new List<Feld>();
            Feld tmp_field = in_field;
            while (true) 
            {
                
                if (tmp_field != null) 
                {
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
            for (int y = 0; y < zeilen; y++)
            {
                for (int x = 0; x < spalten; x++)
                {
                    output = output + spielfeld[x, y];
                }
                output += "\n";
            }
        }
        

    }
    
}
