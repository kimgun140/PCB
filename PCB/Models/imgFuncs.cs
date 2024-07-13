using OpenCvSharp;
using PCB.VIEW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCB.Models
{
    internal class ImgFuncs
    {

        public Mat frame { get; set; }

        public ImgFuncs()
        {
            frame = new Mat();
        }


        public OpenCvSharp.Mat MakeFrame(OpenCvSharp.VideoCapture videoCapture)
        {
            videoCapture.Read(frame);


            return frame;
        }


        public OpenCvSharp.Mat PreProcessing()
        {

            Mat gray = new Mat();
            Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);

            Mat gaus = new Mat();
            Cv2.GaussianBlur(gray, gaus, new OpenCvSharp.Size(1, 1), 9);

            Mat binary = new Mat();

            Cv2.Threshold(gaus, binary, 100, 255, ThresholdTypes.Binary);

            Mat element = new Mat();

            Mat Ero = new Mat(1, 1, MatType.CV_8UC1);
            Mat dil = new Mat();

            Mat asdf = new OpenCvSharp.Mat(3, 3, MatType.CV_8UC1);
            Cv2.Canny(binary, asdf, 50, 255);
            //Cv2.ImShow("canny", asdf);

            Cv2.Erode(binary, Ero, element);
            Cv2.Erode(Ero, Ero, element);
            Cv2.Dilate(Ero, dil, element);
            Cv2.Erode(dil, Ero, element);
            Cv2.Dilate(Ero, dil, element);
            //Cv2.Erode(dil, Ero, element);
            //Cv2.Dilate(Ero, dil, element);
            //Cv2.Erode(dil, Ero, element);
            //Cv2.Dilate(Ero, dil, element);
            //Cv2.Erode(dil, Ero, element);
            //Cv2.Dilate(Ero, dil, element);
            //Cv2.Erode(dil, Ero, element);
            //Cv2.Dilate(Ero, dil, element);
            //Cv2.Dilate(dil, dil, element);


            Mat src = new Mat();
            Cv2.Threshold(dil, src, 0, 255, ThresholdTypes.BinaryInv);
            //Cv2.ImShow("src", src);

            return src;

        }

        public bool MakeContours(OpenCvSharp.Mat src)
        {
            Mat mat;
            OpenCvSharp.Point[][] contours;
            OpenCvSharp.HierarchyIndex[] hierarchy;

            Mat rsrc = src;

            Cv2.FindContours(src, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

            int cnt = 0;



            for (cnt = 1; cnt < contours.Length; cnt++)
            {



                double areaSize = Cv2.ContourArea(contours[cnt]);
                if (areaSize > 300)
                {

                    OpenCvSharp.Rect boundingRect12 = Cv2.BoundingRect(contours[cnt]);
                    Cv2.DrawContours(frame, contours, cnt, Scalar.AliceBlue, 2);

                    Cv2.Rectangle(frame, boundingRect12, Scalar.Yellow, 2);
                    Cv2.Rectangle(src, boundingRect12, Scalar.Black, 5);


                    mat = new Mat(src, boundingRect12);
                    Mat srcSub = src.SubMat(boundingRect12);
                    Mat mat2 = new Mat(rsrc, boundingRect12);

                    //Cv2.ImShow("cap", mat);
                    //Cv2.ImShow("judgframe", frame);
                    //Cv2.ImShow("srcSub" + cnt, srcSub);


                    if (JudgeErr(srcSub) == false)
                    {
                        return false;
                    }

                }

            }




            return true;
        }
        public void OnlyMakeContours(OpenCvSharp.Mat src)
        {
            Mat mat;
            OpenCvSharp.Point[][] contours;
            OpenCvSharp.HierarchyIndex[] hierarchy;
            Mat asdf = new OpenCvSharp.Mat(3, 3, MatType.CV_8UC1);
            Cv2.Canny(src, src, 50, 255);
            //Cv2.ImShow("canny", src);
            Cv2.FindContours(src, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
            int cnt = 0;


            for (cnt = 0; cnt < contours.Length; cnt++)
            {
                OpenCvSharp.Rect boundingRect12 = Cv2.BoundingRect(contours[cnt]);
                Cv2.DrawContours(frame, contours, cnt, Scalar.AliceBlue, 2);
                Cv2.Rectangle(frame, boundingRect12, Scalar.Yellow, 2);
                mat = new Mat(src, boundingRect12);
                //Cv2.ImShow("frame", frame);

            }

        }

        public bool JudgeErr(OpenCvSharp.Mat mat)
        {


            int whitePixelCount = Cv2.CountNonZero(mat);// 흑백사진

            if (whitePixelCount > 0)
            {

                return false;
            }
            else
            {

                return true;
            }


        }
        public void save_img() //저장
        {
            string local_address = "C:\\Users\\lms\\Desktop\\PCB1\\PCB\\Resources\\save_img.jpg";
            Cv2.ImWrite(local_address, frame);

        }
    }
}
