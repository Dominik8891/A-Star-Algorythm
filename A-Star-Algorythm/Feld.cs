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
        private int    vorgänger_pos_x;
        private int    vorgänger_pos_y;
        public double h;
        public double g;
        public double f;

        public void setF()
        {
            this.f = this.g + this.h;
        }

        public void setPos(int in_pos_x, int in_pos_y)
        {
            this.pos_x =  in_pos_x;
            this.pos_y =  in_pos_y;
        }

        public void setH(int[] in_goal) 
        {
            double average_goal_x = rechnen(in_goal[0], this.pos_x);
            double average_goal_y = rechnen(in_goal[1], this.pos_y);
            double average_goal   = Math.Sqrt(average_goal_x * average_goal_x + average_goal_y * average_goal_y);
            this.h = average_goal;
        }
        public void setG(double in_g) 
        {
            this.vorgänger_pos_x = 0;
            this.vorgänger_pos_y = 0;
            this.g = in_g;
        }
        public void setG(Feld in_field_before) 
        {
            double field_before_x = rechnen(in_field_before.getPos_x(), this.pos_x);
            double field_before_y = rechnen(in_field_before.getPos_y(), this.pos_y);

            this.g = in_field_before.getG() + Math.Sqrt(field_before_x * field_before_x + field_before_y * field_before_y);
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

        public double getH() 
        {
            return this.h;
        }

        public double getG() 
        {
            return this.g;
        }

        public double getF()
        {
            return this.g + this.h;
        }

        public int getVorgängerPosX()
        {
            return this.vorgänger_pos_x;
        }
        
        public int getVorgängerPosy()
        {
            return this.vorgänger_pos_y;
        }

        public double rechnen(double in_zahl_1, double in_zahl_2)
        {
            if(in_zahl_1 > in_zahl_2) 
            {
                return in_zahl_1 - in_zahl_2;
            }
            else
            {
                return in_zahl_2 - in_zahl_1;
            }
        }
    }
}
