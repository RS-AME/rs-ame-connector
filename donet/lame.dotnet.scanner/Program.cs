using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lame.dotnet.scanner
{

    interface IScan
    {
        bool SetParam(string param);
        bool Load();
        void Scan(string path);
        IScan Clone();
        void ShowVersion();
        void ShowLicense();
        void UnLoad();
    }


    class Scanner : IScan
    {
        private static readonly object locker = new object();
        private VirusLib vdb = null;
        private Lame lame = new Lame();

        public Scanner(VirusLib vdb) 
        {
            this.vdb = vdb;
        }
        public Scanner(Lame lame ,VirusLib vdb ) 
        {
            if (lame == null || vdb == null)
            {
                throw new Exception("Unknown Excepiton");
            }
            this.lame = lame;
            this.vdb = vdb;
        }
        public bool SetParam(string param) 
        {
            return lame.SetParameters(param);
        }
        public bool Load() 
        {
            try
            {
                if (!vdb.lame_open_vdb(null)) return false;
                if (!lame.Load(vdb)) return false;

                return true;
            }
            catch (System.Exception ex)
            {
                vdb.lame_close_vdb();
                lame.Unload();
                Console.WriteLine(ex.Message);
            }

            return false;
        }
        public void Scan(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path)) return;
                if (File.Exists(path))
                {
                    ScanFile(path);
                    return;
                }

                string[] files = Directory.GetFiles(path);
                foreach (string f in files)
                {
                    ScanFile(f);
                }

                string[] dirs = Directory.GetDirectories(path);
                foreach (string d in dirs)
                {
                    Scan(d);
                }


            }
            catch (System.Exception ex)
            {

            }
        }
        public IScan Clone() 
        {
            if (lame == null) return null;

            return new Scanner((Lame)lame.Clone() , vdb);
        }
        private void ScanFile(string file)
        {
            try
            {
                LameScanResult result = lame.ScanFile(file);
                lock (locker) 
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(file);
                    if (result != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("     Infected:" + result.VirusName + " (" + result.EngineID + ")");
                    }
                    Console.WriteLine("");
                }
               
            }
            catch (System.Exception)
            {

            }
        }
        public void ShowVersion()
        {
            try
            {
                LameVesionInfo info = lame.GetVersion();
                if (info == null) return;

                Console.WriteLine("Engine:" + info.EngineVersion);
                Console.WriteLine("VirusDB:" + info.VirusDBVersion);
            }
            catch (System.Exception ex)
            {
            	
            }
        }
        public void ShowLicense() 
        {
            try
            {
                LameLicenceInfo info = lame.GetLicense();
                if (info == null) return;

                Console.WriteLine("Version:" + info.Version);
                Console.WriteLine("Owner:" + info.Owner);
                Console.WriteLine("Date:" + info.Date);
                Console.WriteLine("Authm:" + info.Authm);
            }
            catch (System.Exception ex)
            {

            }
        }
        public void UnLoad() 
        {
            try
            {
                lame.Unload();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

  
    class ScannerEx : IScan
    {
        public static readonly object locker = new object();
        private VirusLib vdb = null;
        private LameWithEvent lame = new LameWithEvent();
        public ScannerEx(VirusLib vdb) 
        {
            this.vdb = vdb;
        }
         public ScannerEx(LameWithEvent lame, VirusLib vdb) 
        {
            if (lame == null || vdb == null)
            {
                throw new Exception("Unknown Excepiton");
            }
            this.lame = lame;
            this.vdb = vdb;
        }

        public bool SetParam(string param)
        {
            return lame.SetParameters(param);
        }
        public bool Load()
        {
            try
            {
                if (!vdb.lame_open_vdb(null)) return false;
                if (!lame.Load(vdb)) return false;
                lame.SetEventHandle(EventHandle);


                //测试
                //LameWithEvent lm = (LameWithEvent)lame.Clone();
                //lame.Unload();
                //lame = lm;

             

                return true;
            }
            catch (System.Exception ex)
            {
                vdb.lame_close_vdb();
                lame.Unload();
                Console.WriteLine(ex.Message);
            }

            return false;
        }
        public IScan Clone()
        {
            if (lame == null) return null;

            return new ScannerEx((LameWithEvent)lame.Clone(), vdb);
        }
        public void Scan(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path)) return;
                if (File.Exists(path))
                {
                    ScanFile(path);
                    return;
                }

                string[] files = Directory.GetFiles(path);
                foreach (string f in files)
                {
                    ScanFile(f);
                }

                string[] dirs = Directory.GetDirectories(path);
                foreach (string d in dirs)
                {
                    Scan(d);
                }


            }
            catch (System.Exception ex)
            {

            }
        }
        private void ScanFile(string file)
        {
            try
            {
                lame.ScanFile(file);
            }
            catch (System.Exception)
            {

            }
        }
        private void ScanMem(string file) 
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(file);
                lame.ScanMem(bytes);
            }
            catch (System.Exception ex)
            {
            	
            }
        }
        public void ShowVersion()
        {
            try
            {
                LameVesionInfo info = lame.GetVersion();
                if (info == null) return;

                Console.WriteLine("Engine:" + info.EngineVersion);
                Console.WriteLine("VirusDB:" + info.VirusDBVersion);
            }
            catch (System.Exception ex)
            {

            }
        }
        public void ShowLicense()
        {
            try
            {
                LameLicenceInfo info = lame.GetLicense();
                if (info == null) return;

                Console.WriteLine("Version:" + info.Version);
                Console.WriteLine("Owner:" + info.Owner);
                Console.WriteLine("Date:" + info.Date);
                Console.WriteLine("Authm:" + info.Authm);
            }
            catch (System.Exception ex)
            {

            }
        }
        public void UnLoad()
        {
            try
            {
                lame.Unload();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void EventHandle(string file, LameScanResult result)
        {
            lock (ScannerEx.locker) 
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(file);
                if (result != null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("     Infected:" + result.VirusName + " (" + result.EngineID + ")");
                }
                Console.WriteLine("");
            }
           
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            
            if (args.Length == 0)
            {
                Console.WriteLine("Invalid Param.");
                Help();
                return;
            }


            VirusLib vdb = null;
            IScan _Scanner = null;
            try
            {
                vdb = new VirusLib();
                if (!vdb.lame_open_vdb(null))
                {
                    Console.WriteLine("Faild to load virus lib.");
                    return;
                }
        
                List<string> path_list = new List<string>();
                List<string> param_list = new List<string>();

                int scan_thread_count = 2;
                int down_thread_count = 1;

                bool bShowVerion = false;
                bool bShowLicense = false;
                bool bShowFileList = false;
                bool bHold = false;
                bool bMd5List = false;
                foreach (string s in args)
                {
                    string vl = s.ToLower();
                    if (s.StartsWith("-"))
                    {
                        if (vl == "-version")
                        {
                            bShowVerion = true;
                        }
                        else if (vl == "-license")
                        {
                            bShowLicense = true;
                        }
                        else if (vl == "-hold")
                        {
                            bHold = true;
                        }
                        else if (vl == "-show-file-list")
                        {
                            bShowFileList = true;
                        }
                        else if (vl == "-md5-list")
                        {
                            bMd5List = true;
                        }
                        else if (s.StartsWith("-workers="))
                        {
                            string[] v1 = s.Split('=');
                            if (v1.Length == 2)
                            {
                                string[] v2 = v1[1].Split(',');
                                if (v2.Length > 0)
                                {
                                    int value = 0;
                                    if (int.TryParse(v2[0], out value)) scan_thread_count = value;

                                    if (v2.Length > 1)
                                    {
                                        if (int.TryParse(v2[1], out value)) down_thread_count = value;
                                    }
                                }
                            
                            }
                        }
                        else
                        {
                            param_list.Add(s.Substring(1));
                        }

                    }
                    else
                    {
                        path_list.Add(s);
                    }

                }


                if (bShowFileList)
                {
                    _Scanner = new ScannerEx(vdb);
                }
                else 
                {
                    _Scanner = new Scanner(vdb);
                }


                if (bShowVerion)
                {
                    _Scanner.ShowVersion();
                }

                if (bShowLicense)
                {
                    _Scanner.ShowLicense();
                }


                foreach (string s in param_list)
                {
                    _Scanner.SetParam(s);
                }

                if (!_Scanner.Load()) return;

                if (bMd5List)
                {
                    TaskScanner task = new TaskScanner(_Scanner, path_list);
                    task.Run(scan_thread_count, down_thread_count);
                }
                else 
                {
                    foreach (string s in path_list)
                    {
                        _Scanner.Scan(s);
                    }
                }


           
                if (bHold)
                {
                    Console.Read();
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (_Scanner != null)
                {
                    _Scanner.UnLoad();
                }

                if (vdb != null)
                {
                    vdb.lame_close_vdb();
                }
            }

           
        }
        static void Help() 
        {
            Console.WriteLine("Usage:\n");
            Console.WriteLine("	  -version	  : show scanner info.");
            Console.WriteLine("	  -license	  : show scanner license.");
            Console.WriteLine("	  -hold		  : hold , press return to exit.");
            Console.WriteLine("	  -show-file-list : show internal files of scan.");
            Console.WriteLine("	  -md5-list  : read md5 from file and download,then scan,default -workers=2,1");
            Console.WriteLine("	  -workers   : specify scan worker count and download worker count.");


            Console.WriteLine("Example:");
            Console.WriteLine("	  : lame.scanner d:\\samples");
            Console.WriteLine("	  : lame.scanner -lame.v1 -hold d:\\samples");
            Console.WriteLine("	  : lame.scanner -md5-list d:\\md5.txt");
            Console.WriteLine("	  : lame.scanner -md5-list -workers=4,1 d:\\md5.txt");
        }
    }
}
