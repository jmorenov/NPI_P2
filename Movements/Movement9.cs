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
    class Movement9
    {
        Skeleton skeleton;
        bool skeleton_position = false;

        /// <summary>
        /// Pen used for drawing bones that are currently tracked
        /// </summary>
        private readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);

        /// <sumary>
        /// Pen used for drawing bones that are currently tracked and that are in the correct position
        /// </sumary>
        //private readonly Brush posicion_correcta_joint = Brushes.Green;
        private readonly Pen posicion_correcta_hueso = new Pen(Brushes.Green, 6);

        private readonly Pen posicion_fallida_hueso = new Pen(Brushes.Red, 6);

        private Pen drawPen;

        public Movement9() { }

        public Movement9(Skeleton s) { setSkeleton(s); }

        public void setSkeleton(Skeleton s)
        {
            skeleton = s;
            //skeleton_position = CheckPosition();
            CheckPosition();
        }

        /// <summary>
        /// Devuelve el objeto Pen con el que pintar el hueso que se une con la parte del cuerpo JointType.
        /// </summary>
       /* public Pen getPen(JointType j)
        {
            if (j == JointType.KneeRight || j == JointType.AnkleRight)
            {
                if (skeleton_position)
                    return posicion_correcta_hueso;
                return posicion_fallida_hueso;
            }
            return trackedBonePen;
        }*/
        public Pen getPen(JointType j)
        {
            if (j == JointType.KneeRight || j == JointType.AnkleRight)
            {
                return drawPen;
            }
            return trackedBonePen;
        }

        /*private bool CheckPosition()
        {
            //Check knee position
            Joint rodillaDer = skeleton.Joints[JointType.KneeRight];
            Joint caderaDer = skeleton.Joints[JointType.HipRight];

            if ((caderaDer.Position.Y - caderaDer.Position.Y * 0.90) <= (rodillaDer.Position.Y + (rodillaDer.Position.Y * 0.5)) && (caderaDer.Position.Y - caderaDer.Position.Y * 0.5) >= (rodillaDer.Position.Y - (rodillaDer.Position.Y * 0.5)))
            {
                if (caderaDer.Position.X <= (rodillaDer.Position.X + (rodillaDer.Position.X * 0.5)) && caderaDer.Position.X >= (rodillaDer.Position.X - (rodillaDer.Position.X * 0.5)))
                    return true;
                else
                    return false;
            }
            else return false;
        }*/
        private void CheckPosition()
        {
            double angulo = 60;
                //la variable dist_CadRod contiene la distancia entre el punto del esqueleto HipLeft y KneeLeft que se usara para calcular el áungulo que esta lavantada la rodilla
                double dist_CadRod = System.Math.Sqrt(System.Math.Pow(skeleton.Joints[JointType.KneeRight].Position.Y - skeleton.Joints[JointType.HipRight].Position.Y, 2) +
                    System.Math.Pow(skeleton.Joints[JointType.HipRight].Position.Z - skeleton.Joints[JointType.KneeRight].Position.Z, 2));

                //la variable angulo_pos contiene el ángulo que esta levantada la rodilla en la posición que se encuentra el esqueleto.
                double angulo_pos = -(System.Math.Asin((skeleton.Joints[JointType.KneeRight].Position.Z - skeleton.Joints[JointType.HipRight].Position.Z) / dist_CadRod) * (180 / System.Math.PI));

                // el color con el que se dibuja los huesos dependera del ángulo que este levantada la rodilla.
                if (angulo_pos >= angulo - 5)
                {
                    if (angulo_pos >= angulo + 10)
                    {
                        drawPen = new Pen(Brushes.Turquoise, 6);
                    }
                    else
                    {
                        drawPen = new Pen(Brushes.Green, 6);
                    }
                }
                else if ((dist_CadRod - dist_CadRod / 8) >= (skeleton.Joints[JointType.HipLeft].Position.Y - skeleton.Joints[JointType.KneeLeft].Position.Y))
                {
                    drawPen = new Pen(Brushes.Yellow, 6);
                }
                else
                {
                    drawPen = new Pen(Brushes.Red, 6);
                }
            }
        }
    }
