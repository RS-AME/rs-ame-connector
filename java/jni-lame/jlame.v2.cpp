#include "stdafx.h"
#include "jlame.v2.h"


JNIEXPORT jlong JNICALL Java_engine_RavEngine_LameOpenVdb
	(JNIEnv * env, jobject jobj, jstring vlibf)
{
	Jni_auto_release_string jar_vlibf(env,vlibf);
	return (jlong)lame_open_vdb(jar_vlibf);
}

JNIEXPORT void JNICALL Java_engine_RavEngine_LameCloseVdb
	(JNIEnv * env, jobject jobj, jlong vdb)
{
	lame_close_vdb((void*)vdb);
}

JNIEXPORT jlong JNICALL Java_engine_RavEngine_LameCreate
	(JNIEnv * env, jobject jobj, jlong vdb)
{
	if(NULL == vdb) return NULL;
	return (jlong)lame_create((void*)vdb);
}

JNIEXPORT void JNICALL Java_engine_RavEngine_LameDestroy
	(JNIEnv * env, jobject jobj, jlong lame)
{
	if(NULL != lame) lame_destroy((void*)lame);
}

JNIEXPORT jlong JNICALL Java_engine_RavEngine_LameFork
	(JNIEnv * env, jobject jobj, jlong lame)
{
	if(NULL == lame) return NULL;
	return (jlong)lame_fork((void*)lame);
}

JNIEXPORT jlong JNICALL Java_engine_RavEngine_LameParamSet
	(JNIEnv * env, jobject jobj, jlong lame, jstring param)
{
	if(NULL == lame || NULL == param) return E_INVALIDARG;
	Jni_auto_release_string jar_param(env,param);
	return lame_param_set((void*)lame,jar_param);
}

JNIEXPORT jlong JNICALL Java_engine_RavEngine_LameInit
	(JNIEnv * env, jobject jobj, jlong lame)
{
	if(NULL == lame) return E_INVALIDARG;
	return lame_init((void*)lame);
}

static jobject Local_Lame_Set_ScanResult(JNIEnv * env, jclass objectcls, jobject obj, lame_scan_result * result)
{
	if(NULL != objectcls && NULL != obj && NULL != result)
	{
		jfieldID engid = env->GetFieldID(objectcls,"engid","Ljava/lang/String;");
		jfieldID vname = env->GetFieldID(objectcls,"vname","Ljava/lang/String;");
		jfieldID hitag = env->GetFieldID(objectcls,"hitag","Ljava/lang/String;");
		jfieldID mklass = env->GetFieldID(objectcls,"mklass","I");
		jfieldID vid32 = env->GetFieldID(objectcls,"vid32","I");
		jfieldID treat = env->GetFieldID(objectcls,"treat","I");

		env->SetObjectField(obj,engid,env->NewStringUTF(result->engid));
		env->SetObjectField(obj,vname,env->NewStringUTF(result->vname));
		env->SetObjectField(obj,hitag,env->NewStringUTF(result->hitag));
		env->SetIntField(obj,mklass,result->mklass);
		env->SetIntField(obj,vid32,result->vid32);
		env->SetIntField(obj,treat,result->treat);
	}
	return obj;
}

JNIEXPORT jobject JNICALL Java_engine_RavEngine_LameScanFile
	(JNIEnv * env, jobject jobj, jlong lame, jstring fname)
{
	if(NULL == lame || NULL == fname) return NULL;
	Jni_auto_release_string jar_fname(env,fname);
	lame_scan_result result;
	if(S_OK == lame_scan_file((void*)lame,jar_fname,&result))
	{
		jclass objectcls = env->FindClass("engine/ScanResult");
		jobject obj = env->AllocObject(objectcls);
		return Local_Lame_Set_ScanResult(env, objectcls, obj, &result);
	}
	return NULL;
}

typedef struct CallbackInfor
{
	jobject			objInterface;
	JNIEnv*			env;
	jobject			objCallback;
}CallbackInfor;

