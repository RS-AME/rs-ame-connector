package engine;

public class ScanResult {
	public int mklass;
	public int vid32;	
	public int treat;
	public String engid;
	public String vname;
	public String hitag;
	
	public ScanResult() {
		
	}
	
	public ScanResult(ScanResult dr) {
		if(null == dr) return;
		mklass = dr.mklass;
		vid32 = dr.vid32;
		treat = dr.treat;
		engid = dr.engid;
		vname = dr.vname;
		hitag = dr.hitag;
	}
}
