#ifndef __SDK_TYPES_H__
#define __SDK_TYPES_H__

#define _OPTIONAL_

#ifdef __cplusplus
#define EXTERN_C    extern "C"
#else
#define EXTERN_C    extern
typedef char bool;
#define true	1
#define false	0
#endif

#include <stdint.h>


#define VIRUS_LENGTH 256
#define ENGID_LENGTH 32
#define HITAG_LENGTH 32
#define ENGINE_VERSION_LEN	32
#define VIRUSDB_VERSIN_LEN	32

typedef enum rx_mk_t {
	mkTrait		= 0,
	mkHidden		= 1,
	mkHiddenFU	= 2,
	mkComplier	= 3,
	mkPacker		= 4,
	mkFormat		= 5,		// �ļ���ʽ
	mkAppType	= 6,
	mkTrusted	= 9,

	mkMalware = 0x10,

	mkTrojan,
	mkBackdoor,
	mkWorm,
	mkRootkit,
	mkExploit,
	mkHackTool,
	mkAdware,
	mkStealer,
	mkSpammer,
	mkSpyware,
	mkVirus,
	mkJoke,
	mkJunk,
	mkPUA,
	mkDownloader,
	mkDropper,
	/// 2015-12-21
	mkRansom,
	mkHoax,
	mkRiskware,
	// 2016-6-7
	mkUnwanted,
	mkMonetizer,				//	����

	mkMobileBase		= 0xC0,
	// Mobile
	//	ժ�ԡ��ƶ�������������������淶��
	mkPayment = mkMobileBase,	//	����۷�
	mkPrivacy,				//	��˽��ȡ
	mkRemote, 				//	Զ�̿���
	mkSpread, 				//	���⴫��
	mkExpense, 				//	�ʷ�����
	mkSystem, 				//	ϵͳ�ƻ�
	mkFraud, 				//	��ƭ��թ
	mkRogue,					//	��å��Ϊ


	mkAttention = 0xFE,		// ע��!
	mkTypeMax = 0x100,
} rx_mk_t;


enum{
	COMPOUND_SIZE = 0,
	MEST_DEPTH,
	TREAT_TIMES,
	PRECISE_FORMAT,
	KILL,
	MALWARE_PATH,
};


typedef enum rx_scan_opt_t {
	OPT_NO_COW		= 1,
	OPT_TREAT		= 2,
} rx_scan_opt_t;

typedef enum rx_treat_result_code_t {
	TREAT_FAIL_FIX = -1,
	TREAT_OK_DELETED = 1,
	TREAT_OK_FIXED = 2,
} rx_trc_t;

typedef struct rx_licence_info
{
	char			Version[16];
	char			Owner[128];
	char			Date[64];
	char			Authm[32];
	char			Data[2048];
} rx_licence_info;



#endif