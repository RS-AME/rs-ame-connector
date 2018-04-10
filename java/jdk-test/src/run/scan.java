package run;

import java.io.File;

import engine.JdkInterface;
import engine.LicenceInfo;
import engine.LameInfo;
import engine.ScanResult;
import engine.Lame;

public class scan extends Lame implements JdkInterface {

	// test Jni API
	public void TestInterface(String filename) {
		long vdb = LameOpenVdb(null);
		if(0 == vdb) return;
		long lame = LameCreate(vdb);
		if(0 == lame){
			LameCloseVdb(vdb);
			return;
		}
		LameParamSet(lame,"precise-format=0");
		if(0 > LameInit(lame)) {
			LameCloseVdb(vdb);
			LameDestroy(lame);
			return;
		}
		ScanResult sr = LameScanFile(lame,filename);
		if(null != sr){
			System.out.println(filename + "\t" + sr.vname + "!" + sr.vid32);
			LameScanResultRelease(sr);
		} else {
			System.out.println(filename);
		}
		
		if(0 != vdb) LameCloseVdb(vdb);
		if(0 != lame) LameDestroy(lame);

		LameInfo lv = LameGetVersion();
		if(null != lv){
			System.out.println("sdk version info");
			System.out.println("engine: " + lv.engine_version);
			System.out.println("malware: " + lv.virus_db_version);
			LameInfoRelease(lv);
		}
		
		LicenceInfo li = LameGetLicenceInfo();
		if(null != li){
			System.out.println("licence info");
			System.out.println("version: " + li.Version);
			System.out.println("Owner: " + li.Owner);
			System.out.println("Date: " + li.Date);
			System.out.println("Authm: " + li.Authm);
			System.out.println("Data: " + li.Data);
			LameLicenceRelease(li);
		}
	}
	public void callback(String fname, ScanResult sr) {
		if(null == fname || null == sr) return;
		System.out.println("[callback] " + fname + "\tInfected" + sr.vname + "!" + sr.engid);
	}
	// test lame API
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
		System.out.println("------------ main start ----------------------");
		scan s = new scan();
		String path = args[0];
		//s.TestInterface(path);
		boolean usecb = true;
		//if(usecb) s.LameSetCallback(s);
		s.LameTest(path,usecb);
		//if(usecb) s.LameResetCallback(s);
		System.out.println("------------ main end ----------------------");
	}
}