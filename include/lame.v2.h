#ifndef __LAME_V2_H__
#define __LAME_V2_H__

#include "sdk.types.h"

typedef struct lame_scan_result
{
	rx_mk_t			mklass;
	char			engid[ENGID_LENGTH];
	char			vname[VIRUS_LENGTH];
	uint32_t		vid32;
	uint64_t		vid40;
	char			hitag[HITAG_LENGTH];
	rx_trc_t		treat;
}lame_scan_result;


typedef struct lame_info_
{
	char  engv[ENGINE_VERSION_LEN+1];
	char  vdbv[VIRUSDB_VERSIN_LEN+1];
}lame_info;

#define LAMECALLTYPE      __cdecl
#define LAMESTDCALLTYPE      __stdcall

#ifndef _WIN32
#define __cdecl
#define __stdcall
#endif

typedef void  (LAMESTDCALLTYPE *lame_callback)(const char* file_name , lame_scan_result* info,void* usr_data);



#define LAME_SUCCEEDED(hr)   (((long)(hr)) >= 0)
#define LAME_FAILED(hr)      (((long)(hr)) < 0)

EXTERN_C void*  lame_open_vdb(const char* vlibf);
EXTERN_C void   lame_close_vdb(void* vdb);

EXTERN_C void*  lame_create(void* vdb);
EXTERN_C void   lame_destroy(void* lame);
EXTERN_C void*  lame_fork(void* lame);

EXTERN_C long   lame_param_set(void* lame , const char*  param);
EXTERN_C long   lame_init(void* lame);
EXTERN_C long   lame_scan_file(void* lame , const char* fname , lame_scan_result* pResult) ;
EXTERN_C long   lame_scan_mem(void* lame , uint8_t* data , uint32_t size , lame_scan_result* pResult);


EXTERN_C long   lame_scan_file_with_callback(void* lame , const char* fname , lame_callback cb , void* user_data) ;
EXTERN_C long   lame_scan_mem_with_callback(void* lame , uint8_t* data , uint32_t size , lame_callback cb , void* user_data);

EXTERN_C long   lame_get_version(lame_info* info);
EXTERN_C long   lame_get_licence_info(rx_licence_info* info);

#endif