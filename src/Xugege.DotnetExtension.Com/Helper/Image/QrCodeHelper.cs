//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xugege.DotnetExtension.Com.Extension;
//using Xugege.DotnetExtension.Com.Helper;
//using ZXing.Common;
//using ZXing.Core;
////using Abp.Extensions;
////using Abp.IO;
////using ZXing;
////using ZXing.Common;
////using ZXing.QrCode;
////using ZXing.QrCode.Internal;

//namespace MyCompanyName.AbpZeroTemplate.Com.Helper.Image
//{
//    public class QrCodeHelper
//    {

//        /// <summary>
//        /// 生成二维码图片
//        /// </summary>
//        /// <param name="message">信息</param>
//        /// <param name="imgPath">图片存放路径</param>
//        /// <param name="logo">logo图片地址</param>
//        /// <param name="imgHeight">图片的高</param>
//        /// <param name="imgWidth">图片的宽</param>
//        public static void GenreateQrCode(string message, string imgPath, string logo = "", int imgHeight = 300, int imgWidth = 300)
//        {

//            string d = Path.GetDirectoryName(imgPath);
//            DirectoryHelper.CreateIfNotExists(d);

//            if (!logo.IsNullOrEmpty())
//            {
//                if (!File.Exists(logo))
//                {
//                    return;
//                }

//                GenreateQrCodeWidthLogo(message, imgPath, logo, imgHeight, imgWidth);
//            }
//            else
//            {
//                BarcodeWriter writer = new BarcodeWriter() { Format = BarcodeFormat.QR_CODE };
//                QrCodeEncodingOptions options = new QrCodeEncodingOptions()
//                {
//                    CharacterSet = "UTF-8",
//                    Height = imgHeight,
//                    Width = imgWidth,
//                    Margin = 1
//                };

//                writer.Options = options;
//                var map = writer.Write(message);
//                map.Save(imgPath, ImageFormat.Png);
//                map.Dispose();
//            }

//        }
//        /// <summary>
//        /// 生成二维码 带logo
//        /// </summary>
//        /// <param name="message"></param>
//        /// <param name="imgPath"></param>
//        /// <param name="logo"></param>
//        /// <param name="imgHeight"></param>
//        /// <param name="imgWidth"></param>
//        public static void GenreateQrCodeWidthLogo(string message, string imgPath, string logo = "", int imgHeight = 300, int imgWidth = 300)
//        {

//            string d = Path.GetDirectoryName(imgPath);
//            DirectoryHelper.CreateIfNotExists(d);

//            if (!File.Exists(logo))
//            {
//                return;
//            }

//            Bitmap logoImg = new Bitmap(logo);

//            //构造二维码写码器
//            MultiFormatWriter writer = new MultiFormatWriter();
//            Dictionary<EncodeHintType, object> hint =
//                new System.Collections.Generic.Dictionary<EncodeHintType, object>
//                {
//                    {EncodeHintType.CHARACTER_SET, "UTF-8"},
//                    {EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H},
//                    {EncodeHintType.MARGIN,1 }
//                };
//            //生成二维码 
//            BitMatrix bm = writer.encode(message, BarcodeFormat.QR_CODE, imgHeight, imgWidth, hint);
//            BarcodeWriter barcodeWriter = new BarcodeWriter();
//            Bitmap map = barcodeWriter.Write(bm);

//            //获取二维码实际尺寸（去掉二维码两边空白后的实际尺寸）
//            int[] rectangle = bm.getEnclosingRectangle();

//            //计算插入图片的大小和位置
//            int middleW = Math.Min((int)(rectangle[2] / 3.5), logoImg.Width);
//            int middleH = Math.Min((int)(rectangle[3] / 3.5), logoImg.Height);
//            int middleL = (map.Width - middleW) / 2;
//            int middleT = (map.Height - middleH) / 2;

//            //将img转换成bmp格式，否则后面无法创建Graphics对象
//            Bitmap bmpimg = new Bitmap(map.Width, map.Height, PixelFormat.Format32bppArgb);
//            using (Graphics g = Graphics.FromImage(bmpimg))
//            {
//                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
//                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
//                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
//                g.DrawImage(map, 0, 0);
//            }
//            //将二维码插入图片
//            Graphics myGraphic = Graphics.FromImage(bmpimg);
//            //白底
//            myGraphic.FillRectangle(Brushes.White, middleL, middleT, middleW, middleH);
//            myGraphic.DrawImage(logoImg, middleL, middleT, middleW, middleH);

//            //保存成图片
//            bmpimg.Save(imgPath, ImageFormat.Png);

//        }

//        /// <summary>
//        /// 读取二维码
//        /// 读取失败，返回空字符串
//        /// </summary>
//        /// <param name="filename">指定二维码图片位置</param>
//        public static string Read1(string filename)
//        {
//            BarcodeReader reader = new BarcodeReader();
//            reader.Options.CharacterSet = "UTF-8";
//            Bitmap map = new Bitmap(filename);
//            Result result = reader.Decode(map);
//            return result == null ? "" : result.Text;
//        }

//    }
//}
