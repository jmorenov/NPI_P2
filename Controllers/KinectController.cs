using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Kinect;
using System.Windows.Forms;

namespace NPI_P2
{
    /// <summary>
    /// Clase que controla el Kinect con todos los métodos necesarios de 
    /// inicialiación, finalización, pintado de pantalla, ...
    /// </summary>
    public class KinectController
    {
        /// <summary>
        /// Variables de la clase.
        /// </summary>
        private const float RenderWidth = 640.0f;
        private const float RenderHeight = 480.0f;
        private const double JointThickness = 3;
        private const double BodyCenterThickness = 10;
        private const double ClipBoundsThickness = 10;

        /// <summary>
        /// Objeto Pen con el que pintar los huesos inferidos.
        /// </summary>
        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        /// <summary>
        /// Objetos Brush para el pintado de Joint.
        /// </summary>
        private readonly Brush centerPointBrush = Brushes.Blue;
        private readonly Brush inferredJointBrush = Brushes.Yellow;
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Sensor del Kinect.
        /// </summary>
        private KinectSensor sensor;

        /// <summary>
        /// Objetos para el pintado de la pantalla.
        /// </summary>
        private DrawingGroup drawingGroup;
        private DrawingImage imageSource;
        private BitmapSource source;

        private Image image;

        /// <summary>
        /// Variables de control de la ejecución del Kinect.
        /// </summary>
        private bool connected = false;
        private bool started = false;

        private bool WriteText = false;
        private string TextToWrite = "";

        private Timer timer = new Timer();

        /// <summary>
        /// Controlador de los Movimientos.
        /// </summary>
        public MovementController movController = new MovementController();

        /// <summary>
        /// Constructor en el que se definen los objetos para pintar 
        /// en pantalla y se comprueban los sensores.
        /// </summary>
        public KinectController()
        {
            checkSensors();
        }

        /// <summary>
        /// Comprobación de los sensores.s
        /// </summary>
        private void checkSensors()
        {
            this.sensor = null;
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }
            connected = false;
            if (null != this.sensor)
                connected = true;
        }

        /// <summary>
        /// Inicio del Kinect.
        /// </summary>
        public bool start()
        {
            try
            {
                this.sensor.SkeletonStream.Enable();
                this.sensor.ColorStream.Enable();
                this.sensor.SkeletonFrameReady += SensorSkeletonFrameReady;
                this.sensor.ColorFrameReady += SensorColorFrameReady;
                this.sensor.Start();
                this.drawingGroup = new DrawingGroup();
                this.imageSource = new DrawingImage(this.drawingGroup);
                started = true;
            }
            catch (IOException)
            {
                started = false;
                this.sensor = null;
            }
            return started;
        }

        /// <summary>
        /// Devuelve si el Kinect está iniciado.
        /// </summary>
        public bool isStarted()
        {
            return started;
        }

        /// <summary>
        /// Devuelve si el Kinect está conectado.
        /// </summary>
        public bool isConnected()
        {
            return connected;
        }

        /// <summary>
        /// Devuelve la imagen de Skeleton que vamos a pintar en pantalla.
        /// </summary>
        public void setImageSkeleton(ref Image img)
        {
            img.Source = this.imageSource;
        }

        /// <summary>
        /// Devuelve la imagen de Image Stream que vamos a pintar en pantalla.
        /// </summary>
        public void setImageSource(ref Image img)
        {
            image = img;
        }

        /// <summary>
        /// Finaliza la ejecución del Kinect.
        /// </summary>
        public void close()
        {
            if (null != this.sensor)
            {
                this.sensor.Stop();
                started = false;
                connected = false;
            }
        }

        /// <summary>
        /// Event handler for Kinect sensor's SkeletonFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            bool receivedData = false;
            byte[] pixelData = null;
            using (ColorImageFrame CFrame = e.OpenColorImageFrame())
            {
                if (CFrame != null)
                {
                    pixelData = new byte[CFrame.PixelDataLength];
                    CFrame.CopyPixelDataTo(pixelData);
                    receivedData = true;
                }
            }

