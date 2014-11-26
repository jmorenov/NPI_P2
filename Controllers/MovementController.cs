using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using System.Windows;
using System.Windows.Media;

namespace NPI_P2
{
    class MovementController
    {
        Movement1 mov1 = new Movement1();
        Movement5 mov5 = new Movement5();
        Movimiento7 mov7 = new Movimiento7();
        Movement9 mov9 = new Movement9();
        Movement10 mov10 = new Movement10();

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        public MovementController() { }

        public void setSkeleton(Skeleton s)
        {
            //mov7.setSkeleton(s);
            //mov5.setSkeleton(s);
            //mov9.setSkeleton(s);
            mov10.setSkeleton(s);
            mov10.setAngle(60);
        }

        public Brush getBrush(Joint joint)
        {
            return mov10.getBrush(joint);
            //return trackedJointBrush;
        }

        public Pen getPen(JointType j)
        {
            return mov10.getPen(j);
        }
    }
}
