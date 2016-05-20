using Exon.Recab.Domain.SqlServer;
using System;
using System.Configuration;
using System.Diagnostics;


namespace Exon.Recab.Service.Implement.Jobs.MongoBackUp
{
    public class MongoBackUpService
    {
        private readonly string MongoDumpUrl;
        private readonly string MongoRestorUrl;
        private readonly string BackUpFolder;
        private readonly string DataBase;
        private readonly string UserName;
        private readonly string Password;
        private readonly string Port;
        private readonly string Host;

        private readonly SdbContext _sdb;

        public MongoBackUpService()
        {

            AppSettingsReader _settingsReader = new AppSettingsReader();

            _sdb = new SdbContext();

            this.MongoDumpUrl = _settingsReader.GetValue("MongoBack:DumpUrl", typeof(string)).ToString();
            this.MongoRestorUrl = _settingsReader.GetValue("MongoBack:RestorUrl", typeof(string)).ToString();
            this.BackUpFolder = _settingsReader.GetValue("MongoBack:BackUpFolder", typeof(string)).ToString();
            this.DataBase = _settingsReader.GetValue("MongoBack:DataBase", typeof(string)).ToString();
            this.UserName = _settingsReader.GetValue("MongoBack:Username", typeof(string)).ToString();
            this.Password = _settingsReader.GetValue("MongoBack:Password", typeof(string)).ToString();
            this.Port = _settingsReader.GetValue("MongoBack:Port", typeof(string)).ToString();
            this.Host = _settingsReader.GetValue("MongoBack:Host", typeof(string)).ToString();

        }

        public void MongoBack()
        {

            string FileName = DataBase + DateTime.UtcNow.ToFileTime().ToString();

            string query = MongoDumpUrl +
                           " -h " + Host +
                           " --port " + Port +
                           " -u " + UserName +
                           " -p " + Password +
                           " -d " + DataBase +
                           " --archive=" + BackUpFolder + FileName + ".gz" +
                           " --gzip";

            _sdb.MongoBackUps.Add(new Exon.Recab.Domain.Entity.BackUpModole.MongoBackUp
            {
                BackUpDate = DateTime.UtcNow,
                BackUpUrl = BackUpFolder + FileName + ".gz",
                Query = query
            });

            _sdb.SaveChanges();

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C " + query
            };
            process.StartInfo = startInfo;
            process.Start();

            process.WaitForExit();
            process.Close();

        }

        public void MongoRestor()
        {

            //string FileName = DataBase + DateTime.UtcNow.ToFileTime().ToString();

            //string query = MongoDumpUrl +
            //               " -h " + Host +
            //               " --port " + Port +
            //               " -u " + UserName +
            //               " -p " + Password +
            //               " -d " + DataBase +
            //               " -archive=" + BackUpFolder + FileName + ".gz" +
            //               " --gzip";

            //_sdb.MongoBackUps.Add(new Exon.Recab.Domain.Entity.BackUpModole.MongoBackUp
            //{
            //    BackUpDate = DateTime.UtcNow,
            //    BackUpUrl = BackUpFolder + FileName + ".gz",
            //    Query = query
            //});

            //_sdb.SaveChanges();

            //Process.Start("cmd.exe", query);




        }
    }
}
