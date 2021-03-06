/* DO NOT EDIT THIS FILE - it is machine generated */
#include <jni.h>
/* Header for class engine_RavEngine */

#ifndef _Included_engine_RavEngine
#define _Included_engine_RavEngine
#ifdef __cplusplus
extern "C" {
#endif
/*
 * Class:     engine_RavEngine
 * Method:    LameOpenVdb
 * Signature: (Ljava/lang/String;)J
 */
JNIEXPORT jlong JNICALL Java_engine_RavEngine_LameOpenVdb
  (JNIEnv *, jobject, jstring);

/*
 * Class:     engine_RavEngine
 * Method:    LameCloseVdb
 * Signature: (J)V
 */
JNIEXPORT void JNICALL Java_engine_RavEngine_LameCloseVdb
  (JNIEnv *, jobject, jlong);

/*
 * Class:     engine_RavEngine
 * Method:    LameCreate
 * Signature: (J)J
 */
JNIEXPORT jlong JNICALL Java_engine_RavEngine_LameCreate
  (JNIEnv *, jobject, jlong);

/*
 * Class:     engine_RavEngine
 * Method:    LameDestroy
 * Signature: (J)V
 */
JNIEXPORT void JNICALL Java_engine_RavEngine_LameDestroy
  (JNIEnv *, jobject, jlong);

/*
 * Class:     engine_RavEngine
 * Method:    LameFork
 * Signature: (J)J
 */
JNIEXPORT jlong JNICALL Java_engine_RavEngine_LameFork
  (JNIEnv *, jobject, jlong);

/*
 * Class:     engine_RavEngine
 * Method:    LameParamSet
 * Signature: (JLjava/lang/String;)J
 */
JNIEXPORT jlong JNICALL Java_engine_RavEngine_LameParamSet
  (JNIEnv *, jobject, jlong, jstring);

/*
 * Class:     engine_RavEngine
 * Method:    LameInit
 * Signature: (J)J
 */
JNIEXPORT jlong JNICALL Java_engine_RavEngine_LameInit
  (JNIEnv *, jobject, jlong);

/*
 * Class:     engine_RavEngine
 * Method:    LameScanFile
 * Signature: (JLjava/lang/String;)Lengine/ScanResult;
 */
JNIEXPORT jobject JNICALL Java_engine_RavEngine_LameScanFile
  (JNIEnv *, jobject, jlong, jstring);

/*
 * Class:     engine_RavEngine
 * Method:    LameScanFileWithCallback
 * Signature: (JLjava/lang/String;Lengine/JdkInterface;)J
 */
JNIEXPORT jlong JNICALL Java_engine_RavEngine_LameScanFileWithCallback
  (JNIEnv *, jobject, jlong, jstring, jobject);

/*
 * Class:     engine_RavEngine
 * Method:    LameScanMem
 * Signature: (J[B)Lengine/ScanResult;
 */
JNIEXPORT jobject JNICALL Java_engine_RavEngine_LameScanMem
  (JNIEnv *, jobject, jlong, jbyteArray);

/*
 * Class:     engine_RavEngine
 * Method:    LameScanMemWithCallback
 * Signature: (J[BLengine/JdkInterface;)J
 */
JNIEXPORT jlong JNICALL Java_engine_RavEngine_LameScanMemWithCallback
  (JNIEnv *, jobject, jlong, jbyteArray, jobject);

/*
 * Class:     engine_RavEngine
 * Method:    LameScanResultRelease
 * Signature: (Lengine/ScanResult;)V
 */
JNIEXPORT void JNICALL Java_engine_RavEngine_LameScanResultRelease
  (JNIEnv *, jobject, jobject);

/*
 * Class:     engine_RavEngine
 * Method:    LameGetVersion
 * Signature: ()Lengine/LameInfo;
 */
JNIEXPORT jobject JNICALL Java_engine_RavEngine_LameGetVersion
  (JNIEnv *, jobject);

/*
 * Class:     engine_RavEngine
 * Method:    LameInfoRelease
 * Signature: (Lengine/LameInfo;)V
 */
JNIEXPORT void JNICALL Java_engine_RavEngine_LameInfoRelease
  (JNIEnv *, jobject, jobject);

/*
 * Class:     engine_RavEngine
 * Method:    LameGetLicenceInfo
 * Signature: ()Lengine/LicenceInfo;
 */
JNIEXPORT jobject JNICALL Java_engine_RavEngine_LameGetLicenceInfo
  (JNIEnv *, jobject);

/*
 * Class:     engine_RavEngine
 * Method:    LameLicenceRelease
 * Signature: (Lengine/LicenceInfo;)V
 */
JNIEXPORT void JNICALL Java_engine_RavEngine_LameLicenceRelease
  (JNIEnv *, jobject, jobject);

#if 0
/*
 * Class:     engine_RavEngine
 * Method:    LameSetCallback
 * Signature: (Lengine/JdkInterface;)J
 */
JNIEXPORT jlong JNICALL Java_engine_RavEngine_LameSetCallback
  (JNIEnv *, jobject, jobject);

/*
 * Class:     engine_RavEngine
 * Method:    LameResetCallback
 * Signature: (Lengine/JdkInterface;)J
 */
JNIEXPORT jlong JNICALL Java_engine_RavEngine_LameResetCallback
  (JNIEnv *, jobject, jobject);
#endif

#ifdef __cplusplus
}
#endif
#endif
