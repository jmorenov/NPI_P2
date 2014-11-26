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
    class Movement10
    {
        Skeleton skeleton;
        double angulo;

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Brush used for drawing joints that are currently inferred
        /// </summary>        
        private readonly Brush inferredJointBrush = Brushes.Yellow;

        /// <summary>
        /// Pen used for drawing bones that are currently inferred
        /// </summary>        
        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        /// <summary>
        /// Pen used for drawing bones that are currently tracked
        /// </summary>
        private readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);

        public bool isFinished()
        {
            return false;
        }

        public Movement10() { }

        public Movement10(Skeleton s) { setSkeleton(s); }

        public void setSkeleton(Skeleton s)
        {
            skeleton = s;
        }

        public void setAngle(double a)
        {
            angulo = a;
        }

        public Brush getBrush(Joint joint)
        {
            Brush drawBrush = null;

            if (joint.TrackingState == JointTrackingState.Tracked)
            {
                if (joint.JointType != JointType.KneeLeft && joint.JointType != JointType.AnkleLeft && joint.JointType != JointType.FootLeft)
                {
                    //se dibujan los puntos del esqueleto que no son de la pierna izquierda con el color por defecto(verde)
                    drawBrush = this.trackedJointBrush;
                }
                else
                {
                    //la variable dist_CadRod contiene la distancia entre el punto del esqueleto HipLeft y KneeLeft que se usara para calcular el áungulo que esta lavantada la rodilla
                    double dist_CadRod = System.Math.Sqrt(System.Math.Pow(skeleton.Joints[JointType.KneeLeft].Position.Y - skeleton.Joints[JointType.HipLeft].Position.Y, 2) +
                            System.Math.Pow(skeleton.Joints[JointType.HipLeft].Position.Z - skeleton.Joints[JointType.KneeLeft].Position.Z, 2));
                    //la variable angulo_pos contiene el ángulo que esta levantada la rodilla en la posición que se encuentra el esqueleto.
                    double angulo_pos = -(System.Math.Asin((skeleton.Joints[JointType.KneeLeft].Position.Z - skeleton.Joints[JointType.HipLeft].Position.Z) / dist_CadRod) * (180 / System.Math.PI));

                    // el color con el que se dibuja las articulaciones de la pierna dependera del angulo que este levantada la rodilla.
                    if (angulo_pos >= angulo - 5)
                    {
                        if (angulo_pos >= angulo + 10)
                        {
                            //se ha incluido el caso en el que se levante la rodilla mas de lo indicado, pero puede que no fuera necesario ya que las personas suelen tener un límite
                            drawBrush = Brushes.Turquoise;
                        }
                        else
                        {
                            drawBrush = this.trackedJointBrush;
                        }
                    }
                    else if ((dist_CadRod - dist_CadRod / 8) >= (skeleton.Joints[JointType.HipLeft].Position.Y - skeleton.Joints[JointType.KneeLeft].Position.Y))
                    {
                        // En el caso de que se halla empezado a levantar la rodilla pero aun no halla llegado al ángulo indicado se dibujara amarillo.
                        drawBrush = Brushes.Yellow;
                    }
                    else
                    {
                        drawBrush = Brushes.Red;
                    }
                }
            }
            else if (joint.TrackingState == JointTrackingState.Inferred)
            {
                drawBrush = this.inferredJointBrush;
            }
            return drawBrush;
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
        public Pen getPen(JointType j)
        {
            // We assume all drawn bones are inferred unless BOTH joints are tracked
            Pen drawPen = this.trackedBonePen;
            if (j == JointType.KneeLeft || j == JointType.AnkleLeft || j == JointType.FootLeft)
            {
                //la variable dist_CadRod contiene la distancia entre el punto del esqueleto HipLeft y KneeLeft que se usara para calcular el áungulo que esta lavantada la rodilla
                double dist_CadRod = System.Math.Sqrt(System.Math.Pow(skeleton.Joints[JointType.KneeLeft].Position.Y - skeleton.Joints[JointType.HipLeft].Position.Y, 2) +
                    System.Math.Pow(skeleton.Joints[JointType.HipLeft].Position.Z - skeleton.Joints[JointType.KneeLeft].Position.Z, 2));

                //la variable angulo_pos contiene el ángulo que esta levantada la rodilla en la posición que se encuentra el esqueleto.
                double angulo_pos = -(System.Math.Asin((skeleton.Joints[JointType.KneeLeft].Position.Z - skeleton.Joints[JointType.HipLeft].Position.Z) / dist_CadRod) * (180 / System.Math.PI));

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
            return drawPen;
        }
    }
}
