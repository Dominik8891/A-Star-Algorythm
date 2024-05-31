using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace A_Star_Algorythm
{
    internal class Program
    {
        static int zeilen = 30;
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
            List<Feld> next_known = new List<Feld>();
            List<Feld> completed  = new List<Feld>();
            List<Feld> neighbors = new List<Feld>();

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
            start_feld.setH(ziel);
            start_feld.setG(0);
            start_feld.setF();
            known.Add(start_feld);

            while (true)
            {
                zeigeSpielfeld();

                // zählt die liste auf inhalte wenn mehr als einer vorhanden wird die liste entfernung zum zielpunkt sortiert
                if(known.Count() >= 2)
                {
                    known.Sort(delegate (Feld x, Feld y)
                    {
                        return x.getF().CompareTo(y.getF());
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
                    foreach (Feld y in completed)
                    {
                        // wenn ja werden sie weiter untern entfernt
                        if (neighbors[i].getPos_x() == y.getPos_x() && neighbors[i].getPos_y() == y.getPos_y())
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
                foreach (Feld x in neighbors) 
                {
                    int counter = 0;
                    foreach (Feld y in known)
                    {
                        // wenn sie sich bereits in der bekannten liste befinden wird der g (kosten von start zum aktuellen knoten) wert überprüft 
                        if(x.getPos_x() == y.getPos_x() && x.getPos_y() == y.getPos_y())
                        {
                            is_existing = true;
                            /*if(x.f < y.f)
                            {
                                known[counter].f = x.f; 
                                
                            }*/
                        }
                        counter++;
                    }
                    // und wenn er noch nicht existiert wird er in die bekannten liste aufgenommen
                    if (!is_existing)
                    {
                        known.Add((Feld)x);
                        is_existing = false;
                    }
                }

                if(tmp_feld.getPos_x() == ziel[0] && tmp_feld.getPos_y() == ziel[1])
                {
                    break;
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

                Console.Write(output);
                Console.WriteLine();
                Console.WriteLine(tmp_feld.getPos_x());
                Console.WriteLine(tmp_feld.getPos_y());
                Console.WriteLine(tmp_feld.getF());

                count++;

                Thread.Sleep(150);
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
                                tmp_feld.setG(in_feld);
                                tmp_feld.setH(ziel);
                                tmp_feld.setF();
                                neighbors.Add(tmp_feld);
                            }
                        }
                    }
                    
                }
            }
            return neighbors;
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
