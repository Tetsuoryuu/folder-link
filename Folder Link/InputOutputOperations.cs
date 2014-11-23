using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Folder_Link
{
    class InputOutputOperations
    {
        public static void OpenFile(string path)
        {
            System.Diagnostics.Process.Start(path);
        }

        public static void OpenLocation(string path)
        {
            string argument = @"/select, " + path;
            System.Diagnostics.Process.Start("explorer.exe", argument);
        }

        public static bool Rename(string oldName, string newName)
        {
            bool success = true;
            try
            {
                File.Move(oldName, newName);
            }
            catch
            {
                success = false;
            }
            return success;
        }
        
        public static bool Delete(string path)
        {
            bool success = true;
            try
            {
                File.Delete(path);
            }
            catch
            {
                success = false;
            }
            return success;
        }

        public static bool Cut(string path)
        {
            //TODO: FileWatcher in case the file is moved from its location 

            bool success = true;
            try
            {

                byte[] moveEffect = new byte[] { 2, 0, 0, 0 };
                MemoryStream dropEffect = new MemoryStream();
                dropEffect.Write(moveEffect, 0, moveEffect.Length);

                DataObject data = new DataObject();
                data.SetFileDropList(new StringCollection() { path });
                data.SetData("Preferred DropEffect", dropEffect);

                Clipboard.Clear();
                Clipboard.SetDataObject(data, true);

            }
            catch 
            { 
                success = false; 
            }
            return success;
        }

        public static bool Copy(string path)
        {
            bool success = true;

            try
            {
                Clipboard.SetFileDropList(new StringCollection() { path });
            }
            catch
            {
                success = false;
            }

            return success;
        }


    }
}
