using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Kinect;

namespace NPI_P2
{
    public class MovementController
    {
        private List<Movement> m = new List<Movement>{new Movement1(), new Movement5(), new Movement7(), new Movement9(), new Movement10()};

        private int mov_i = 0;
        private double ERROR;
        private double angle;
        private bool exercise_started = false;

        public void startExercise(double ERROR, double angle)
        {
            this.ERROR = ERROR;
            this.angle = angle;
            exercise_started = true;
        }

        public void refresh()
        {
            if (exercise_started)
            {
                bool finish = false;
                finish = m[mov_i].isFinished();
                if (finish)
                {
                    mov_i++;
                    if (mov_i > 4) mov_i = 0;
                }
            }
        }

        public void setSkeleton(Skeleton s)
        {
            m[mov_i].setSkeleton(s);
        }

        public Brush getBrush(Joint joint)
        {
            return m[mov_i].getBrush(joint);
        }

        public Pen getPen(JointType j0, JointType j1)
        {
            return m[mov_i].getPen(j1);
        }
    }
}