#if 0
static CallbackInfor gs_cbInfo;

JNIEXPORT jlong JNICALL Java_engine_RavEngine_LameSetCallback
	(JNIEnv * env, jobject jobj, jobject jdkcb)
{
	gs_cbInfo.env = env;
	gs_cbInfo.objCallback = env->NewGlobalRef(jobj);
	gs_cbInfo.objInterface = env->NewGlobalRef(jdkcb);
	return S_OK;
}

JNIEXPORT jlong JNICALL Java_engine_RavEngine_LameResetCallback
	(JNIEnv * env, jobject jobj, jobject jdkcb)
{
	gs_cbInfo.env->DeleteGlobalRef(jdkcb);
	gs_cbInfo.env->DeleteGlobalRef(jobj);
	memset(&gs_cbInfo,0,sizeof(CallbackInfor));
	return S_OK;
}
#endif

void __stdcall Local_Lame_Scan_Callback(const char* fname, lame_scan_result* result, void* user_data)
{
	CallbackInfor* cbinfor = (CallbackInfor*)user_data;
	if(NULL == cbinfor) return;

	JNIEnv* env = cbinfor->env;
	if(NULL == env) return;

	JavaVM* pVm;
	env->GetJavaVM(&pVm);
	if(NULL == pVm) return;
	pVm->AttachCurrentThread((void**)&env,NULL);

	jstring fnameobj = NULL;
	jobject scanresultobj = NULL;
	do
	{
		jobject objInterface = cbinfor->objInterface;
		if(NULL == objInterface) return;
		jclass clscb = env->GetObjectClass(objInterface);
		if(NULL == clscb) return;
		jmethodID midcb = env->GetMethodID(clscb,"callback","(Ljava/lang/String;Lengine/ScanResult;)V");
		if(NULL == midcb) return;

		if(result)
		{
			//parameter 1
			fnameobj = env->NewStringUTF(fname);
			//paramter 2
			jclass srcls = env->FindClass("engine/ScanResult");
			if(NULL == srcls) break;
			jmethodID midsr = env->GetMethodID(srcls, "<init>", "()V");
			scanresultobj = env->NewObject(srcls, midsr);
			if(NULL == scanresultobj) break;
			Local_Lame_Set_ScanResult(env,srcls,scanresultobj,result);
		}
		//call
		env->CallVoidMethod(objInterface,midcb,fnameobj,scanresultobj);
	} while (0);

	pVm->DetachCurrentThread();

	//release
	if(fnameobj) env->DeleteLocalRef(fnameobj);
	if(scanresultobj) env->DeleteLocalRef(scanresultobj);
}

JNIEXPORT jlong JNICALL Java_engine_RavEngine_LameScanFileWithCallback
	(JNIEnv * env, jobject jobj, jlong lame, jstring fname, jobject cb)
{
	if(NULL == lame || NULL == fname || NULL == cb) return E_INVALIDARG;
	Jni_auto_release_string jar_fname(env,fname);
	CallbackInfor cbInfo = {};
	cbInfo.env = env;
	cbInfo.objCallback = env->NewLocalRef(jobj);
	cbInfo.objInterface = env->NewLocalRef(cb);
	HRESULT hr = lame_scan_file_with_callback((void*)lame,jar_fname,Local_Lame_Scan_Callback,&cbInfo);
	env->DeleteLocalRef(cb);
	env->DeleteLocalRef(jobj);
	return hr;
}

JNIEXPORT jobject JNICALL Java_engine_RavEngine_LameScanMem
	(JNIEnv * env, jobject jobj, jlong lame, jbyteArray data)
{
	if(NULL == data) return NULL;
	jsize length = env->GetArrayLength(data);
	Jni_auto_release_array<uint8_t> jay(env, data);
	if( 0 == jay || 0 == length ) return NULL;
	lame_scan_result result;
	if(S_OK == lame_scan_mem((void*)lame, jay, length, &result))
	{
		jclass objectcls = env->FindClass("engine/ScanResult");
		jobject obj = env->AllocObject(objectcls);
		return Local_Lame_Set_ScanResult(env,objectcls,obj,&result);
	}
	return NULL;
}

