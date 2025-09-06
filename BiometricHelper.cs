using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace StudentManagementSystem
{
    public static class BiometricHelper
    {
        /// <summary>
        /// Saves an image and computes its hash.
        /// This method is now separated from the UI logic.
        /// </summary>
        public static bool SaveAndHashImage(string userId, Image<Bgr, byte> imageToSave, out string savedImagePath, out string savedImageHash)
        {
            savedImagePath = "";
            savedImageHash = "";

            if (imageToSave == null) return false;

            try
            {
                Directory.CreateDirectory("biometrics");
                string fileName = $"{userId}_{DateTime.Now.Ticks}.jpg";
                savedImagePath = Path.Combine("biometrics", fileName);

                imageToSave.Save(savedImagePath);

                using (var ms = new MemoryStream())
                {
                    imageToSave.AsBitmap().Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    using (var sha = SHA256.Create())
                    {
                        savedImageHash = BitConverter.ToString(sha.ComputeHash(ms.ToArray())).Replace("-", "");
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Verifies a live image against registered images for a user.
        /// </summary>
        public static bool VerifyFace(string userId, Image<Bgr, byte> liveImage, out double confidence)
        {
            confidence = 9999;
            if (liveImage == null) return false;

            try
            {
                string[] files = Directory.GetFiles("biometrics", $"{userId}_*.jpg");
                if (files.Length == 0) return false; // No registered face

                string registeredImagePath = files[0];
                using (var registeredImage = new Image<Gray, byte>(registeredImagePath))
                using (var testImage = liveImage.Convert<Gray, byte>())
                {
                    var recognizer = new LBPHFaceRecognizer(1, 8, 8, 8, 100);

                    using (var mats = new VectorOfMat())
                    using (var labels = new VectorOfInt())
                    {
                        mats.Push(registeredImage.Mat);
                        labels.Push(new int[] { 1 });
                        recognizer.Train(mats, labels);
                    }

                    var result = recognizer.Predict(testImage);
                    confidence = result.Distance;

                    // Lower confidence is a better match. Threshold can be adjusted.
                    return (result.Label == 1) && (confidence < 70);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}