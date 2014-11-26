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
        private Movement1 mov1 = new Movement1();
        private Movement5 mov5 = new Movement5();
        private Movement7 mov7 = new Movement7();
        private Movement9 mov9 = new Movement9();
        private Movement10 mov10 = new Movement10();

        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));
        private readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);

        private int mov = 0;
        private double difficulty;
        private double angulo;
        public MovementController() { }

        public void startExercise(double difficulty, double angulo)
        {
            this.difficulty = difficulty;
            this.angulo = angulo;
        }

        public void refresh()
        {
            bool finish = false;
            switch (mov)
            {
                case 0:
                    finish = mov1.isFinished();
                    break;
                case 1:
                    finish = mov5.isFinished();
                    break;
                case 2:
                    finish = mov7.isFinished();
                    break;
                case 3:
                    finish = mov9.isFinished();
                    break;
                case 4:
                    finish = mov10.isFinished();
                    break;
            }
            if (finish)
            {
                mov++;
                if (mov > 4) mov = 0;
            }
        }

        public void setSkeleton(Skeleton s)
        {
            switch (mov)
            {
                case 0:
                    mov1.setSkeleton(s);
                    break;
                case 1:
                    mov5.setSkeleton(s);
                    break;
                case 2:
                    mov7.setSkeleton(s);
                    break;
                case 3:
                    mov9.setSkeleton(s);
                    break;
                case 4:
                    mov10.setSkeleton(s);
                    break;
            }
        }

        public Brush getBrush(Joint joint)
        {
            Brush b = trackedJointBrush;
            switch (mov)
            {
                case 0:
                    b = mov1.getBrush(joint);
                    break;
                case 1:
                    b = mov5.getBrush(joint);
                    break;
                case 2:
                    b = mov7.getBrush(joint);
                    break;
                case 3:
                    b = mov9.getBrush(joint);
                    break;
                case 4:
                    b = mov10.getBrush(joint);
                    break;
            }
            return b;
        }

        public Pen getPen(JointType j0, JointType j1)
        {
            Pen b = trackedBonePen;
            switch (mov)
            {
                case 0:
                    b = mov1.getPen(j1);
                    break;
                case 1:
                    b = mov5.getPen(j1);
                    break;
                case 2:
                    b = mov7.getPen(j1);
                    break;
                case 3:
                    b = mov9.getPen(j1);
                    break;
                case 4:
                    b = mov10.getPen(j1);
                    break;
            }
            return b;
        }
    }
}