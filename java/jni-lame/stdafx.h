#pragma once

#ifdef _WIN32

#define WIN32_LEAN_AND_MEAN
#include <windows.h>

#else

typedef long				HRESULT;
#define E_FAIL				long(0x80004005L)
#define S_OK				long(0x0L)
#define E_INVALIDARG		long(0x80070057L)
#include <stdint.h>

#endif

#include <jni.h>
#include "../include/lame.h"
#include "../include/lame.v2.h"
#include "jnistring.h"