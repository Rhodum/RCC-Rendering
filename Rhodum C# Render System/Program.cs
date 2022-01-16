using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Rhodum_Render_System;
using Newtonsoft.Json;
using System.Text;

namespace Rhodum_Render_System
{
    class Program
    {
        public static Image ResizeImage(Image image, Size size, bool preserveAspectRatio = true)
        {
            checked
            {
                int width2;
                int height2;
                if (preserveAspectRatio)
                {
                    int width = image.Width;
                    int height = image.Height;
                    float num = (float)size.Width / (float)width;
                    float num2 = (float)size.Height / (float)height;
                    float num3 = (num2 < num) ? num2 : num;
                    width2 = (int)Math.Round((double)(unchecked((float)width * num3)));
                    height2 = (int)Math.Round((double)(unchecked((float)height * num3)));
                }
                else
                {
                    width2 = size.Width;
                    height2 = size.Height;
                }
                Image image2 = new Bitmap(width2, height2);
                Graphics graphics = Graphics.FromImage(image2);
                try
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(image, 0, 0, width2, height2);
                }
                finally
                {
                    bool flag = graphics != null;
                    if (flag)
                    {
                        ((IDisposable)graphics).Dispose();
                    }
                }
                return image2;
            }
        }
        private Bitmap Screenshot()
        {
            Bitmap bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            Graphics g = Graphics.FromImage(bmpScreenshot);

            g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);

