import os
import sys
import platform
import signal
import ccolor
from lame import *
import ctypes


STD_INPUT_HANDLE = -10
STD_OUTPUT_HANDLE= -11
STD_ERROR_HANDLE = -12

FOREGROUND_BLACK = 0x0
FOREGROUND_BLUE = 0x01 # text color contains blue.
FOREGROUND_GREEN= 0x02 # text color contains green.
FOREGROUND_RED = 0x04 # text color contains red.
FOREGROUND_INTENSITY = 0x08 # text color is intensified.

BACKGROUND_BLUE = 0x10 # background color contains blue.
BACKGROUND_GREEN= 0x20 # background color contains green.
BACKGROUND_RED = 0x40 # background color contains red.
BACKGROUND_INTENSITY = 0x80 # background color is intensified.

class ConsoleColor:
    std_out_handle = ctypes.windll.kernel32.GetStdHandle(STD_OUTPUT_HANDLE)
    
    def set_cmd_color(self, color, handle=std_out_handle):
        bool = ctypes.windll.kernel32.SetConsoleTextAttribute(handle, color)
        return bool
        
    def reset_color(self):
        self.set_cmd_color(FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE)
    
    def set_red_text(self):
        self.set_cmd_color(FOREGROUND_RED | FOREGROUND_INTENSITY)
        
    def set_green_text(self):
        self.set_cmd_color(FOREGROUND_GREEN | FOREGROUND_INTENSITY)

    
    def set_blue_text(self): 
        self.set_cmd_color(FOREGROUND_BLUE | FOREGROUND_INTENSITY)

          
    def set_red_text_with_blue_bg(self):
        self.set_cmd_color(FOREGROUND_RED | FOREGROUND_INTENSITY| BACKGROUND_BLUE | BACKGROUND_INTENSITY)

class LameScanner:
    
    def __init__(self , lpath):
        self._lame = Lame(lpath)
        self._color = ccolor.ConsoleColor()
    
    def Load(self , vdb):

        if self._lame is None:
            return

        if not self._lame.Load(vdb):
            return

        return True

    def SetParam(self , param):
        if param is None:
            return
        if self._lame is None:
            return
        
        self._lame.SetParam(param)

    
    def Scan(self , path_):

        if os.path.isfile(path_):
            self.__ScanFile(path_)
        else:
            self.__ScanDir(path_)

    def Unload(self):
        if self._lame is not None:
            self._lame.Unload()
            self._lame = None


    def ShowLicense(self):
        if self._lame is None:
            return
        _license = self._lame.GetLicense()
        if _license is None:
            return

        print("License:")
        print("     Version: " + _license.version)
        print("     Owner  : " + _license.Owner)
        print("     Date   : " + _license.Data)
        print("     Authm  : " + _license.Authm)

    def ShowVersion(self):

        if self._lame is None:
            return
        _version = self._lame.GetVersion()
        if _version is None:
            return
           
        print("Engine Version    :" + _version.engv)
        print("Viruse Lib Version: " + _version.vdbv)
        

    def __ScanFile(self , fname):

        if not os.path.exists(fname):
            return
        if self._lame is None:
            return
        
        self._color.set_green_text()
        sys.stdout.write(fname)

        _result = self._lame.ScanFile(fname)
        if _result is not None:
            self._color.set_red_text()
            sys.stdout.write(" " + "  Infected: " + _result.vname + " (" + _result.engid+")")
        
        sys.stdout.write('\n')
       

    
    def __ScanDir(self , dir_):

        if not os.path.exists(dir_):
            return
        
        for dirpath,dirnames,filenames in os.walk(dir_):
            for file in filenames:
                fullpath=os.path.join(dirpath,file)
                self.__ScanFile(fullpath)


class LameScanner2:
    
    def __init__(self , lpath):
        self._lame = LameWithFeedback(lpath)
        self._lame.SetCallack(self.__ReportHandle)
        self._color = ccolor.ConsoleColor()
    
    def Load(self , vdb):

        if self._lame is None:
            return

        if not self._lame.Load(vdb):
            return

        return True

    def SetParam(self , param):
        if param is None:
            return
        if self._lame is None:
            return
        
        self._lame.SetParam(param)


    def Unload(self):
        if self._lame is not None:
            self._lame.Unload()
            self._lame = None


    def ShowLicense(self):
        if self._lame is None:
            return
        _license = self._lame.GetLicense()
        if _license is None:
            return

        print("License:")
        print("     Version: " + _license.version)
        print("     Owner  : " + _license.Owner)
        print("     Date   : " + _license.Data)
        print("     Authm  : " + _license.Authm)

    def ShowVersion(self):

        if self._lame is None:
            return
        _version = self._lame.GetVersion()
        if _version is None:
            return
           
        print("Engine Version    :" + _version.engv)
        print("Viruse Lib Version: " + _version.vdbv)
        

    def __ReportHandle(self , fname , result):
        
        self._color.set_green_text()
        sys.stdout.write(fname)
        
        if result is not None:
            self._color.set_red_text()
            sys.stdout.write(" " + "  Infected: " +result.vname + " (" + result.engid+")")
 
        sys.stdout.write('\n')
        

    
    def Scan(self , path_):

        if os.path.isfile(path_):
            self.__ScanFile(path_)
        else:
            self.__ScanDir(path_)
        
        

    def __ScanFile(self , fname):

        if not os.path.exists(fname):
            return
        if self._lame is None:
            return
        
        self._lame.ScanFile(fname)
       

    
    def __ScanDir(self , dir_):

        if not os.path.exists(dir_):
            return
        
        for dirpath,dirnames,filenames in os.walk(dir_):
            for file in filenames:
                fullpath=os.path.join(dirpath,file)
                self.__ScanFile(fullpath)



def exit(signum, frame):  
    sys.exit()  
  

def main(argv):
    _lame_path = "E:\\work\\svn\\rame\\build\\release"
    _param_lst = []
    _scan_path_lst = []

    bShowVersion = False
    bShowLicense = False
    bShowFileList = False

    for _param in argv:
        if _param.startswith('-'):
            if _param.lower() == "-version":
                bShowVersion = True
            elif _param.lower() == "-license":
                bShowLicense = True
            elif _param.lower() == "-show-file-list":
                bShowFileList = True
            else:
                _param_lst.append(_param[1:])

        else:
            _scan_path_lst.append(_param)

    _dbf = VirusDb(_lame_path)
    if not _dbf.OpenVdb(None):
        print("Faild to openvdb.")
        return
    
    
    _scanner = None
    _lame = None
    if bShowFileList:
        _lame = LameScanner2(_lame_path)
    else:
        _lame = LameScanner(_lame_path)

    for _param in _param_lst:
        _lame.SetParam(_param)

    if not _lame.Load(_dbf):
        _dbf.CloseVdb()
        print("Faild to load lame.")
        return

    if bShowLicense:
        _lame.ShowLicense()

            

    if bShowVersion:
        _lame.ShowVersion()

    for _path in _scan_path_lst:
        _lame.Scan(_path)


    _lame.Unload()
    _dbf.CloseVdb()





if  __name__ == "__main__":
    signal.signal(signal.SIGINT, exit)  
    signal.signal(signal.SIGTERM, exit)
    main(sys.argv[1:])
