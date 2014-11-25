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
        Movimiento7 mov7 = new Movimiento7();

        public MovementController() { }

        public void setSkeleton(Skeleton s)
        {
            mov7.setSkeleton(s);
        }

        public Brush getBrush(Joint joint)
        {
            return mov7.getBrush(joint);
        }

        public Pen getPen(JointType j)
        {
            return mov7.getPen(j);
        }
    }
}
