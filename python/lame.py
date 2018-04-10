import os
import sys
from ctypes import *
import platform
import signal
import ccolor


class LameScanResult(Structure):
    _fields_ = [
        ("mkclass", c_int),
        ("engid", c_char  * 32),
        ("vname", c_char  * 256),
        ("vid32", c_int),
        ("vid40", c_int),
        ("hitag", c_char  * 256),
        ("treat", c_int)
        ]

class LameVersionInfo(Structure):
    _fields_ = [
        ("engv" , c_char * 33),
        ("vdbv" , c_char * 33)
    ]


class LameLicenseInfo(Structure):
    _field = [
        ("Version" , c_char * 16),
        ("Owner"   , c_char * 128),
        ("Date"    , c_char * 64),
        ("Authm"   , c_char * 32),
        ("Data"    , c_char * 2048),
    ]

class LameUtity:

    @staticmethod
    def GetFullLameName(lpath):
        if platform.system() == "Windows":
            os.environ['path'] = os.environ['path'] + ';' + lpath
            lamepath = lpath + '\\lame.dll'
        else:
            lamepath = lpath + '/liblame.so'
        
        return lamepath



class VirusDb:
 
    def __init__(self , lpath):
        _lame_path = LameUtity.GetFullLameName(lpath)
        if not os.path.exists(_lame_path):
            raise Exception("File is not exists.")
        self._lame_dll_handle = CDLL(_lame_path)

        if self._lame_dll_handle is None:
            raise Exception("Faild to load lame library.")

        
    def OpenVdb(self,vdbf):
        if self._lame_dll_handle is None:
            return False

        self._lame_lib_handle = self._lame_dll_handle.lame_open_vdb(vdbf)
        if self._lame_lib_handle is None:
            return False

        return True 
        
        
    def CloseVdb(self):
        if self._lame_dll_handle is None:
            return
        
        if self._lame_lib_handle is None:
            return

        self._lame_dll_handle.lame_close_vdb(self._lame_lib_handle)
        self._lame_lib_handle = None

    def GetVbdHandle(self):
        return self._lame_lib_handle



class LameBase:

    def __init__(self , lpath):
        self._lame_path = LameUtity.GetFullLameName(lpath)
        if not os.path.exists(self._lame_path):
            raise Exception("File is not exists.")
        self._lame_dll_handle = CDLL(self._lame_path)

        if self._lame_dll_handle is None:
            raise Exception("Faild to load lame library.")
        
        self._lame_path = lpath
        self._lame_engine_handle = None
        self._params = []


        
    def SetParam(self , param):
        if self._lame_dll_handle is None:
            return

        if param is None:
            return
        
        self._params.append(param)

        if self._lame_engine_handle is not None:
            self._lame_dll_handle.lame_param_set(self._lame_engine_handle , param)
        

    
    def Load(self, vdb_object):
        self._vbd_handle = vdb_object.GetVbdHandle()
        if self._vbd_handle is None:
            return False

        self._lame_engine_handle = self._lame_dll_handle.lame_create(self._vbd_handle)
        if self._lame_engine_handle is None:
            return False

        for p in self._params:
            self._lame_dll_handle.lame_param_set(self._lame_engine_handle , p)

        ret = self._lame_dll_handle.lame_init(self._lame_engine_handle)   

        if ret < 0:
            self._lame_dll_handle.lame_destroy(self._lame_engine_handle)
            return False

        return True


    def GetVersion(self):

        if self._lame_dll_handle is None:
            return None
        
        _version = LameVersionInfo()
        ret = self._lame_dll_handle.lame_get_version(byref(_version))
        if ret < 0:
            return None

        return _version


    def GetLicense(self):

        if self._lame_dll_handle is None:
            return None
        
        _License = LameLicenseInfo()
        ret = self._lame_dll_handle.lame_get_licence_info(byref(_License))
        if ret < 0:
            return None

        return _License

        


    def Unload(self):
        if self._lame_dll_handle is None:
            return
        
        if self._lame_engine_handle is None:
            return
        
        self._lame_dll_handle.lame_destroy(self._lame_engine_handle)
    

    


       



class Lame(LameBase):

    def __init__(self , lpath):
        LameBase.__init__(self , lpath)

    def ScanFile(self,fname):

        if self._lame_dll_handle is None:
            return None
        
        if self._lame_engine_handle is None:
            return None

        if fname is None:
            return None

        if not os.path.exists(fname):
            return None


        _result = LameScanResult()
        ret = self._lame_dll_handle.lame_scan_file(self._lame_engine_handle , fname , byref(_result))
        if ret < 0:
            return None
        
        return _result

    def ScanMem(self, data):
        if self._lame_dll_handle is None:
            return None
        
        if self._lame_engine_handle is None:
            return None

        if data is None:
            return None


        _result = LameScanResult()
        ret = self._lame_dll_handle.lame_scan_mem(self._lame_engine_handle , data , len(data) , byref(_result))
        if ret < 0:
            return None
        
        return _result

    def Clone(self):
        _lame = LameWithFeedback(self._lame_path) 

        for s in self._params:
            _lame.SetParam(s)
        
        if not _lame.Load(self._vbd_handle):
            return None
        
        return _lame


CMPFUNC = WINFUNCTYPE(None,c_char_p, POINTER(LameScanResult),c_void_p) 
def LameCallback(fname , result , userdata):
    
    _result = None

    if  hasattr(result , "contents"):
        _result = result.contents

    _lame = cast(userdata, py_object).value
    _lame.Report(fname , _result)



class LameWithFeedback(LameBase):
 

    def __init__(self , lpath):
        LameBase.__init__(self , lpath)
        self._callback = None
        self._this_obj_ref = c_void_p()
        self._this_obj_ref.value = id(self)

    def SetCallack(self,cb):
        self._callback = cb


    def Report(self , fname , result):
        self._callback(fname , result)

    def ScanFile(self,fname):

        if self._lame_dll_handle is None:
            return False
        
        if self._lame_engine_handle is None:
            return False

        if fname is None:
            return False
        
        self._lame_dll_handle.lame_scan_file_with_callback(self._lame_engine_handle , fname , CMPFUNC(LameCallback) , self._this_obj_ref)

        return True

    def ScanMem(self, data ):
        if self._lame_dll_handle is None:
            return False
        
        if self._lame_engine_handle is None:
            return False

        if data is None:
            return False
        
        self._lame_dll_handle.lame_scan_mem_with_callback(self._lame_engine_handle , data , len(data) , CMPFUNC(LameCallback) , self._this_obj_ref)

        return True



    def Clone(self):
        _lame = LameWithFeedback(self._lame_path) 

        for s in self._params:
            _lame.SetParam(s)
        
        if not _lame.Load(self._vbd_handle):
            return None

        _lame.SetCallack(self._callback)
        
        return _lame


            
