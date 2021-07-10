using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope
{
    public class StorageUtils
    {
        public static Stream getFile(string fileName)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=sqlvayp7ddoadz6yqi;AccountKey=+i0uP/O2v7C2TW3TD3ZgrFwY+4kQIf/24m7kExuIX0uH0r8DBz9Szp2GvALnXZp53VRUuX/y3Dj7qAqMW2qAgA==;EndpointSuffix=core.windows.net";
            // Name of the share, directory, and file we'll create
            string shareName = "ciclope";
            string dirName = "/";

            // Get a reference to the file
            ShareClient share = new ShareClient(connectionString, shareName);
            ShareDirectoryClient directory = share.GetDirectoryClient(dirName);
            ShareFileClient file = directory.GetFileClient(fileName);

            // Download the file
            ShareFileDownloadInfo download = file.Download();
            return download.Content;
        }


        public static void uploadFile(Stream stream,string filename)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=sqlvayp7ddoadz6yqi;AccountKey=+i0uP/O2v7C2TW3TD3ZgrFwY+4kQIf/24m7kExuIX0uH0r8DBz9Szp2GvALnXZp53VRUuX/y3Dj7qAqMW2qAgA==;EndpointSuffix=core.windows.net";
            // Name of the share, directory, and file we'll create
            string shareName = "ciclope";
            string dirName = "/";

           
            // Get a reference to a share and then create it
            ShareClient share = new ShareClient(connectionString, shareName);
            ShareDirectoryClient directory = share.GetDirectoryClient(dirName);
            ShareFileClient file = directory.GetFileClient(filename);

            file.Create(stream.Length);
            file.UploadRange(
                new HttpRange(0, stream.Length),
                stream);
        }



        





    }
}
