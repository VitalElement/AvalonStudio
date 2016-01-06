//
//    LLILUM OS Abstraction Layer - Errors
// 

#ifndef __UW_ERROR_H_
#define __UW_ERROR_H_

//
// Errors
//

#if defined(WIN32)

#include <winerror.h>

#else // WIN32

typedef int HRESULT;

//
// Severity values
//

#define SEVERITY_SUCCESS    0
#define SEVERITY_ERROR      1


//
// Generic test for success on any status value (non-negative numbers
// indicate success).
//

#define SUCCEEDED(Status) ((HRESULT)(Status) >= 0)

//
// and the inverse
//

#define FAILED(Status) ((HRESULT)(Status)<0)


//
// Success codes
//
#define S_OK                                   ((HRESULT)0x00000000L)
#define S_FALSE                                ((HRESULT)0x00000001L)


//
// Return the code
//

#define HRESULT_CODE(hr)    ((hr) & 0xFFFF)

//
//  Return the facility
//

#define HRESULT_FACILITY(hr)  (((hr) >> 16) & 0x1fff)

//
//  Return the severity
//

#define HRESULT_SEVERITY(hr)  (((hr) >> 31) & 0x1)

//
// Create an HRESULT value from component pieces
//

#define MAKE_HRESULT(sev,fac,code) ((HRESULT) (((unsigned long)(sev)<<31) | ((unsigned long)(fac)<<16) | ((unsigned long)(code))) )


//--//

//
// These are denormalized HRESULTs, only the 8 most significant bits are set, to allow use of MOV <rd>,#<imm> by the compiler.
//

#define LLOS_E_UNKNOWN_INSTRUCTION                      MAKE_HRESULT( SEVERITY_ERROR  , 0x0100, 0x0000 )
#define LLOS_E_UNSUPPORTED_INSTRUCTION                  MAKE_HRESULT( SEVERITY_ERROR  , 0x0200, 0x0000 )

#define LLOS_E_STACK_OVERFLOW                           MAKE_HRESULT( SEVERITY_ERROR  , 0x1100, 0x0000 )
#define LLOS_E_STACK_UNDERFLOW                          MAKE_HRESULT( SEVERITY_ERROR  , 0x1200, 0x0000 )

#define LLOS_E_ENTRY_NOT_FOUND                          MAKE_HRESULT( SEVERITY_ERROR  , 0x1500, 0x0000 )
#define LLOS_E_ASSM_WRONG_CHECKSUM                      MAKE_HRESULT( SEVERITY_ERROR  , 0x1600, 0x0000 )
#define LLOS_E_ASSM_PATCHING_NOT_SUPPORTED              MAKE_HRESULT( SEVERITY_ERROR  , 0x1700, 0x0000 )
#define LLOS_E_SHUTTING_DOWN                            MAKE_HRESULT( SEVERITY_ERROR  , 0x1800, 0x0000 )
#define LLOS_E_OBJECT_DISPOSED                          MAKE_HRESULT( SEVERITY_ERROR  , 0x1900, 0x0000 )
#define LLOS_E_WATCHDOG_TIMEOUT                         MAKE_HRESULT( SEVERITY_ERROR  , 0x1A00, 0x0000 )

#define LLOS_E_NULL_REFERENCE                           MAKE_HRESULT( SEVERITY_ERROR  , 0x2100, 0x0000 )
#define LLOS_E_WRONG_TYPE                               MAKE_HRESULT( SEVERITY_ERROR  , 0x2200, 0x0000 )
#define LLOS_E_TYPE_UNAVAILABLE                         MAKE_HRESULT( SEVERITY_ERROR  , 0x2300, 0x0000 )
#define LLOS_E_INVALID_CAST                             MAKE_HRESULT( SEVERITY_ERROR  , 0x2400, 0x0000 )
#define LLOS_E_OUT_OF_RANGE                             MAKE_HRESULT( SEVERITY_ERROR  , 0x2500, 0x0000 )

#define LLOS_E_SERIALIZATION_VIOLATION                  MAKE_HRESULT( SEVERITY_ERROR  , 0x2700, 0x0000 )
#define LLOS_E_SERIALIZATION_BADSTREAM                  MAKE_HRESULT( SEVERITY_ERROR  , 0x2800, 0x0000 )
#define LLOS_E_INDEX_OUT_OF_RANGE                       MAKE_HRESULT( SEVERITY_ERROR  , 0x2900, 0x0000 )

#define LLOS_E_DIVIDE_BY_ZERO                           MAKE_HRESULT( SEVERITY_ERROR  , 0x3100, 0x0000 )

#define LLOS_E_BUSY                                     MAKE_HRESULT( SEVERITY_ERROR  , 0x3300, 0x0000 )

#define LLOS_E_PROCESS_EXCEPTION                        MAKE_HRESULT( SEVERITY_ERROR  , 0x4100, 0x0000 )

#define LLOS_E_THREAD_WAITING                           MAKE_HRESULT( SEVERITY_ERROR  , 0x4200, 0x0000 )

#define LLOS_E_LOCK_SYNCHRONIZATION_EXCEPTION           MAKE_HRESULT( SEVERITY_ERROR  , 0x4400, 0x0000 )

#define LLOS_E_APPDOMAIN_EXITED                         MAKE_HRESULT( SEVERITY_ERROR  , 0x4800, 0x0000 )
#define LLOS_E_APPDOMAIN_MARSHAL_EXCEPTION              MAKE_HRESULT( SEVERITY_ERROR  , 0x4900, 0x0000 )
#define LLOS_E_NOTIMPL                                  MAKE_HRESULT( SEVERITY_ERROR  , 0x4a00, 0x0000 )

