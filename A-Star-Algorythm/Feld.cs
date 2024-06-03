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
        private int    vorgänger_pos_x;
        private int    vorgänger_pos_y;
        private double cost_until_now;
        private double cost_to_destination;
        private double total_cost;

        public void setTotalCost()
        {
            //this.f = this.g + this.h;
            this.total_cost = this.cost_until_now + this.cost_to_destination;
        }

        public void setPos(int in_pos_x, int in_pos_y)
        {
            this.pos_x =  in_pos_x;
            this.pos_y =  in_pos_y;
        }

        public void setCostToDestination(int[] in_goal) 
        {
            double average_goal_x = getDistance(in_goal[0], this.pos_x);
            double average_goal_y = getDistance(in_goal[1], this.pos_y);
            double average_goal   = Math.Sqrt(average_goal_x * average_goal_x + average_goal_y * average_goal_y);
            //this.h = average_goal;
            this.cost_to_destination = average_goal;
        }
        public void setCostUntilNow(double in_g) 
        {
            this.predecessor     = null;
            this.vorgänger_pos_x = 0;
            this.vorgänger_pos_y = 0;
            //this.g = in_g;
            this.cost_until_now = in_g;
        }
        public void setCostUntilNow(Feld in_field_before) 
        {
            double field_before_x = getDistance(in_field_before.getPos_x(), this.pos_x);
            double field_before_y = getDistance(in_field_before.getPos_y(), this.pos_y);
            this.vorgänger_pos_x  = in_field_before.getPos_x();
            this.vorgänger_pos_y  = in_field_before.getPos_y();
            this.predecessor      = in_field_before; 

            //this.g = in_field_before.getG() + Math.Sqrt(field_before_x * field_before_x + field_before_y * field_before_y);
            this.cost_until_now = in_field_before.getCostUntilNow() + Math.Sqrt(field_before_x * field_before_x + field_before_y * field_before_y);
            /*double field_before_x = Math.Abs(in_field_before.getPos_x() - this.pos_x);
            double field_before_y = Math.Abs(in_field_before.getPos_y() - this.pos_y);
            double average_field        = (this.pos_x               + this.pos_y)         * 0.5;
            double average_field_before = (field_before_x + field_before_y) * 0.5;
            this.vorgänger_pos_x = in_field_before.pos_x;
            this.vorgänger_pos_y = in_field_before.pos_y;
            this.g = Math.Abs(average_field_before);*/
            
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
            //return this.h;
            return this.cost_to_destination;
        }

        public double getCostUntilNow() 
        {
            //return this.g;
            return this.cost_until_now;
        }

        public double getTotalCost()
        {
            //return this.g + this.h;
            return this.total_cost;
        }

        public int getVorgängerPosX()
        {
            return this.vorgänger_pos_x;
        }
        
        public int getVorgängerPosy()
        {
            return this.vorgänger_pos_y;
        }

        public Feld getPredecessor()
        {
            return this.predecessor; 
        }

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
