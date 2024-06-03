using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Star_Algorythm
{
    internal class Feld
    {
        private int    pos_x;
        private int    pos_y;
        private Feld   predecessor;
        private double cost_until_now;
        private double cost_to_destination;
        private double total_cost;

        // gesamte kosten vom des feldes vom start bis zum ziel
        public void setTotalCost()
        {
            this.total_cost = this.cost_until_now + this.cost_to_destination;
        }

        // position des feldes x und y achse 
        public void setPos(int in_pos_x, int in_pos_y)
        {
            this.pos_x =  in_pos_x;
            this.pos_y =  in_pos_y;
        }

        // hier werden die geschätzten kosten bis zum ziel errechnet
        public void setCostToDestination(int[] in_dest) 
        {
            double average_dest_x = getDistance(in_dest[0], this.pos_x);
            double average_dest_y = getDistance(in_dest[1], this.pos_y);
            double average_dest   = Math.Sqrt(average_dest_x * average_dest_x + average_dest_y * average_dest_y);

            this.cost_to_destination = average_dest;
        }

        // hier werden die kosten bis zum aktuellen feld gesetzt überladene funktion für das startfeld hier sind kosten 0
        // zusätzlich wird das vorgängerfeld gesetzt
        public void setCostUntilNow(double in_g) 
        {
            this.predecessor     = null;
            this.cost_until_now = in_g;
        }

        // auch hier werden kosten für das aktuelle feld gesetzt diese werden aus der position des vorgängerfeldes und des aktuellen feldes berechnet und mit den kosten des vorgängerfeldes addiert+
        // zusätzlich wird das vorgängerfeld gesetzt
        public void setCostUntilNow(Feld in_field_before) 
        {
            double field_before_x = getDistance(in_field_before.getPos_x(), this.pos_x);
            double field_before_y = getDistance(in_field_before.getPos_y(), this.pos_y);
            this.predecessor      = in_field_before; 

            this.cost_until_now = in_field_before.getCostUntilNow() + Math.Sqrt(field_before_x * field_before_x + field_before_y * field_before_y);
        }

        public int getPos_x() 
        { 
            return this.pos_x;
        }

        public int getPos_y() 
        {
            return this.pos_y;
        }

        public double getCostToDestination() 
        {
            return this.cost_to_destination;
        }

        public double getCostUntilNow() 
        {
            return this.cost_until_now;
        }

        public double getTotalCost()
        {
            return this.total_cost;
        }

        public Feld getPredecessor()
        {
            return this.predecessor; 
        }

        // eine methode zur berechnung der distanz zwischen zwei feldern
        public double getDistance(double in_pos_1, double in_pos_2)
        {
            if(in_pos_1 > in_pos_2) 
            {
                return in_pos_1 - in_pos_2;
            }
            else
            {
                return in_pos_2 - in_pos_1;
            }
        }
    }
}
