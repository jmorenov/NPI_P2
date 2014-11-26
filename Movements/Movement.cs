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

        protected bool finished = true;

        public Movement()
        {
            skeleton = null;
        }

        public Movement(Skeleton s)
        {
            setSkeleton(s);
        }

        public virtual void setSkeleton(Skeleton s)
        {
            skeleton = s;
        }

        public virtual bool isFinished()
        {
            return finished;
        }

        public virtual Brush getBrush(Joint joint)
        {
            return trackedJointBrush;
        }

        public virtual Pen getPen(JointType j)
        {
            return trackedBonePen;
        }

        public virtual void setAngle(double a)
        {
            angle = a;
        }

        public virtual void setError(float e)
        {
            ERROR = e;
        }

        public virtual void setErrorPercent(float e)
        {
            ERROR_PERCENT = e;
        }

    }
}
