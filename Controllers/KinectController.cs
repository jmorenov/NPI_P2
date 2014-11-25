using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPI_P2.Controllers
{
    class KinectController
    {
        private static KinectController kinect = null;
        private KinectController()
        {
            kinect = new KinectController();
        }

        public KinectController getInstance()
        {
            return kinect;
        }
    }
}
