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
    /// Clase Movement5 basada en el movimiento 5 desarrollado por: https://github.com/AdriMedina/Practica1-NPI/tree/master/SkeletonBasics-WPF
    /// 
    /// Movimiento: Igual movimiento 1, con los brazos sobre la cabeza. Las manos no deben estar juntas, los brazos rectos sobre la cabeza.
    /// 
    /// </summary>
    class Movement5 : Movement
    {
        private readonly Pen huesosOK = new Pen(Brushes.Green, 6);
        private readonly Pen huesosFAIL = new Pen(Brushes.Red, 6);
        private readonly Pen huesoAdelantado = new Pen(Brushes.Turquoise, 6);
        private readonly Pen huesoAtrasado = new Pen(Brushes.Yellow, 6);

        private Pen huesoLeft, huesoRight;
        
        public override void setSkeleton(Skeleton s)
        {
            base.setSkeleton(s);
            check_movimiento();
        }

        public override bool isFinished()
        {
            if (brazoIzqOK() && manosSeparadas() && brazoDerOK())
                return true;
            return false;
        }

        /// <summary>
        /// Devuelve el objeto Pen con el que pintar el hueso que se une con la parte del cuerpo JointType.
        /// </summary>
        public override Pen getPen(JointType j)
        {
            if (j == JointType.ElbowLeft || j == JointType.WristLeft || j == JointType.HandLeft)
                return huesoLeft;
            else if (j == JointType.ElbowRight || j == JointType.WristRight || j == JointType.HandRight)
                return huesoRight;
            else
                return trackedBonePen;
        }

        // Comprueba si el brazo derecho está en la posición correcta
        private bool brazoDerOK()
        {
            // Coordenadas actuales del brazo derecho
            float hombroDerX = skeleton.Joints[JointType.ShoulderRight].Position.X;
            float hombroDerY = skeleton.Joints[JointType.ShoulderRight].Position.Y;

            float codoDerX = skeleton.Joints[JointType.ElbowRight].Position.X;
            float codoDerY = skeleton.Joints[JointType.ElbowRight].Position.Y;

            float munecaDerX = skeleton.Joints[JointType.WristRight].Position.X;
            float munecaDerY = skeleton.Joints[JointType.WristRight].Position.Y;

            float manoDerX = skeleton.Joints[JointType.HandRight].Position.X;
            float manoDerY = skeleton.Joints[JointType.HandRight].Position.Y;

            // Coordenadas del eje Y del brazo derecho en posición correcta
            bool brazoDerYOK = hombroDerY < codoDerY && hombroDerY < munecaDerY && hombroDerY < manoDerY &&
                codoDerY < munecaDerY && codoDerY < manoDerY &&
                munecaDerY < manoDerY;

            // Coordenadas del eje X del brazo derecho en posición correcta
            bool brazoDerXOK = Math.Abs(hombroDerX - codoDerX - munecaDerX - manoDerX) < 1.2f;


            // Devolvemos true si el brazo está en la posición correcta
            return brazoDerXOK && brazoDerYOK;

        }


        // Comprueba si el brazo derecho está en la posición correcta
        private bool brazoIzqOK()
        {
            // Coordenadas actuales del brazo izquierdo
            float hombroIzqX = skeleton.Joints[JointType.ShoulderLeft].Position.X;
            float hombroIzqY = skeleton.Joints[JointType.ShoulderLeft].Position.Y;

            float codoIzqX = skeleton.Joints[JointType.ElbowLeft].Position.X;
            float codoIzqY = skeleton.Joints[JointType.ElbowLeft].Position.Y;

            float munecaIzqX = skeleton.Joints[JointType.WristLeft].Position.X;
            float munecaIzqY = skeleton.Joints[JointType.WristLeft].Position.Y;

            float manoIzqX = skeleton.Joints[JointType.HandLeft].Position.X;
            float manoIzqY = skeleton.Joints[JointType.HandLeft].Position.Y;


            // Coordenadas del eje Y del brazo izquierdo en posición correcta
            bool brazoIzqYOK = hombroIzqY < codoIzqY && hombroIzqY < munecaIzqY && hombroIzqY < manoIzqY &&
                codoIzqY < munecaIzqY && codoIzqY < manoIzqY &&
                munecaIzqY < manoIzqY;

            // Coordenadas del eje X del brazo izquierdo en posición correcta
            bool brazoIzqXOK = Math.Abs(hombroIzqX - codoIzqX - munecaIzqX - manoIzqX) < 1.2f;


            // Devolvemos true si el brazo está en la posición correcta
            return brazoIzqXOK && brazoIzqYOK;

        }


        // Comprueba si el brazo derecho está adelantado
        private bool brazoDerAdelantado()
        {
            // Coordenadas en el eje Z y en el eje Y del hombro, codo y muñeca
            float hombroDerZ = skeleton.Joints[JointType.ShoulderRight].Position.Z;
            float codoDerZ = skeleton.Joints[JointType.ElbowRight].Position.Z;
            float munecaDerZ = skeleton.Joints[JointType.WristRight].Position.Z;

            float hombroDerY = skeleton.Joints[JointType.ShoulderRight].Position.Y;
            float codoDerY = skeleton.Joints[JointType.ElbowRight].Position.Y;
            float munecaDerY = skeleton.Joints[JointType.WristRight].Position.Y;

            // Angulos en los que consideramos que el brazo está adelantado
            int anguloMin = 40, anguloMax = 90;

            // Calculamos el angulo actual del brazo sobre el eje YZ
            double hipotenusa = Math.Sqrt(Math.Pow((munecaDerZ - hombroDerZ), 2) + Math.Pow((munecaDerY - hombroDerY), 2));
            double aux = Math.Cos((munecaDerY - hombroDerY) / hipotenusa);
            int angulo_actual = 90 - (int)((Math.Acos(aux) * 180) / 3.1416);

            // Comprobamos que el brazo esté al frente como minimo. Si el brazo está por debajo, se considera erroneo.
            bool brazoZ = hombroDerZ > codoDerZ && hombroDerZ > munecaDerZ && codoDerZ > munecaDerZ;
            bool brazoY = hombroDerY < codoDerY && hombroDerY < munecaDerY && codoDerY < munecaDerY;


            // Si está adelantado devolvemos true.
            if (angulo_actual > anguloMin && angulo_actual < anguloMax && brazoZ && brazoY)
                return true;
            else
                return false;

        }

        // Comprueba si el brazo izquierdo está adelantado
        private bool brazoIzqAdelantado()
        {
            // Coordenadas en el eje Z y en el eje Y del hombro, codo y muñeca
            float hombroIzqZ = skeleton.Joints[JointType.ShoulderLeft].Position.Z;
            float codoIzqZ = skeleton.Joints[JointType.ElbowLeft].Position.Z;
            float munecaIzqZ = skeleton.Joints[JointType.WristLeft].Position.Z;

            float hombroIzqY = skeleton.Joints[JointType.ShoulderLeft].Position.Y;
            float codoIzqY = skeleton.Joints[JointType.ElbowLeft].Position.Y;
            float munecaIzqY = skeleton.Joints[JointType.WristLeft].Position.Y;

            // Angulos en los que consideramos que el brazo está adelantado
            int anguloMin = 40, anguloMax = 90;

            // Calculamos el angulo actual del brazo sobre el eje YZ
            double hipotenusa = Math.Sqrt(Math.Pow((munecaIzqZ - hombroIzqZ), 2) + Math.Pow((munecaIzqY - hombroIzqY), 2));
            double aux = Math.Cos((munecaIzqY - hombroIzqY) / hipotenusa);
            int angulo_actual = 90 - (int)((Math.Acos(aux) * 180) / 3.1416);

            // Comprobamos que el brazo esté al frente como minimo. Si el brazo está por debajo, se considera erroneo.
            bool brazoZ = hombroIzqZ > codoIzqZ && hombroIzqZ > munecaIzqZ && codoIzqZ > munecaIzqZ;
            bool brazoY = hombroIzqY < codoIzqY && hombroIzqY < munecaIzqY && codoIzqY < munecaIzqY;


            // Si está adelantado devolvemos true.
            if (angulo_actual > anguloMin && angulo_actual < anguloMax && brazoZ && brazoY)
                return true;
            else
                return false;

        }

        // Comprueba si el brazo derecho está atrasado
        private bool brazoDerAtrasado()
        {
            // Coordenadas en el eje Z y en el eje Y del hombro, codo y muñeca
            float hombroDerZ = skeleton.Joints[JointType.ShoulderRight].Position.Z;
            float codoDerZ = skeleton.Joints[JointType.ElbowRight].Position.Z;
            float munecaDerZ = skeleton.Joints[JointType.WristRight].Position.Z;

            float hombroDerY = skeleton.Joints[JointType.ShoulderRight].Position.Y;
            float codoDerY = skeleton.Joints[JointType.ElbowRight].Position.Y;
            float munecaDerY = skeleton.Joints[JointType.WristRight].Position.Y;

            // Angulos en los que consideramos que el brazo está adelantado
            int anguloMin = 270, anguloMax = 345;

            // Calculamos el angulo actual del brazo sobre el eje YZ
            double hipotenusa = Math.Sqrt(Math.Pow((munecaDerZ - hombroDerZ), 2) + Math.Pow((munecaDerY - hombroDerY), 2));
            double aux = Math.Cos((munecaDerY - hombroDerY) / hipotenusa);
            int angulo_actual = 90 - (int)((Math.Acos(aux) * 180) / 3.1416);

            // Comprobamos que el brazo esté al frente como minimo. Si el brazo está por debajo, se considera erroneo.
            bool brazoZ = hombroDerZ < codoDerZ && hombroDerZ < munecaDerZ && codoDerZ < munecaDerZ;
            bool brazoY = hombroDerY < codoDerY && hombroDerY < munecaDerY && codoDerY < munecaDerY;


            // Si está adelantado devolvemos true.
            if (angulo_actual > anguloMin && angulo_actual < anguloMax && brazoZ && brazoY)
                return true;
            else
                return false;
        }

        // Comprueba si el brazo izquierdo está atrasado
        private bool brazoIzqAtrasado()
        {
            // Coordenadas en el eje Z y en el eje Y del hombro, codo y muñeca
            float hombroIzqZ = skeleton.Joints[JointType.ShoulderLeft].Position.Z;
            float codoIzqZ = skeleton.Joints[JointType.ElbowLeft].Position.Z;
            float munecaIzqZ = skeleton.Joints[JointType.WristLeft].Position.Z;

            float hombroIzqY = skeleton.Joints[JointType.ShoulderLeft].Position.Y;
            float codoIzqY = skeleton.Joints[JointType.ElbowLeft].Position.Y;
            float munecaIzqY = skeleton.Joints[JointType.WristLeft].Position.Y;

            // Angulos en los que consideramos que el brazo está adelantado
            int anguloMin = 270, anguloMax = 345;

            // Calculamos el angulo actual del brazo sobre el eje YZ
            double hipotenusa = Math.Sqrt(Math.Pow((munecaIzqZ - hombroIzqZ), 2) + Math.Pow((munecaIzqY - hombroIzqY), 2));
            double aux = Math.Cos((munecaIzqY - hombroIzqY) / hipotenusa);
            int angulo_actual = 90 - (int)((Math.Acos(aux) * 180) / 3.1416);

            // Comprobamos que el brazo esté al frente como minimo. Si el brazo está por debajo, se considera erroneo.
            bool brazoZ = hombroIzqZ < codoIzqZ && hombroIzqZ < munecaIzqZ && codoIzqZ < munecaIzqZ;
            bool brazoY = hombroIzqY < codoIzqY && hombroIzqY < munecaIzqY && codoIzqY < munecaIzqY;


            // Si está adelantado devolvemos true.
            if (angulo_actual > anguloMin && angulo_actual < anguloMax && brazoZ && brazoY)
                return true;
            else
                return false;
        }

        // Controla que las manos estén separadas. 
        private bool manosSeparadas()
        {
            float munecaDerX = skeleton.Joints[JointType.WristRight].Position.X;
            float munecaIzqX = skeleton.Joints[JointType.WristLeft].Position.X;

            return Math.Abs(munecaDerX - munecaIzqX) >= 0.2f;

        }

        // Se encarga de controlar el movimiento número 5
        private void check_movimiento()
        {
            // Left Arm
            if (brazoIzqAdelantado())
            {
                huesoLeft = huesoAdelantado;
            }
            else if (brazoIzqAtrasado())
            {
                huesoLeft = huesoAtrasado;
            }
            else if (brazoIzqOK() && manosSeparadas())
            {
                huesoLeft = huesosOK;
            }
            else
            {
                huesoLeft = huesosFAIL;
            }

            // Right Arm
            if (brazoDerAdelantado())
            {
                huesoRight = huesoAdelantado;

            }
            else if (brazoDerAtrasado())
            {
                huesoRight = huesoAtrasado;
            }
            else if (brazoDerOK() && manosSeparadas())
            {
                huesoRight = huesosOK;
            }
            else
            {
                huesoRight = huesosFAIL;
            }
        }
    }
}