JNIEXPORT jlong JNICALL Java_engine_RavEngine_LameScanMemWithCallback
	(JNIEnv * env, jobject jobj, jlong lame, jbyteArray data, jobject cb)
{
	if(NULL == data || NULL == cb) return E_INVALIDARG;
	jsize length = env->GetArrayLength(data);
	Jni_auto_release_array<uint8_t> jay(env, data);
	if( 0 == jay || 0 == length ) return E_INVALIDARG;
	CallbackInfor cbInfo = {};
	cbInfo.env = env;
	cbInfo.objCallback = env->NewLocalRef(jobj);
	cbInfo.objInterface = env->NewLocalRef(cb);
	HRESULT hr = lame_scan_mem_with_callback((void*)lame,jay,length,Local_Lame_Scan_Callback,&cbInfo);
	env->DeleteLocalRef(cb);
	env->DeleteLocalRef(jobj);
	return hr;
}

JNIEXPORT jobject JNICALL Java_engine_RavEngine_LameGetVersion
	(JNIEnv * env, jobject jobj)
{
	lame_info info = {};
	jobject obj = NULL;
	if(S_OK == lame_get_version(&info))
	{
		jclass objectcls = env->FindClass("engine/LameInfo");
		if(objectcls)
		{
			obj = env->AllocObject(objectcls);
			jfieldID engine_version = env->GetFieldID(objectcls,"engine_version","Ljava/lang/String;");
			jfieldID virus_db_version = env->GetFieldID(objectcls,"virus_db_version","Ljava/lang/String;");

			env->SetObjectField(obj,engine_version,env->NewStringUTF(info.engv));
			env->SetObjectField(obj,virus_db_version,env->NewStringUTF(info.vdbv));
		}
	}
	return obj;
}

JNIEXPORT jobject JNICALL Java_engine_RavEngine_LameGetLicenceInfo
	(JNIEnv * env, jobject jobj)
{
	rx_licence_info info = {};
	jobject obj = NULL;
	if(S_OK == lame_get_licence_info(&info))
	{
		jclass objectcls = env->FindClass("engine/LicenceInfo");
		if(objectcls)
		{
			obj = env->AllocObject(objectcls);
			jfieldID Version = env->GetFieldID(objectcls,"Version","Ljava/lang/String;");
			jfieldID Owner = env->GetFieldID(objectcls,"Owner","Ljava/lang/String;");
			jfieldID Date = env->GetFieldID(objectcls,"Date","Ljava/lang/String;");
			jfieldID Authm = env->GetFieldID(objectcls,"Authm","Ljava/lang/String;");
			jfieldID Data = env->GetFieldID(objectcls,"Data","Ljava/lang/String;");

			env->SetObjectField(obj,Version,env->NewStringUTF(info.Version));
			env->SetObjectField(obj,Owner,env->NewStringUTF(info.Owner));
			env->SetObjectField(obj,Date,env->NewStringUTF(info.Date));
			env->SetObjectField(obj,Authm,env->NewStringUTF(info.Authm));
			env->SetObjectField(obj,Data,env->NewStringUTF(info.Data));
		}
	}
	return obj;
}

JNIEXPORT void JNICALL Java_engine_RavEngine_LameScanResultRelease
	(JNIEnv * env, jobject jobj, jobject obj)
{
	if(NULL == obj) return;
	env->DeleteLocalRef(obj);
}

JNIEXPORT void JNICALL Java_engine_RavEngine_LameInfoRelease
	(JNIEnv * env, jobject jobj, jobject obj)
{
	if(NULL == obj) return;
	env->DeleteLocalRef(obj);
}

JNIEXPORT void JNICALL Java_engine_RavEngine_LameLicenceRelease
	(JNIEnv * env, jobject jobj, jobject obj)
{
	if(NULL == obj) return;
	env->DeleteLocalRef(obj);
}
