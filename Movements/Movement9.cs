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
    /// <summary>
    /// Clase Movement9 basada en el movimiento 9 desarrollado por: https://github.com/ToskoKozlov/Kinect_Project/blob/master/MainWindow.xaml.cs
    /// 
    /// Movimiento: En pie con rodilla derecha levantada (plano XZ) y brazos en cruz. El ángulo de la pierna debe ser un parámetro de entrada.
    /// 
    /// </summary>
    class Movement9 : Movement
    {
        private Pen drawPen;

        public override void setSkeleton(Skeleton s)
        {
            base.setSkeleton(s);
            CheckPosition();
        }

        public override Pen getPen(JointType j)
        {
            if (j == JointType.KneeRight || j == JointType.AnkleRight)
            {
                return drawPen;
            }
            return trackedBonePen;
        }

        private void CheckPosition()
        {
            double angulo = angle;
                //la variable dist_CadRod contiene la distancia entre el punto del esqueleto HipLeft y KneeLeft que se usara para calcular el áungulo que esta lavantada la rodilla
                double dist_CadRod = System.Math.Sqrt(System.Math.Pow(skeleton.Joints[JointType.KneeRight].Position.Y - skeleton.Joints[JointType.HipRight].Position.Y, 2) +
                    System.Math.Pow(skeleton.Joints[JointType.HipRight].Position.Z - skeleton.Joints[JointType.KneeRight].Position.Z, 2));

                //la variable angulo_pos contiene el ángulo que esta levantada la rodilla en la posición que se encuentra el esqueleto.
                double angulo_pos = -(System.Math.Asin((skeleton.Joints[JointType.KneeRight].Position.Z - skeleton.Joints[JointType.HipRight].Position.Z) / dist_CadRod) * (180 / System.Math.PI));
                finished = false;
                // el color con el que se dibuja los huesos dependera del ángulo que este levantada la rodilla.
                if (angulo_pos >= angulo - ERROR_PERCENT)
                {
                    if (angulo_pos >= angulo + ERROR_PERCENT*2)
                    {
                        drawPen = new Pen(Brushes.Turquoise, 6);
                    }
                    else
                    {
                        drawPen = new Pen(Brushes.Green, 6);
                        finished = true;
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
