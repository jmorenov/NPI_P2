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
    /// Clase Movement7 basada en el movimiento 7 desarrollado por: https://github.com/jmorenov/SkeletonBasics-WPF-Mov7
    /// 
    /// Movimiento: Mover los brazos rectos al frente, las manos no deben estar juntas.
    /// 
    /// Clase que realiza los cálculos necesarios para comprobar, 
    /// pasado un objeto Skeleton, si los brazos están en posición recta, 
    /// de frente y las manos sin juntarse.
    /// </summary>
    class Movement7 : Movement
    {
        /// <summary>
        /// Lista con los JoinType que hay que comprobar para que el movimiento sea el correcto.
        /// </summary>
        private static List<JointType> jointsTypes = new List<JointType> 
                                                                            { JointType.ShoulderLeft, JointType.ElbowLeft, JointType.WristLeft, JointType.HandLeft,
                                                                            JointType.ShoulderRight, JointType.ElbowRight, JointType.WristRight, JointType.HandRight};
        /// <summary>
        /// Lista de tamaño igual al número de JoinType que hay que comprobar, 
        /// en esta lista se almacenan los valores de error de cada posición.
        /// </summary>
        private List<int> diff_positions = new List<int>(new int[jointsTypes.Count]);


        /// <summary>
        /// Inicializa la detección del cuerpo, se reinician los calculos 
        /// con el nuevo valor de Skeleton también.
        /// </summary>
        public override void setSkeleton(Skeleton s)
        {
            base.setSkeleton(s);
            checkArms();
        }

        /// <summary>
        /// Calcula la diferencia entre dos valores.
        /// </summary>
        private float diff(float v1, float v2) { return Math.Abs(v1 - v2); }

        /// <summary>
        /// Comprueba el error que hay entre dos puntos.
        /// </summary>
        /// <return> 0 : Posición correcta. </return>
        /// <return> 1: Posición incorrecta y punto por encima. </return>
        /// <return> -1: Posición incorrecta y punto por debajo. </return>
        private int checkPoints(SkeletonPoint P1, SkeletonPoint P2)
        {
            if (diff(P1.X, P2.X) <= ERROR && diff(P1.Y, P2.Y) <= ERROR)
                return 0;
            else
            {
                if (P1.Y > P2.Y)
                    return -1;
                else
                    return 1;
            }
        }

        /// <summary>
        /// Realiza los calculos para comprobar si las posiciones de los brazos son correctas.
        /// Almacena los resultados en la lista de enteros diff_positions.
        /// </summary>
        private void checkArms()
        {
            if (skeleton != null)
            {
                SkeletonPoint p1, p2;
                finished = true;
                for (int i = 0; i < 7; i++)
                {
                    p1 = skeleton.Joints[jointsTypes[i]].Position;
                    p2 = skeleton.Joints[jointsTypes[i + 1]].Position;

                    diff_positions[i + 1] = checkPoints(p1, p2);
                    if (diff_positions[i + 1] != 0)
                        finished = false;

                    if (i == 2)
                        i++;
                }
            }
        }

        /// <summary>
        /// Devuelve el objeto Brush con el que pintar la posición del cuerpo Joint.
        /// </summary>
        public override Brush getBrush(Joint joint)
        {
            if (jointsTypes.Contains(joint.JointType))
            {
                int i = jointsTypes.IndexOf(joint.JointType);
                if (diff_positions[i] == -1)
                    return underPositionJointBrush;
                else if (diff_positions[i] == 1)
                    return upPositionJointBrush;
            }
            return trackedJointBrush;
        }

        /// <summary>
        /// Devuelve el objeto Pen con el que pintar el hueso que se une con la parte del cuerpo JointType.
        /// </summary>
        public override Pen getPen(JointType j)
        {
            if (jointsTypes.Contains(j))
            {
                int i = jointsTypes.IndexOf(j);
                if (diff_positions[i] != 0)
                    return failBonePen;
            }
            return trackedBonePen;
        }
    }
}