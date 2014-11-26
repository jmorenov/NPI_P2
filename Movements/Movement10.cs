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
    class Movement10 : Movement
    {
        private Pen drawPen;
        Brush drawBrush;

        public override void setSkeleton(Skeleton s)
        {
            base.setSkeleton(s);
            checkPosition();
        }

        public override Brush getBrush(Joint joint)
        {
            if (joint.JointType != JointType.KneeLeft && joint.JointType != JointType.AnkleLeft && joint.JointType != JointType.FootLeft)
            {
                return base.getBrush(joint);
            }
            else
            {
                return drawBrush;
            }
        }

        /// <summary>
        /// Dibuja un hueso entre dos puntos del esqueleto. 
        /// Dependiendo de si la rodilla está levantada el angulo indicado lo dibujará de un color u otro.
        /// </summary>
        /// <param name="skeleton">skeleton to draw bones from</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// <param name="jointType0">joint to start drawing from</param>
        /// <param name="jointType1">joint to end drawing at</param>
        /// <param name="angulo">ángulo en grados que tiene que estar levantada la rodilla izquierda para que se dibuje verde</param>
        public override Pen getPen(JointType j)
        {
            // We assume all drawn bones are inferred unless BOTH joints are tracked
            if (j == JointType.KneeLeft || j == JointType.AnkleLeft || j == JointType.FootLeft)
                return drawPen;
            return base.getPen(j);
        }

        public void checkPosition()
        {
            //la variable dist_CadRod contiene la distancia entre el punto del esqueleto HipLeft y KneeLeft que se usara para calcular el áungulo que esta lavantada la rodilla
            double dist_CadRod = System.Math.Sqrt(System.Math.Pow(skeleton.Joints[JointType.KneeLeft].Position.Y - skeleton.Joints[JointType.HipLeft].Position.Y, 2) +
                System.Math.Pow(skeleton.Joints[JointType.HipLeft].Position.Z - skeleton.Joints[JointType.KneeLeft].Position.Z, 2));

            //la variable angulo_pos contiene el ángulo que esta levantada la rodilla en la posición que se encuentra el esqueleto.
            double angulo_pos = -(System.Math.Asin((skeleton.Joints[JointType.KneeLeft].Position.Z - skeleton.Joints[JointType.HipLeft].Position.Z) / dist_CadRod) * (180 / System.Math.PI));

            finished = false;
            // el color con el que se dibuja los huesos dependera del ángulo que este levantada la rodilla.
            if (angulo_pos >= angle - ERROR_PERCENT)
            {
                if (angulo_pos >= angle + ERROR_PERCENT * 2)
                {
                    drawPen = new Pen(Brushes.Turquoise, 6);
                    drawBrush = Brushes.Turquoise;
                }
                else
                {
                    drawPen = new Pen(Brushes.Green, 6);
                    drawBrush = trackedJointBrush;
                    finished = true;
                }
            }
            else if ((dist_CadRod - dist_CadRod / 8) >= (skeleton.Joints[JointType.HipLeft].Position.Y - skeleton.Joints[JointType.KneeLeft].Position.Y))
            {
                drawPen = new Pen(Brushes.Yellow, 6);
                drawBrush = Brushes.Yellow;
            }
            else
            {
                drawPen = new Pen(Brushes.Red, 6);
                drawBrush = Brushes.Red;
            }
        }
    }
}