            if (receivedData)
            {
                source = BitmapSource.Create(640, 480, 96, 96, PixelFormats.Bgr32, null, pixelData, 640 * 4);

                if (WriteText)
                {
                    System.Drawing.Bitmap bmp = BitmapFromSource(source);
                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
                    {
                        g.DrawString(TextToWrite, new System.Drawing.Font("Tahoma", 20), System.Drawing.Brushes.Red, new System.Drawing.PointF(3, 3));
                        g.Flush();
                    }
                    source = ConvertBitmap(bmp);
                }
                //image.Source = source;
            }
        }

        private System.Drawing.Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            System.Drawing.Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
            }
            return bitmap;
        }

        public static BitmapSource ConvertBitmap(System.Drawing.Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                          source.GetHbitmap(),
                          IntPtr.Zero,
                          Int32Rect.Empty,
                          BitmapSizeOptions.FromEmptyOptions());
        }

        private System.Drawing.Bitmap GetBitmap(BitmapSource source)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(
                  source.PixelWidth,
                  source.PixelHeight,
                  System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                  System.Drawing.Imaging.BitmapData data = bmp.LockBits(
                  new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size),
                  System.Drawing.Imaging.ImageLockMode.WriteOnly,
                  System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                source.CopyPixels(
                  Int32Rect.Empty,
                  data.Scan0,
                  data.Height * data.Stride,
                  data.Stride);
                bmp.UnlockBits(data);
            return bmp;
        }

        /// <summary>
        /// Event handler for Kinect sensor's SkeletonFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            if (movController.isFinished())
                return;
            Skeleton[] skeletons = new Skeleton[0];

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            using (DrawingContext dc = this.drawingGroup.Open())
            //using (DrawingContext dc = System.Drawing.Graphics.FromImage(source))
            {
                //dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));
                dc.DrawImage(source, new Rect(0.0, 0.0, RenderWidth, RenderHeight));
                if (skeletons.Length != 0)
                {
                    foreach (Skeleton skel in skeletons)
                    {
                        RenderClippedEdges(skel, dc);

                        if (skel.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            movController.setSkeleton(skel);
                            this.DrawBonesAndJoints(skel, dc);
                            movController.refresh();
                        }
                        else if (skel.TrackingState == SkeletonTrackingState.PositionOnly)
                        {
                            dc.DrawEllipse(this.centerPointBrush, null, this.SkeletonPointToScreen(skel.Position), BodyCenterThickness, BodyCenterThickness);
                        }
                    }
                }
                this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
            }
            
        }

        /// <summary>
        /// Draws indicators to show which edges are clipping skeleton data
        /// </summary>
        /// <param name="skeleton">skeleton to draw clipping information for</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private static void RenderClippedEdges(Skeleton skeleton, DrawingContext drawingContext)
        {
            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Bottom))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, RenderHeight - ClipBoundsThickness, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Top))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Left))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, ClipBoundsThickness, RenderHeight));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Right))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(RenderWidth - ClipBoundsThickness, 0, ClipBoundsThickness, RenderHeight));
            }
        }

        /// <summary>
        /// Draws a skeleton's bones and joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawBonesAndJoints(Skeleton skeleton, DrawingContext drawingContext)
        {
            // Render Torso
            //this.DrawBone(skeleton, drawingContext, JointType.Head, JointType.ShoulderCenter);
            //this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
            //this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
            //this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.Spine);
            /*this.DrawBone(skeleton, drawingContext, JointType.Spine, JointType.HipCenter);
            this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft);
            this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight);*/

            // Left Leg
            this.DrawBone(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft);
            this.DrawBone(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft);
            this.DrawBone(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft);

            // Right Leg
            this.DrawBone(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight);
            this.DrawBone(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight);
            this.DrawBone(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight);

            // Left Arm
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
            this.DrawBone(skeleton, drawingContext, JointType.ElbowLeft, JointType.WristLeft);
            this.DrawBone(skeleton, drawingContext, JointType.WristLeft, JointType.HandLeft);

            // Right Arm
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderRight, JointType.ElbowRight);
            this.DrawBone(skeleton, drawingContext, JointType.ElbowRight, JointType.WristRight);
            this.DrawBone(skeleton, drawingContext, JointType.WristRight, JointType.HandRight);

            // Render Joints
            foreach (Joint joint in skeleton.Joints)
            {
                Brush drawBrush = null;

                if (joint.TrackingState == JointTrackingState.Tracked)
                {
                    drawBrush = movController.getBrush(joint);
                }
                else if (joint.TrackingState == JointTrackingState.Inferred)
                {
                    drawBrush = this.inferredJointBrush;
                }

                if (drawBrush != null && drawBrush != trackedJointBrush)
                {
                    drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
                }
            }
        }

        /// <summary>
        /// Maps a SkeletonPoint to lie within our render space and converts to Point
        /// </summary>
        /// <param name="skelpoint">point to map</param>
        /// <returns>mapped point</returns>
        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            // Convert point to depth space.  
            // We are not using depth directly, but we do want the points in our 640x480 output resolution.
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }

        protected readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);
        /// <summary>
        /// Draws a bone line between two joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw bones from</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// <param name="jointType0">joint to start drawing from</param>
        /// <param name="jointType1">joint to end drawing at</param>
        private void DrawBone(Skeleton skeleton, DrawingContext drawingContext, JointType jointType0, JointType jointType1)
        {
            Joint joint0 = skeleton.Joints[jointType0];
            Joint joint1 = skeleton.Joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == JointTrackingState.NotTracked ||
                joint1.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }

            // Don't draw if both points are inferred
            if (joint0.TrackingState == JointTrackingState.Inferred &&
                joint1.TrackingState == JointTrackingState.Inferred)
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            Pen drawPen = this.inferredBonePen;
            if (joint0.TrackingState == JointTrackingState.Tracked && joint1.TrackingState == JointTrackingState.Tracked)
            {
                drawPen = movController.getPen(jointType0, jointType1);
            }
            if(drawPen.Brush != trackedBonePen.Brush)
                drawingContext.DrawLine(drawPen, this.SkeletonPointToScreen(joint0.Position), this.SkeletonPointToScreen(joint1.Position));
        }
    }
}