#define LLOS_E_UNKNOWN_TYPE                             MAKE_HRESULT( SEVERITY_ERROR  , 0x4d00, 0x0000 )
#define LLOS_E_ARGUMENT_NULL                            MAKE_HRESULT( SEVERITY_ERROR  , 0x4e00, 0x0000 )
#define LLOS_E_IO                                       MAKE_HRESULT( SEVERITY_ERROR  , 0x4f00, 0x0000 )

#define LLOS_E_ENTRYPOINT_NOT_FOUND                     MAKE_HRESULT( SEVERITY_ERROR  , 0x5000, 0x0000 )
#define LLOS_E_DRIVER_NOT_REGISTERED                    MAKE_HRESULT( SEVERITY_ERROR  , 0x5100, 0x0000 )


//
// Gp IO error codes
//
#define LLOS_E_PIN_UNAVAILABLE                          MAKE_HRESULT( SEVERITY_ERROR  , 0x5400, 0x0000 )
#define LLOS_E_PIN_DEAD                                 MAKE_HRESULT( SEVERITY_ERROR  , 0x5500, 0x0000 )
#define LLOS_E_INVALID_OPERATION                        MAKE_HRESULT( SEVERITY_ERROR  , 0x5600, 0x0000 )
#define LLOS_E_WRONG_INTERRUPT_TYPE                     MAKE_HRESULT( SEVERITY_ERROR  , 0x5700, 0x0000 )
#define LLOS_E_NO_INTERRUPT                             MAKE_HRESULT( SEVERITY_ERROR  , 0x5800, 0x0000 )
#define LLOS_E_DISPATCHER_ACTIVE                        MAKE_HRESULT( SEVERITY_ERROR  , 0x5900, 0x0000 )

//
// IO error codes 
// (Keep in-sync with IOExceptionErrorCode enum in IOException.cs)
//
#define LLOS_E_FILE_IO                                  MAKE_HRESULT( SEVERITY_ERROR  , 0x6000, 0x0000 )
#define LLOS_E_INVALID_DRIVER                           MAKE_HRESULT( SEVERITY_ERROR  , 0x6100, 0x0000 )
#define LLOS_E_FILE_NOT_FOUND                           MAKE_HRESULT( SEVERITY_ERROR  , 0x6200, 0x0000 )
#define LLOS_E_DIRECTORY_NOT_FOUND                      MAKE_HRESULT( SEVERITY_ERROR  , 0x6300, 0x0000 )
#define LLOS_E_VOLUME_NOT_FOUND                         MAKE_HRESULT( SEVERITY_ERROR  , 0x6400, 0x0000 )
#define LLOS_E_PATH_TOO_LONG                            MAKE_HRESULT( SEVERITY_ERROR  , 0x6500, 0x0000 )
#define LLOS_E_DIRECTORY_NOT_EMPTY                      MAKE_HRESULT( SEVERITY_ERROR  , 0x6600, 0x0000 )
#define LLOS_E_UNAUTHORIZED_ACCESS                      MAKE_HRESULT( SEVERITY_ERROR  , 0x6700, 0x0000 )
#define LLOS_E_PATH_ALREADY_EXISTS                      MAKE_HRESULT( SEVERITY_ERROR  , 0x6800, 0x0000 )
#define LLOS_E_TOO_MANY_OPEN_HANDLES                    MAKE_HRESULT( SEVERITY_ERROR  , 0x6900, 0x0000 )

//
// General error codes
//

#define LLOS_E_NOT_FOUND                                MAKE_HRESULT( SEVERITY_ERROR  , 0x7500, 0x0000 )
#define LLOS_E_BUFFER_TOO_SMALL                         MAKE_HRESULT( SEVERITY_ERROR  , 0x7600, 0x0000 )
#define LLOS_E_NOT_SUPPORTED                            MAKE_HRESULT( SEVERITY_ERROR  , 0x7700, 0x0000 )
#define LLOS_E_HMAC_NOT_SUPPORTED                       MAKE_HRESULT( SEVERITY_ERROR  , 0x7701, 0x0000 )
#define LLOS_E_RESCHEDULE                               MAKE_HRESULT( SEVERITY_ERROR  , 0x7800, 0x0000 )

#define LLOS_E_OUT_OF_MEMORY                            MAKE_HRESULT( SEVERITY_ERROR  , 0x7A00, 0x0000 )
#define LLOS_E_RESTART_EXECUTION                        MAKE_HRESULT( SEVERITY_ERROR  , 0x7B00, 0x0000 )

#define LLOS_E_INVALID_PARAMETER                        MAKE_HRESULT( SEVERITY_ERROR  , 0x7D00, 0x0000 )
#define LLOS_E_TIMEOUT                                  MAKE_HRESULT( SEVERITY_ERROR  , 0x7E00, 0x0000 )
#define LLOS_E_FAIL                                     MAKE_HRESULT( SEVERITY_ERROR  , 0x7F00, 0x0000 )

//--//

#define LLOS_S_THREAD_EXITED                            MAKE_HRESULT( SEVERITY_SUCCESS, 0x0100, 0x0000 )
#define LLOS_S_QUANTUM_EXPIRED                          MAKE_HRESULT( SEVERITY_SUCCESS, 0x0200, 0x0000 )
#define LLOS_S_NO_READY_THREADS                         MAKE_HRESULT( SEVERITY_SUCCESS, 0x0300, 0x0000 )
#define LLOS_S_NO_THREADS                               MAKE_HRESULT( SEVERITY_SUCCESS, 0x0400, 0x0000 )
#define LLOS_S_RESTART_EXECUTION                        MAKE_HRESULT( SEVERITY_SUCCESS, 0x0500, 0x0000 )

#endif //WIN32 || _WIN32

#endif // __UW_ERROR_H_
