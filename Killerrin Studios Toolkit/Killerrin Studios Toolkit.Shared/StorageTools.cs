using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Windows.Storage;
using System.Threading.Tasks;
using System.IO;

using KillerrinStudiosToolkit.Enumerators;

namespace KillerrinStudiosToolkit
{
    public static class StorageTools
    {
        public static bool isSavingComplete = true; // If True, it is safe to exit. If not, loop continuously

        public static class StorageConsts
        {
            public static string LocalStorageFolderPrefix = "ms-appdata:///local/";
            public static string VisualStudioSolutionFilePrefix = "ms-appx://";

            public static string TileFolder = "TileImages";
            public static string AssetsFolder = "Assets";
        }

        public static async Task<bool> DoesFileExist(string fileName)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();

            foreach (StorageFile file in files) {
                if (fileName == file.Name) { return true; }
            }

            return false;
        }

        public static async Task<bool> DoesFolderExist(string fileName)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            IReadOnlyList<StorageFolder> files = await folder.GetFoldersAsync();

            foreach (StorageFolder file in files) {
                if (fileName == file.Name) { return true; }
            }

            return false;
        }

        #region Simple Tools
        public static async Task<bool> CreateFolder(string folderName, CreationCollisionOption collisionSettings = CreationCollisionOption.OpenIfExists)
        {
            var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(folderName, collisionSettings);

            return true;
        }

        public static async Task<bool> SaveToStorage(string fileName, string content)
        {
            if (Consts.isApplicationClosing) return false;

            isSavingComplete = false;

            try {
                Debug.WriteLine("SaveToStorage(): Saving to Storage");
                byte[] data = Encoding.UTF8.GetBytes(content);

                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                using (Stream s = await file.OpenStreamForWriteAsync()) {
                    await s.WriteAsync(data, 0, data.Length);
                }
                Debug.WriteLine("Storage Saved: " + fileName);

                isSavingComplete = true;
                return true;
            }
            catch (Exception) { isSavingComplete = true; return false; }
        }
        public static async Task<bool> SaveFileFromServer(Uri serverURI, string fileName)
        {
            isSavingComplete = false;

            try {
                Debug.WriteLine("Opening Client");
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

                Debug.WriteLine("Grabbing File");
                byte[] result = await client.GetByteArrayAsync(serverURI);

                Debug.WriteLine("Writing File");
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                using (Stream s = await file.OpenStreamForWriteAsync()) {
                    await s.WriteAsync(result, 0, result.Length);
                }

                Debug.WriteLine("Storage Saved: " + fileName);

                isSavingComplete = true;
                return true;
            }
            catch (Exception) { isSavingComplete = true; return false; }
        }

        public static async Task<StorageFile> LoadStorageFileFromStorage(string fileName)
        {
            Debug.WriteLine("LoadStorageFileFromStorage(" + fileName + "): entering");

            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.GetFileAsync(fileName);
            return file;
        }
        public static async Task<string> LoadFileFromStorage(string fileName)
        {
            StorageFile file = await LoadStorageFileFromStorage(fileName);

            byte[] data;
            using (Stream s = await file.OpenStreamForReadAsync()) {
                data = new byte[s.Length];
                await s.ReadAsync(data, 0, (int)s.Length);
            }
            Debug.WriteLine(fileName + " Loaded");
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }


        public static async Task<StorageFile> LoadStorageFileFromPackage(string folderName, string fileName)
        {
            Debug.WriteLine("LoadStorageFileFromPackage(" + fileName + "): entering");
            StorageFolder installFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;

            StorageFile file;
            if (!string.IsNullOrEmpty(folderName)) // folderName != null
            {
                StorageFolder subFolder = await installFolder.GetFolderAsync(folderName);
                file = await subFolder.GetFileAsync(fileName);
            }
            else {
                file = await installFolder.GetFileAsync(fileName);
            }
            return file;
        }
        public static async Task<string> LoadPackagedFile(string folderName, string fileName)
        {
            StorageFile file = await LoadStorageFileFromPackage(folderName, fileName);

            byte[] data;
            using (Stream s = await file.OpenStreamForReadAsync()) {
                data = new byte[s.Length];
                await s.ReadAsync(data, 0, (int)s.Length);
            }
            Debug.WriteLine(fileName + " Loaded");
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }
        #endregion

        #region Deletion Tools
        public static async Task<bool> DeleteFolder(string folderName, StorageDeleteOption deleteOptions = StorageDeleteOption.PermanentDelete)
        {
            Debug.WriteLine("DeleteFolder(" + folderName + ")");

            StorageFolder folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(folderName);
            await folder.DeleteAsync(deleteOptions);

            return true;
        }
        public static async Task<bool> DeleteFile(string fileName, StorageDeleteOption deleteOptions = StorageDeleteOption.PermanentDelete)
        {
            Debug.WriteLine("DeleteFile(" + fileName + ")");

            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            await file.DeleteAsync(deleteOptions);

            return true;
        }
        #endregion
    }
}
