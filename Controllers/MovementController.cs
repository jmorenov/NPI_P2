using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Kinect;
using System.Diagnostics;

namespace NPI_P2
{
    /// <summary>
    /// Clase que controla los movimientos con todos los métodos necesarios de inicialización, 
    /// finalización y consecución.
    /// </summary>
    public class MovementController
    {
        /// <summary>
        /// Lista de movimientos a realizar.
        /// </summary>
        private List<Movement> m = new List<Movement>{new Movement1(), new Movement5(), 
                                                        new Movement9(), new Movement10()};

        /// <summary>
        /// Variables de la clase.
        /// </summary>
        private int mov_i = 0;
        private double ERROR;
        private double angle;
        private bool exercise_started = false;
        private bool exercise_finished = false;
        private double time;
        private double record;
        Stopwatch watch;

        /// <summary>
        /// Inicio del ejercicio con el valor de porcentaje de error y ángulo. 
        /// También se inicia el cronómetro para almacenar el tiempo de realización del ejercicio.
        /// </summary>
        public void startExercise(double ERROR, double angle)
        {
            this.ERROR = ERROR;
            this.angle = angle;
            exercise_started = true;
            exercise_finished = false;
            m[mov_i].setAngle(angle);
            m[mov_i].setErrorPercent((float)ERROR);
            time = 0.0;
            watch = Stopwatch.StartNew();
        }

        /// <summary>
        /// Comprueba si el movimiento ha finalizado para pasar al siguiente, o si han terminado todos los movimientos.
        /// </summary>
        public void refresh()
        {
            if (exercise_started)
            {
                bool finish = false;
                finish = m[mov_i].isFinished();
                if (finish)
                {
                    mov_i++;
                    if (mov_i >= m.Count)
                    {
                        watch.Stop();
                        time = watch.ElapsedMilliseconds;
                        record = time / ERROR;
                        exercise_finished = true;
                    }
                }
            }
        }

        /// <summary>
        /// Devuelve el tiempo de realización del ejercicio.
        /// </summary>
        public double getTime()
        {
            return time;
        }

        /// <summary>
        /// Devuelve si ha terminado el ejercicio.
        /// </summary>
        public bool isFinished()
        {
            return exercise_finished;
        }

        public double getRecord()
        {

            return record;
        }

        /// <summary>
        /// Almacena el objeto Skeleton con el que trabajar en los movimientos.
        /// </summary>
        public void setSkeleton(Skeleton s)
        {
            m[mov_i].setSkeleton(s);
        }

        /// <summary>
        /// Devuelve el objeto Brush con el que pintar el Joint joint.
        /// </summary>
        public Brush getBrush(Joint joint)
        {
            return m[mov_i].getBrush(joint);
        }

        /// <summary>
        /// Devuelve el objeto Pen con el que pintar la unión entre los JointType j0 y j1.
        /// </summary>
        public Pen getPen(JointType j0, JointType j1)
        {
            return m[mov_i].getPen(j1);
        }
    }
}