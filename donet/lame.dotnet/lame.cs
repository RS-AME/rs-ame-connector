using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace lame.dotnet
{
    public enum TreatResult
    {
        TreatFailFix = -1,
        TreatOkDeleted = 1,
        TreatOkFixed = 2,
    };


    public enum VirusType
    {
        Trait = 0,
        Hidden = 1,
        HiddenFU = 2,
        Complier = 3,
        Packer = 4,
        Format = 5,		// 文件格式
        AppType = 6,
        Trusted = 9,

        Malware = 0x10,

        Trojan,
        Backdoor,
        Worm,
        Rootkit,
        Exploit,
        HackTool,
        Adware,
        Stealer,
        Spammer,
        Spyware,
        Virus,
        Joke,
        Junk,
        PUA,
        Downloader,
        Dropper,
        /// 2015-12-21
        Ransom,
        Hoax,
        Riskware,
        // 2016-6-7
        Unwanted,
        Monetizer,				//	套现

        MobileBase = 0xC0,
        // Mobile
        //	摘自《移动互联网恶意代码描述规范》
        Payment = MobileBase,	//	恶意扣费
        Privacy,				//	隐私窃取
        Remote, 				//	远程控制
        Spread, 				//	恶意传播
        Expense, 				//	资费消耗
        System, 				//	系统破坏
        Fraud, 				//	诱骗欺诈
        Rogue,					//	流氓行为

        Attention = 0xFE,		// 注意!
        TypeMax = 0x100,
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ScanResult_
    {
        public VirusType vtype;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
        public string engid;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
        public string vname;
        public int vid32;
        public long vid40;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
        public string hitag;
        public TreatResult treat;
    };

     [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
     public struct LameLicenceInfo_
     {
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
	    public string			Version;

        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 128)]
        public string Owner;

        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Date;
         [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
        public string Authm;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 2048)]
         public  string Data;
    };

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
     public struct LameVesionInfo_ 
     {
         [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 33)]
         public string EngineVersion;

         [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 33)]
         public string VirusDBVersion;
     }


     public class LameLicenceInfo
     {
         public string Version { set; get; }
         public string Owner { set; get; }
         public string Date { set; get; }
         public string Authm { set; get; }
         public string Data { set; get; }
     };


     public class LameVesionInfo
     {
         public string EngineVersion { set; get; }
         public string VirusDBVersion { set; get; }

     }


    public class LameScanResult
    {
        public VirusType VirusType { set; get; }
        public string EngineID { set; get; }
        public string VirusName { set; get; }
        public int VirusID32 { set; get; }
        public long VirusID40 { set; get; }
        public string HitTag { set; get; }
        public TreatResult TreatResult { set; get; }

    }



    public delegate void ScanInternalFileEvent_(string fname, IntPtr result, IntPtr zero);

    class LameUtity 
    {
        [System.Runtime.InteropServices.DllImportAttribute("lame.dll", EntryPoint = "lame_create", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern System.IntPtr lame_create_(System.IntPtr vdb);

        [System.Runtime.InteropServices.DllImportAttribute("lame.dll", EntryPoint = "lame_param_set", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int lame_param_set_(System.IntPtr pEngine, string pFileName);

        [System.Runtime.InteropServices.DllImportAttribute("lame.dll", EntryPoint = "lame_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int lame_init_(System.IntPtr pEngine);

        [System.Runtime.InteropServices.DllImportAttribute("lame.dll", EntryPoint = "lame_scan_file", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int lame_scan_file_(System.IntPtr pEngine, string pFileName, ref ScanResult_ result);

        [System.Runtime.InteropServices.DllImportAttribute("lame.dll", EntryPoint = "lame_scan_file_with_callback", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int lame_scan_file_with_callback_(System.IntPtr pEngine, string pFileName, ScanInternalFileEvent_ cb, System.IntPtr user_data);

        [System.Runtime.InteropServices.DllImportAttribute("lame.dll", EntryPoint = "lame_scan_mem", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int lame_scan_mem_(System.IntPtr pEngine, byte[] data, uint uSize, ref ScanResult_ result);

        [System.Runtime.InteropServices.DllImportAttribute("lame.dll", EntryPoint = "lame_scan_mem_with_callback", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int lame_scan_mem_with_callback_(System.IntPtr pEngine, byte[] data, uint uSize, ScanInternalFileEvent_ cb, System.IntPtr user_data);

        [System.Runtime.InteropServices.DllImportAttribute("lame.dll", EntryPoint = "lame_destroy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void lame_destroy_(System.IntPtr pEngine);

        [System.Runtime.InteropServices.DllImportAttribute("lame.dll", EntryPoint = "lame_get_licence_info", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int lame_get_licence_info_(ref LameLicenceInfo_ license);

        [System.Runtime.InteropServices.DllImportAttribute("lame.dll", EntryPoint = "lame_get_version", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int lame_get_version_(ref LameVesionInfo_ ver);
    }

    public class LameBase 
    {

        protected List<string> params_lst = new List<string>();

        protected IntPtr pEngine = IntPtr.Zero;

        protected VirusLib _vlib = null;

        public bool Load(VirusLib lib)
        {
            try
            {
                if (lib == null) throw new Exception("Invalid Param.");

                if (lib.vdb_handle == IntPtr.Zero) throw new Exception("Invalid VirusLib.");

                pEngine = LameUtity.lame_create_(lib.vdb_handle);
                if (pEngine == IntPtr.Zero) throw new Exception("Faild to create lame.");

                foreach (string s in params_lst)
                {
                    LameUtity.lame_param_set_(pEngine, s);
                }

                if (LameUtity.lame_init_(pEngine) < 0)
                {
                    LameUtity.lame_destroy_(pEngine);
                    pEngine = IntPtr.Zero;
                    throw new Exception("Faild to init lame.");
                }

                _vlib = lib;
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public bool SetParameters(string param)
        {
            if (string.IsNullOrEmpty(param)) return false;
            params_lst.Add(param);
            return true;
        }

        public void Unload()
        {
            try
            {
                if (pEngine == IntPtr.Zero) return;
                LameUtity.lame_destroy_(pEngine);
                pEngine = IntPtr.Zero;
            }
            catch (Exception)
            {
            }

        }

        public LameLicenceInfo GetLicense()
        {

            try
            {
                LameLicenceInfo_ lame_info_ = new LameLicenceInfo_();

                if (LameUtity.lame_get_licence_info_(ref lame_info_) < 0) return null;

                LameLicenceInfo lame_info = new LameLicenceInfo();
                lame_info.Authm = lame_info_.Authm;
                lame_info.Data = lame_info_.Data;
                lame_info.Date = lame_info_.Date;
                lame_info.Owner = lame_info_.Owner;
                lame_info.Version = lame_info_.Version;

                return lame_info;
            }
            catch (Exception e)
            {
                //throw e;
            }

            return null;
        }

        public LameVesionInfo GetVersion()
        {
            try
            {
                LameVesionInfo_ ver_ = new LameVesionInfo_();
                if (LameUtity.lame_get_version_(ref ver_) < 0) return null;

                LameVesionInfo ver = new LameVesionInfo();
                ver.EngineVersion = ver_.EngineVersion;
                ver.VirusDBVersion = ver_.VirusDBVersion;
                return ver;
            }
            catch (System.Exception)
            {

            }

            return null;
        }
    }

    public class Lame : LameBase
    {
        public LameScanResult ScanFile(string sFile)
        {
            try
            {
                if (string.IsNullOrEmpty(sFile)) throw new Exception("Invalid param");
                if (pEngine == IntPtr.Zero) throw new Exception("Invalid lame");


                ScanResult_ _result = new ScanResult_();
                if (LameUtity.lame_scan_file_(pEngine, sFile, ref _result) < 0) return null;

                LameScanResult result = new LameScanResult();
                result.EngineID = _result.engid;
                result.VirusName = _result.vname;
                result.HitTag = _result.hitag;
                result.VirusID32 = _result.vid32;
                result.VirusID40 = _result.vid40;
                result.VirusType = _result.vtype;
                result.TreatResult = _result.treat;

                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public LameScanResult ScanMem(byte[] bytes)
        {
            try
            {
                if (bytes == null || bytes.Length == 0 ) throw new Exception("Invalid param");

                if (pEngine == IntPtr.Zero) throw new Exception("Invalid lame");

                ScanResult_ _result = new ScanResult_();
                if (LameUtity.lame_scan_mem_(pEngine, bytes, (uint)bytes.Length, ref _result) < 0) return null;

                LameScanResult result = new LameScanResult();

                result.EngineID = _result.engid;
                result.VirusName = _result.vname;
                result.HitTag = _result.hitag;
                result.VirusID32 = _result.vid32;
                result.VirusID40 = _result.vid40;
                result.VirusType = _result.vtype;
                result.TreatResult = _result.treat;

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public object Clone()
        {
            Lame _lame = null;
            try
            {
                _lame = new Lame();
                foreach (string s in params_lst)
                {
                    _lame.SetParameters(s);
                }

                if (!_lame.Load(_vlib))
                {
                    _lame.Unload();
                    return null;
                }

                return _lame;
            }
            catch (System.Exception)
            {

            }

            return null;
        }
    }

    public delegate void ScanInternalFileEvent(string fname, LameScanResult result);
    public class LameWithEvent : LameBase
    {
        private ScanInternalFileEvent _handle_file = null;
        private ScanInternalFileEvent_ al = null;
        public void SetEventHandle(ScanInternalFileEvent handle) 
        {
            _handle_file = handle;
            al = new ScanInternalFileEvent_(ScanInternalFile);
        }

        public void ScanFile(string sFile)
        {
            try
            {
                if (string.IsNullOrEmpty(sFile)) throw new Exception("Invalid param");
                if (pEngine == IntPtr.Zero) throw new Exception("Invalid lame");
                if (al == null) throw new Exception("Invalid event handle.");
                if (LameUtity.lame_scan_file_with_callback_(pEngine, sFile, al, IntPtr.Zero) < 0) return;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ScanMem(byte[] bytes)
        {
            try
            {
                if (bytes == null || bytes.Length == 0) throw new Exception("Invalid param");
                if (al == null) throw new Exception("Invalid event handle.");
                if (pEngine == IntPtr.Zero) throw new Exception("Invalid lame");

                if (LameUtity.lame_scan_mem_with_callback_(pEngine, bytes , (uint)bytes.Length, al, IntPtr.Zero) < 0) return;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void ScanInternalFile(string fname, IntPtr resp, IntPtr zero)
        {
            LameScanResult result = null;
            if (_handle_file == null) return;

            if (resp != IntPtr.Zero) { result = FetchResult(resp); }

            _handle_file(fname, result);
        }

        public object Clone()
        {
            LameWithEvent _lame = null;
            try
            {
                _lame = new LameWithEvent();
                foreach (string s in params_lst)
                {
                    _lame.SetParameters(s);
                }

                if (!_lame.Load(_vlib))
                {
                    _lame.Unload();
                    return null;
                }

                _lame.SetEventHandle(_handle_file);

                return _lame;
            }
            catch (System.Exception)
            {

            }

            return null;
        }
        private LameScanResult FetchResult(IntPtr resp) 
        {
            try
            {
                ScanResult_ _result = (ScanResult_)Marshal.PtrToStructure(resp, typeof(ScanResult_));


                LameScanResult result = new LameScanResult();
                result.EngineID = _result.engid;
                result.VirusName = _result.vname;
                result.HitTag = _result.hitag;
                result.VirusID32 = _result.vid32;
                result.VirusID40 = _result.vid40;
                result.VirusType = _result.vtype;
                result.TreatResult = _result.treat;

                return result;
            }
            catch (System.Exception)
            {
            	
            }

            return null;
        }

    }

}
