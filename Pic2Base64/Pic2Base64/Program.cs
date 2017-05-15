using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Pic2Base64
{
    class Program
    {
        private static readonly Dictionary<string, string> _supportedExtensions = new Dictionary<string, string>()
        {
            { ".jpg", "image/jpeg" },
            { ".png", "image/png" },
            { ".gif", "image/gif" },
        };

        [STAThreadAttribute]
        static void Main(string[] args)
        {
            if (args is null || args.Count() != 1)
            {
                Console.WriteLine("Invalid parameter.");
            }
            else if (!File.Exists(args[0]))
            {
                Console.WriteLine(string.Format("Picture '{0}' not found.", args[0]));
            }
            else
            {
                var extension = Path.GetExtension(args[0]);

                if (!_supportedExtensions.ContainsKey(extension))
                {
                    Console.WriteLine(string.Format("Unsupported image format {0}.", extension));
                }
                else
                {
                    using (var image = Image.FromFile(args[0]))
                    {
                        using (var m = new MemoryStream())
                        {
                            image.Save(m, image.RawFormat);
                            var imageBytes = m.ToArray();

                            var base64String = string.Format("data:{0};base64,{1}", _supportedExtensions[extension], Convert.ToBase64String(imageBytes));
                            Console.Write(base64String);
                            Clipboard.SetText(base64String);
                            Console.WriteLine("Text has been copied to clipboard.");
                        }
                    }
                }
                
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
