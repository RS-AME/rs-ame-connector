package engine;

import java.util.ArrayList;
import java.io.File;
import java.util.List;

public class Lame extends RavEngine /*implements JdkInterface*/ {
	protected List<String> parameters = new ArrayList<>();
	protected long engine = 0;
	protected long vdb_handle = 0;

	public long OpenVdb(String vdbf) {
		return LameOpenVdb(vdbf);
	}
	public void CloseVdb() {
		if(0 != vdb_handle) {
			LameCloseVdb(vdb_handle);
			vdb_handle = 0;
		}
	}
	public boolean Load(long lib) {
		if(0 == lib) {
			lib = OpenVdb(null);
		}
		engine = LameCreate(lib);
		if(0 == engine) return false;
		for(String str:parameters) {
			LameParamSet(engine, str);
		}
		if(0 > LameInit(engine)) {
			LameDestroy(engine);
			return false;
		}
		vdb_handle = lib;
		return true;
	}
	public boolean Set(String param) {
		if(null == param) return false;
		parameters.add(param);
		return true;
	}
	public void Unload() {
		if(0 != vdb_handle) {
			CloseVdb();
			vdb_handle = 0;
		}
		if(0 != engine) {
			LameDestroy(engine);
			engine = 0;
		}
	}
	public LicenceInfo GetLicense() {
		LicenceInfo li = LameGetLicenceInfo();
		if(null == li) return null;
		LicenceInfo info = new LicenceInfo(li);
		LameLicenceRelease(li);
		return info;
	}
	public LameInfo GetVersion() {
		LameInfo li = LameGetVersion();
		if(null == li) return null;
		LameInfo info = new LameInfo(li);
		LameInfoRelease(li);
		return info;
	}
	public ScanResult ScanFile(String fname) {
		if(0 == engine || null == fname) return null;
		ScanResult result = LameScanFile(engine, fname);
		if(null == result) return null;
		ScanResult sr = new ScanResult(result);
		LameScanResultRelease(result);
		return sr;
	}
	public ScanResult ScanMem(byte [] data) {
		if(0 == engine || null == data) return null;
		ScanResult result = LameScanMem(engine, data);
		if(null == result) return null;
		ScanResult sr = new ScanResult(result);
		LameScanResultRelease(result);
		return sr;
	}
	public boolean ScanFileWithCallback(String fname,JdkInterface cb) {
		if(null == fname) return false;
		return 0 == LameScanFileWithCallback(engine, fname, cb);
	}
	public boolean ScanMemWithCallback(byte [] data,JdkInterface cb) {
		if(null == data) return false;
		return 0 == LameScanMemWithCallback(engine, data, cb);
	}
	//public boolean SetCallback(JdkInterface cb) {
	//	if(null == cb) return false;
	//	return 0 == LameSetCallback(cb);
	//}
	//public boolean ResetCallback(JdkInterface cb) {
	//	if(null == cb) return false;
	//	return 0 == LameResetCallback(cb);
	//}

	/*
	// test
	public void callback(String fname, ScanResult sr) {
		if(null == fname || null == sr) return;
		System.out.println("[callback] " + fname + "\tInfected" + sr.vname + "!" + sr.engid);
	}

	public void PrintResultl(String path, boolean usecb) {
		if(usecb) {
			ScanFileWithCallback(path, this);
		}
		else {
			ScanResult sr = ScanFile(path);
			System.out.println("[normal] " + path + "\tInfected: " + sr.vname);	
		}
	}
	public void TravelPath(String path, boolean usecb) {
		File or = new File(path);
		if(!or.exists()) return;
		
		if(or.isDirectory()) {
			File [] files = or.listFiles();
			if (files != null) {
				for (File file : files) {
					if (file.isFile()) {
						PrintResultl(file.getAbsolutePath(), usecb);
					} else if (file.isDirectory()) {
						TravelPath(file.getAbsolutePath(), usecb);
					}
				}
			}
		} else if(or.isFile()) {
			PrintResultl(or.getAbsolutePath(), usecb);
		}
	}
	void LameTest(String path,boolean usecb) {
		File file = new File(path);
		if(!file.exists()) return;
		if(!Load(0)) return;
		TravelPath(path,usecb);
		Unload();
	}
	public static void main(String[] args) {
		Lame lame = new Lame();
		//lame.LameSetCallback(lame);
		lame.LameTest("d:\\rsdata\\5",true);
		//lame.LameResetCallback(lame);
	}
	//*/
}