            return bmpScreenshot;
        }

        [DllImport("user32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern bool IsIconic(int hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int FindWindow([MarshalAs(UnmanagedType.VBByRefStr)] ref string lpClassName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpWindowName);
        public static void FocusWindow(string strWindowCaption, string strClassName)
        {
            int num = Program.FindWindow(ref strClassName, ref strWindowCaption);
            bool flag = num > 0;
            if (flag)
            {
                Program.SetForegroundWindow((IntPtr)num);
                flag = Program.IsIconic(num);
                if (flag)
                {
                    Program.ShowWindow((IntPtr)num, Program.ShowWindowEnum.Restore);
                }
                else
                {
                    Program.ShowWindow((IntPtr)num, Program.ShowWindowEnum.Show);
                }
            }
        }
        private enum ShowWindowEnum
        {
            Hide,
            ShowNormal,
            ShowMinimized,
            ShowMaximized,
            Maximize = 3,
            ShowNormalNoActivate,
            Show,
            Minimize,
            ShowMinNoActivate,
            ShowNoActivate,
            Restore,
            ShowDefault,
            ForceMinimized
        }

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, Program.ShowWindowEnum flags);

        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hwnd);

        public static object BringMainWindowToFront(string processName)
        {
            Process process = Process.GetProcessesByName(processName).FirstOrDefault<Process>();
            bool flag = process != null;
            object result;
            if (flag)
            {
                bool flag2 = process.MainWindowHandle == IntPtr.Zero;
                if (flag2)
                {
                    Program.ShowWindow(process.Handle, Program.ShowWindowEnum.Restore);
                }
                Program.SetForegroundWindow(process.MainWindowHandle);
                Console.WriteLine("Focused window");
                result = true;
            }
            else
            {
                Console.WriteLine("Window not found. Retrying..");
                result = false;
            }
            return result;
        }

        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern long SetCursorPos(int X, int Y);

        [DllImport("user32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        private static void Crap()
        {
            Program.SetCursorPos(0, 0);
            Program.SetCursorPos(242, 30);
            Program.mouse_event(2, 0, 0, 0, 1);
            Program.mouse_event(4, 0, 0, 0, 1);
            Thread.Sleep(500);
            Program.SetCursorPos(303, 200);
            Program.mouse_event(2, 0, 0, 0, 1);
            Program.mouse_event(4, 0, 0, 0, 1);
            Thread.Sleep(500);
            Program.SetCursorPos(0, 0);
        }

        public static void ImageModelServerView()
        {
            Program.Crap();
            Thread.Sleep(1000);
            Program.SetCursorPos(0, 0);
            Program.Crap();
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        protected static string GetBase64StringForImage(string imgPath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }



        public static string Postimage(int uid, string rendtype, string base64image)
        {
            WebClient client = new WebClient();
            string uploadurl = "http://labs.rhodum.xyz/img/uplimg.php?apikey=jv9BkLv8TFfcs67q&uid=" + uid + "&typeofasset=" + rendtype;
            client.Headers.Add("Content-Type", "application/octet-stream");
            using (Stream fileStream = File.OpenRead(uid +".txt"))
            using (Stream requestStream = client.OpenWrite(new Uri(uploadurl), "POST"))
            {
                fileStream.CopyTo(requestStream);
            }

            client.DownloadString("http://labs.rhodum.xyz/img/doFinish.php?renderid="+ uid + "&type=" + rendtype + "&apiKey=DebianIsAHotFuckingChickAndRaymonfNeedsToDie342908590498590485&version=1");
            File.Delete(uid + ".txt");
            return "posted";
        }


        public static string dorender(int uid, string rendtype)
        {
            Console.WriteLine("Doing render for " + rendtype + " with id " + uid);
            string procPath = "";
            WebClient wClient = new WebClient();
            procPath = Directory.GetCurrentDirectory() + "\\" + "Render.exe";
            ProcessStartInfo RenderExecutable = new ProcessStartInfo(procPath);

            // Generate arguments for launch
            Process mainProcess;
            if (rendtype == "character")
            {
                RenderExecutable.Arguments = "-script \"dofile('http://labs.rhodum.xyz/img/character.php?id=" + uid + "')\"";
            }
            else if (rendtype == "shirts")
            {
                RenderExecutable.Arguments = "-script \"dofile('http://labs.rhodum.xyz/img/shirts.php?id=" + uid + "')\"";
            }
            else if (rendtype == "pants")
            {
                RenderExecutable.Arguments = "-script \"dofile('http://labs.rhodum.xyz/img/pants.php?id=" + uid + "')\"";
            }
            else if (rendtype == "tshirts")
            {
                RenderExecutable.Arguments = "-script \"dofile('http://labs.rhodum.xyz/img/tshirts.php?id=" + uid + "')\"";
            }
            else
            {
                RenderExecutable.Arguments = "-no3d";
            }

            // start the render exectuable with the given arguments
            mainProcess = Process.Start(RenderExecutable);


            // make sure its in image server view
            Thread.Sleep(10000);
            Console.WriteLine("Sleeping 10 seconds...");
            //Program.FocusWindow("Roblox - [Place1]", null);
            Program.BringMainWindowToFront("Render");
            Program.ImageModelServerView();

            // screenshot taking and saving
            Console.WriteLine("Saving...");

            Rectangle rectangle = default(Rectangle);
            Point upperLeftSource = new Point(rectangle.Left + 560, rectangle.Top + 123);
            Bitmap bitmap = new Bitmap(805, 805);
            Graphics graphics = Graphics.FromImage(bitmap);
            Graphics graphics5 = graphics;
            graphics5.CopyFromScreen(upperLeftSource, Point.Empty, bitmap.Size);
            Size size = new Size(310, 290);
            Image newimage = Program.ResizeImage(bitmap, size, true);
            newimage.Save(uid + ".png");

            string imgpath = Directory.GetCurrentDirectory() + "\\" + uid + ".png";
            string encodedthumb = GetBase64StringForImage(imgpath);

            //write the base64 image to text file and delete the original image
            File.WriteAllText(uid + ".txt", encodedthumb);
            File.Delete(uid + ".png");

            // screenshot saved, kill the renderer
            mainProcess.Kill();

            // upload the image to the website
            //string uploadurl = "http://labs.rhodum.xyz/img/uplimg.php?apikey=jv9BkLv8TFfcs67q&uid="+ uid + "&typeofasset=" + rendtype;
            Console.WriteLine("Uploading...");
            Postimage(uid, rendtype, encodedthumb);
            Console.WriteLine("Finished Rendering!");
            return "Done!";

        }
        static void Main(string[] args)
        {
            try
            {
                WebClient wClient = new WebClient();
                while (true)
                {
                    if (wClient.DownloadString("http://labs.rhodum.xyz/img/getList.php") != "no-render")
                    {
                        dynamic quejson = JsonConvert.DeserializeObject(wClient.DownloadString("http://labs.rhodum.xyz/img/getList.php"));
                        dorender(Convert.ToInt32(quejson.userid), Convert.ToString(quejson.type));
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }

            }
            catch (Exception)
            {
                Console.ReadKey();
                MessageBox.Show("some shit went wrong");
            }
        }
    }
}
