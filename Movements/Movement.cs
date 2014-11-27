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
    /// <summary>
    /// Clase abstract en la que se basan los diferentes Movimientos.
    /// </summary>
    public abstract class Movement
    {
        /// <summary>
        /// Objeto Skeleton donde se almacena la detección del cuerpo actual.
        /// </summary>
        protected Skeleton skeleton;

        /// <summary>
        /// Índice de error.
        /// </summary>
        protected float ERROR = 0.05F;
        protected float ERROR_PERCENT = 4.5F;
        protected double angle = 30;

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        protected readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));
        protected readonly Brush underPositionJointBrush = Brushes.Yellow;
        protected readonly Brush upPositionJointBrush = Brushes.Turquoise;
        protected readonly Brush inferredJointBrush = Brushes.Yellow;

        /// <summary>
        /// Pen used for drawing bones that are currently tracked
        /// </summary>
        protected readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);
        protected readonly Pen failBonePen = new Pen(Brushes.Red, 6);
        protected readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        /// <summary>
        /// Variable de control para la finalización del movimiento.
        /// </summary>
        protected bool finished = true;

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public Movement()
        {
            skeleton = null;
        }

        /// <summary>
        /// Constructor que define el objeto Skeleton a usar.
        /// </summary>
        public Movement(Skeleton s)
        {
            setSkeleton(s);
        }

        /// <summary>
        /// Almacena el objeto Skeleton a usar.
        /// </summary>
        public virtual void setSkeleton(Skeleton s)
        {
            skeleton = s;
        }

        /// <summary>
        /// Comprueba si el movimiento ha finalizado.
        /// </summary>
        public virtual bool isFinished()
        {
            return finished;
        }

        /// <summary>
        /// Devuelve el objeto Brush con el que pintar el Joint joint.
        /// </summary>
        public virtual Brush getBrush(Joint joint)
        {
            return trackedJointBrush;
        }

        /// <summary>
        /// Devuelve el objeto Pen con el que pintar la unión entre un JointType y el JointType j.
        /// </summary>
        public virtual Pen getPen(JointType j)
        {
            return trackedBonePen;
        }

        /// <summary>
        /// Define el ángulo a usar en el movimiento.
        /// </summary>
        public virtual void setAngle(double a)
        {
            angle = a;
        }

        /// <summary>
        /// Define el porcentaje de error del movimiento.
        /// </summary>
        public virtual void setError(float e)
        {
            ERROR = e;
        }

        /// <summary>
        /// Define el porcentaje de error sobre 100 del movimiento.
        /// </summary>
        public virtual void setErrorPercent(float e)
        {
            ERROR_PERCENT = e;
            ERROR = ERROR_PERCENT / 100;
        }

    }
}
