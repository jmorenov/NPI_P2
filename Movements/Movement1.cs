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
    class Movement1
    {
        Skeleton skeleton;
        private readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);
        private readonly Pen correctMove = new Pen(Brushes.Green, 5);
        private readonly Pen incorrectMove = new Pen(Brushes.Red, 5);
        private readonly Pen closeToMove = new Pen(Brushes.Yellow, 5);

        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        public Movement1() { }

        public Movement1(Skeleton s) { setSkeleton(s); }

        public void setSkeleton(Skeleton s)
        {
            skeleton = s;
        }

        public bool isFinished()
        {
            return false;
        }

        public Brush getBrush(Joint joint)
        {
            return trackedJointBrush;
        }

        /// <summary>
        /// Devuelve el objeto Pen con el que pintar el hueso que se une con la parte del cuerpo JointType.
        /// </summary>
        public Pen getPen(JointType j)
        {
            if ((IsAlignedBodyAndArms() && AreFeetTogether()) || (IsAlignedBodyAndArms() && AreFeetSeparate()))
            {
                return correctMove;
            }
            else if ((IsAlignedBodyAndArms() && !AreFeetSeparate() && !AreFeetTogether()) 
                || (!IsAlignedBodyAndArms() && AreFeetSeparate() && !AreFeetTogether()) 
                || (!IsAlignedBodyAndArms() && !AreFeetSeparate() && AreFeetTogether()))
            {
                return closeToMove;
            }
            return incorrectMove;
        }

        // boolean method that return true if body is completely aligned and arms are in a relaxed position
        private bool IsAlignedBodyAndArms()
        {
            double HipCenterPosX = skeleton.Joints[JointType.HipCenter].Position.X;
            double HipCenterPosY = skeleton.Joints[JointType.HipCenter].Position.Y;
            double HipCenterPosZ = skeleton.Joints[JointType.HipCenter].Position.Z;

            double ShoulCenterPosX = skeleton.Joints[JointType.ShoulderCenter].Position.X;
            double ShoulCenterPosY = skeleton.Joints[JointType.ShoulderCenter].Position.Y;
            double ShoulCenterPosZ = skeleton.Joints[JointType.ShoulderCenter].Position.Z;

            double HeadCenterPosX = skeleton.Joints[JointType.Head].Position.X;
            double HeadCenterPosY = skeleton.Joints[JointType.Head].Position.Y;
            double HeadCenterPosZ = skeleton.Joints[JointType.Head].Position.Z;

            double ElbLPosX = skeleton.Joints[JointType.ElbowLeft].Position.X;
            double ElbLPosY = skeleton.Joints[JointType.ElbowLeft].Position.Y;

            double ElbRPosX = skeleton.Joints[JointType.ElbowRight].Position.X;
            double ElbRPosY = skeleton.Joints[JointType.ElbowRight].Position.Y;

            double WriLPosX = skeleton.Joints[JointType.WristLeft].Position.X;
            double WriLPosY = skeleton.Joints[JointType.WristLeft].Position.Y;
            double WriLPosZ = skeleton.Joints[JointType.WristLeft].Position.Z;

            double WriRPosX = skeleton.Joints[JointType.WristRight].Position.X;
            double WriRPosY = skeleton.Joints[JointType.WristRight].Position.Y;
            double WriRPosZ = skeleton.Joints[JointType.WristRight].Position.Z;

            double ShouLPosX = skeleton.Joints[JointType.ShoulderLeft].Position.X;
            double ShouLPosY = skeleton.Joints[JointType.ShoulderLeft].Position.Y;
            double ShouLPosZ = skeleton.Joints[JointType.ShoulderLeft].Position.Z;

            double ShouRPosX = skeleton.Joints[JointType.ShoulderRight].Position.X;
            double ShouRPosY = skeleton.Joints[JointType.ShoulderRight].Position.Y;
            double ShouRPosZ = skeleton.Joints[JointType.ShoulderRight].Position.Z;

            //have to change to correspond to the 5% error
            //distance from Shoulder to Wrist for the projection in line with shoulder
            double distShouLtoWristL = ShouLPosY - WriLPosY;
            //caldulate admited error 5% that correspond to 9 degrees for each side
            double radian = (9 * Math.PI) / 180;
            double DistErrorL = distShouLtoWristL * Math.Tan(radian);

            double distShouLtoWristR = ShouRPosY - WriRPosY;
            //caldulate admited error 5% that correspond to 9 degrees for each side

            double DistErrorR = distShouLtoWristR * Math.Tan(radian);
            //double ProjectionWristX = ShouLPosX;
            //double ProjectionWristZ = WriLPosZ;

            //determine of projected point from shoulder to wrist LEFT and RIGHT and then assume error
            double ProjectedPointWristLX = ShouLPosX;
            double ProjectedPointWristLY = WriLPosY;
            double ProjectedPointWristLZ = ShouLPosZ;

            double ProjectedPointWristRX = ShouRPosX;
            double ProjectedPointWristRY = WriRPosY;
            double ProjectedPointWristRZ = ShouRPosZ;


            //Create method to verify if the center of the body is completely aligned
            //head with shoulder center and with hip center
            if (Math.Abs(HeadCenterPosX-ShoulCenterPosX)<=0.05 && Math.Abs(ShoulCenterPosX-HipCenterPosX)<=0.05)
            {
                //if position of left wrist is between [ProjectedPointWrist-DistError,ProjectedPointWrist+DistError]
                if (Math.Abs(WriLPosX-ProjectedPointWristLX)<= DistErrorL && Math.Abs(WriRPosX-ProjectedPointWristRX )<= DistErrorR)
                {
                    return true;
                }
                else return false;
            }
            else return false;

        }
        //first position to be Tracked and Accepted
        private bool AreFeetTogether()
        {
                foreach (Joint joint in skeleton.Joints)
                {
                    if (joint.TrackingState == JointTrackingState.Tracked)
                    {//first verify if the body is alignet and arms are in a relaxed position

                        //{here verify if the feet are together
                        //use the same strategy that was used in the previous case of the arms in a  relaxed position
                        double HipCenterPosX = skeleton.Joints[JointType.HipCenter].Position.X;
                        double HipCenterPosY = skeleton.Joints[JointType.HipCenter].Position.Y;
                        double HipCenterPosZ = skeleton.Joints[JointType.HipCenter].Position.Z;

                        //if left ankle is very close to right ankle then verify the rest of the skeleton points
                        //if (skeleton.Joints[JointType.AnkleLeft].Equals(skeleton.Joints[JointType.AnkleRight])) 
                        double AnkLPosX = skeleton.Joints[JointType.AnkleLeft].Position.X;
                        double AnkLPosY = skeleton.Joints[JointType.AnkleLeft].Position.Y;
                        double AnkLPosZ = skeleton.Joints[JointType.AnkleLeft].Position.Z;

                        double AnkRPosX = skeleton.Joints[JointType.AnkleRight].Position.X;
                        double AnkRPosY = skeleton.Joints[JointType.AnkleRight].Position.Y;
                        double AnkRPosZ = skeleton.Joints[JointType.AnkleRight].Position.Z;
                        //assume that the distance Y between HipCenter to each foot is the same
                        double distHiptoAnkleL = HipCenterPosY - AnkLPosY;
                        //caldulate admited error 5% that correspond to 9 degrees for each side
                        double radian1 = (4.5 * Math.PI) / 180;
                        double DistErrorL = distHiptoAnkleL * Math.Tan(radian1);
                        //determine of projected point from HIP CENTER to LEFT ANKLE and RIGHT and then assume error
                        double ProjectedPointFootLX = HipCenterPosX;
                        double ProjectedPointFootLY = AnkLPosY;
                        double ProjectedPointFootLZ = HipCenterPosZ;



                        // could variate AnkLposX and AnkLPosY
                        if (Math.Abs(AnkLPosX - ProjectedPointFootLX) <= DistErrorL && Math.Abs(AnkRPosX - ProjectedPointFootLX) <= DistErrorL)
                            return true;
                        else
                            return false;
                  
                    }//CLOSE if (joint.TrackingState == JointTrackingState.Tracked)
                    else return false;
                }//close foreach
            return false;
        }//close method AreFeetTogether
        //method for the second position feet separate between 60 degrees to be accepted
        private bool AreFeetSeparate()
        {
                foreach (Joint joint in skeleton.Joints)
                {
                    if (joint.TrackingState == JointTrackingState.Tracked)
                    {//first verify if the body is alignet and arms are in a relaxed position


                        //{//here verify if the feet are together
                        //use the same strategy that was used in the previous case of the arms in a  relaxed position
                        double HipCenterPosX = skeleton.Joints[JointType.HipCenter].Position.X;
                        double HipCenterPosY = skeleton.Joints[JointType.HipCenter].Position.Y;
                        double HipCenterPosZ = skeleton.Joints[JointType.HipCenter].Position.Z;

                        //if left ankle is very close to right ankle then verify the rest of the skeleton points
                        //if (skeleton.Joints[JointType.AnkleLeft].Equals(skeleton.Joints[JointType.AnkleRight])) 
                        double AnkLPosX = skeleton.Joints[JointType.AnkleLeft].Position.X;
                        double AnkLPosY = skeleton.Joints[JointType.AnkleLeft].Position.Y;
                        double AnkLPosZ = skeleton.Joints[JointType.AnkleLeft].Position.Z;

                        double AnkRPosX = skeleton.Joints[JointType.AnkleRight].Position.X;
                        double AnkRPosY = skeleton.Joints[JointType.AnkleRight].Position.Y;
                        double AnkRPosZ = skeleton.Joints[JointType.AnkleRight].Position.Z;
                        //assume that the distance Y between HipCenter to each foot is the same
                        double distHiptoAnkleL = HipCenterPosY - AnkLPosY;
                        //caldulate admited error 5% that correspond to 9 degrees for each side
                        double radian1 = (4.5 * Math.PI) / 180;
                        double DistErrorL = distHiptoAnkleL * Math.Tan(radian1);
                        //determine of projected point from HIP CENTER to LEFT ANKLE and RIGHT and then assume error
                        double ProjectedPointFootLX = HipCenterPosX;
                        double ProjectedPointFootLY = AnkLPosY;
                        double ProjectedPointFootLZ = HipCenterPosZ;

                        double radian2 = (30 * Math.PI) / 180;
                        double DistSeparateFoot = distHiptoAnkleL * Math.Tan(radian2);
                        //DrawingVisual MyDrawingVisual = new DrawingVisual();


                        // could variate AnkLposX and AnkLPosY
                        if (Math.Abs(AnkRPosX-AnkLPosX) <= Math.Abs( DistSeparateFoot + DistErrorL) && Math.Abs( AnkRPosX - AnkLPosX) >= Math.Abs((DistSeparateFoot) - DistErrorL))
                            return true;
                        else return false;
                        

                    }//CLOSE if (joint.TrackingState == JointTrackingState.Tracked)
                    else return false;
                }//close foreach
            return false;
        }//close method AreFeetseparate
    }
    }
