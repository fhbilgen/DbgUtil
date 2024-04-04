// NatDbgHelper.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "common.h"
#include <DbgHelp.h>

//PSTR g_dumpFile;
wchar_t* g_dumpFile;
PSTR g_symbolPath;

#define DEBUG_FORMAT_MESSAGE_LENGTH 300
#define MAX_ENV_BUFFER_LENGTH 32767

IDebugClient7*          g_dbgClient;
IDebugControl7*         g_dbgControl;
//IDebugSymbols5*         g_dbgSymbols;
IDebugSymbols3*         g_dbgSymbols;
IDebugSystemObjects4*   g_dbgSysObjects;
IDebugAdvanced3*        g_dbgAdvanced;
IDebugDataSpaces4*      g_dbgDataSpace;
ULONG                   g_thCount = 0;
wchar_t* g_wszCmdLine = NULL;

wchar_t DebuggeeTypeName[14][50] = { {_T("Not initialized")},
                                     {_T("Live Kernel Debugging")}, {_T("Local Kernel")}, {_T("Live Kernel Debugging connected to eXDI driver")}, {_T("Kernel-mode small dump")}, {_T("Kernel-mode kernel dump")},
                                     {_T("Kernel-mode complete dump")}, {_T("Live user mode debugging")}, {_T("User-mode small dump")}, {_T("User-mode full dump")},
                                      {_T("Invalid Kernel-mode Qualifier")}, {_T("Invalid User-mode Qualifier")}, {_T("N/A")} };

wchar_t g_dbgTypeName[17][30] = { _T("DEFAULT "), _T("WRITE_CAB "), {_T("CAB_SECONDARY_FILES ")}, {_T("NO_OVERWRITE ")}, {_T("FULL_MEMORY ")}, {_T("HANDLE_DATA ")}, {_T("UNLOADED_MODULES ")}, {_T("INDIRECT_MEMORY ")}
                                    , {_T("DATA_SEGMENTS ")} , {_T("FILTER_MEMORY ")} , {_T("FILTER_PATHS ")} , {_T("PROCESS_THREAD_DATA ")} , {_T("PRIVATE_READ_WRITE_MEMORY ")} , {_T("NO_OPTIONAL_DATA ")}
                                    , {_T("FULL_MEMORY_INFO ")} , {_T("THREAD_INFO ")}, {_T("CODE_SEGMENTS ")} };

wchar_t* GetDebuggeeTypeName(ULONG Class, ULONG Qualifier);
wchar_t* GetDebugTypeName(ULONG dbgType);
void DumpStack(void);

void Exit(int Code, _In_ _Printf_format_string_ PCSTR Format, ...)
{
    // Clean up any resources.
    if (g_dbgSymbols != NULL)
    {
        g_dbgSymbols->Release();
    }
    if (g_dbgControl != NULL)
    {
        g_dbgControl->Release();
    }
    if (g_dbgClient != NULL)
    {
        //
        // Request a simple end to any current session.
        // This may or may not do anything but it isn't
        // harmful to call it.
        //

        // We don't want to see any output from the shutdown.
        g_dbgClient->SetOutputCallbacks(NULL);

        g_dbgClient->EndSession(DEBUG_END_PASSIVE);

        g_dbgClient->Release();
    }

    // Output an error message if given.
   /* if (Format != NULL)
    {
        va_list Args;

        va_start(Args, Format);
        vfprintf(stderr, Format, Args);
        va_end(Args);
    }*/

    exit(Code);
}


void Initialize()
{
    HRESULT status = S_OK;

    if ( (status = DebugCreate(__uuidof(IDebugClient7), (void**)&g_dbgClient)) != S_OK)
        Exit(1, "DebugCreate Failed");

    if ( 
         (status = g_dbgClient->QueryInterface(__uuidof(IDebugControl7), (void**)&g_dbgControl)) != S_OK ||
         //(status = g_dbgClient->QueryInterface(__uuidof(IDebugSymbols5), (void**)&g_dbgSymbols)) != S_OK  ||
        (status = g_dbgClient->QueryInterface(__uuidof(IDebugSymbols3), (void**)&g_dbgSymbols)) != S_OK ||
         (status = g_dbgClient->QueryInterface(__uuidof(IDebugSystemObjects4), (void**)&g_dbgSysObjects)) != S_OK ||
         (status = g_dbgClient->QueryInterface(__uuidof(IDebugAdvanced3), (void**)&g_dbgAdvanced)) != S_OK ||
         (status = g_dbgClient->QueryInterface(__uuidof(IDebugDataSpaces4), (void**)&g_dbgDataSpace)) != S_OK
        )
        Exit(1, "QueryInterface failed");    
}

void MakeTheSymbolsWork()
{
    ULONG symOpt;
    ULONG symPathSize;
   
    
    ULONG* pProcs = NULL;
    HRESULT status;

    char path[200];
        

    symOpt = SYMOPT_CASE_INSENSITIVE | SYMOPT_UNDNAME | SYMOPT_DEFERRED_LOADS | SYMOPT_LOAD_LINES | SYMOPT_OMAP_FIND_NEAREST | SYMOPT_FAIL_CRITICAL_ERRORS |
        SYMOPT_AUTO_PUBLICS | SYMOPT_NO_IMAGE_SEARCH;

    
    status = g_dbgSymbols->SetSymbolOptions(symOpt);
    if (status != S_OK)
	    printf("Error in SetSymbolOption: 0x%x\n", status);

    //status = g_dbgSymbols->SetSymbolPath("srv*e:\\sym\\pri*https://symweb");
    status = g_dbgSymbols->SetSymbolPath("e:\\sym\\pri");
    if (status != S_OK)
        _tprintf(_T("Error happened in SetSymbolPath 0x:%x\n"), status);

    status = g_dbgSymbols->GetSymbolOptions(&symOpt);
    if (status != S_OK)
        printf("Error in GetSymbolOption: 0x%x\n", status);

    status = g_dbgSymbols->GetSymbolPath(path, 200, &symPathSize);
    if (status != S_OK)
        printf("Error in GetSymbolPath: 0x%x\n", status);

    _tprintf(_T("Symbol option: 0x%x\n"), symOpt);    
    printf("Symbol path  : %s\n", path);
    
}


void GetInformationFromDumpFile()
{
    HRESULT status = S_OK;
    ULONG procID;
    ULONG threadCount;
    ULONG* pThreads = NULL, *pSysThreads = NULL;
    PWSTR exeName = NULL;
    ULONG exeNameLen = 0, exeNameBufLen = 0;
    PWSTR compName = NULL;
    ULONG compNameLen = 0, compNameBufLen = 0;
    ULONG procUpTime = 0;
    ULONG sysUpTime = 0;
    ULONG dmpTime = 0;
    ULONG cpuType = 0;
    PWSTR cpuLongName = NULL;
    PWSTR cpuShortName = NULL;
    ULONG cpuLongNameLen = 0, cpuLongNameBufLen = 0;
    ULONG cpuShortNameLen = 0, cpuShortNameBufLen = 0;
    ULONG dbgeClass = 0, dbgeQualifier = 0;
    ULONG dmpFormat = 0;
    wchar_t* pdbgTypeName = NULL;
    ULONG gleType = 0,
        gleProcId = 0,
        gleThreadId = 0,
        gleExtraInfoSize = 0,
        gleExtraInfoUsed = 0,
        gleDescSize = 0,
        gleDescUsed = 0;
    DWORD expCode = 0;
    ULONG cpuCount = 0;
  
    PVOID gleExtraInfo = NULL;
    PSTR  gleDesc = NULL;
    ULONG platId = 0;
    ULONG win32Major = 0;
    ULONG win32Minor = 0;
    ULONG kdMajor = 0;
    ULONG kdMinor = 0;
    ULONG svPlatId = 0;
    ULONG svMajor = 0;
    ULONG svMinor = 0;
    PSTR svSPString = NULL;
    ULONG svSPStringSize = 0;
    ULONG svSPStringUsed = 0;
    ULONG svSPNumber = 0;
    PSTR svBuildString = NULL;
    ULONG svBuildStringSize = 0;
    ULONG svBuildStringUsed = 0;
   
    if ((status = g_dbgSysObjects->GetCurrentProcessSystemId(&procID)) !=S_OK )
        printf("Error in GetCurrentProcessSystemId: 0x%x\n", status);

    if (( status = g_dbgSysObjects->GetNumberThreads(&threadCount)) != S_OK )
        printf("Error in GetNumberThreads: 0x%x\n", status);

    g_thCount = threadCount;

    pThreads = (ULONG*)malloc(sizeof(ULONG) * threadCount);
    pSysThreads = (ULONG*)malloc(sizeof(ULONG) * size_t(threadCount));

    if ( (status = g_dbgSysObjects->GetThreadIdsByIndex(0, threadCount, pThreads, pSysThreads)) != S_OK )
        printf("Error in GetThreadIdsByIndex: 0x%x\n", status);


    exeName = (PWSTR)malloc(sizeof(wchar_t) * (size_t)4);
    if (exeName != NULL)
        wcscpy_s(exeName, 4, _T("N/A"));

    if ((status = g_dbgSysObjects->GetCurrentProcessExecutableNameWide(NULL, exeNameBufLen, &exeNameLen)) == S_OK)
    {
        exeName = (PWSTR)malloc(sizeof(wchar_t) * (static_cast<unsigned long long>(exeNameLen) + 1));
        exeNameBufLen = exeNameLen + 1;
        if ((status = g_dbgSysObjects->GetCurrentProcessExecutableNameWide(exeName, exeNameBufLen, &exeNameLen)) != S_OK)
        {            
        }                    
    }
        
    if ((status=g_dbgSysObjects->GetCurrentProcessUpTime(&procUpTime)) != S_OK )
        printf("Error in GetCurrentProcessUpTime: 0x%x\n", status);

    if ((status = g_dbgControl->GetCurrentSystemUpTime(&sysUpTime) ) != S_OK)
        printf("Error in GetCurrentProcessUpTime: 0x%x\n", status);

    if ((status = g_dbgControl->GetActualProcessorType(&cpuType)) != S_OK)
        printf("Error in GetCurrentProcessUpTime: 0x%x\n", status);
    
    cpuLongName = (PWSTR)malloc(sizeof(wchar_t) * (size_t)4);
    if (cpuLongName != NULL)
        wcscpy_s(cpuLongName, 4, _T("N/A"));

    cpuShortName = (PWSTR)malloc(sizeof(wchar_t) * (size_t)4);
    if (cpuShortName != NULL)
        wcscpy_s(cpuShortName, 4, _T("N/A"));

    if ((status = g_dbgControl->GetProcessorTypeNamesWide(cpuType, NULL, cpuLongNameBufLen, &cpuLongNameLen, NULL, cpuShortNameBufLen, &cpuShortNameLen)) == S_OK)
    {
    cpuLongName = (PWSTR)malloc(sizeof(wchar_t) * (static_cast<unsigned long long>(cpuLongNameLen) + 1));
    cpuShortName = (PWSTR)malloc(sizeof(wchar_t) * (static_cast<unsigned long long>(cpuShortNameLen) + 1));

    cpuLongNameBufLen = cpuLongNameLen + 1;
    cpuShortNameBufLen = cpuShortNameLen + 1;

    if ((status = g_dbgControl->GetProcessorTypeNamesWide(cpuType, cpuLongName, cpuLongNameBufLen, &cpuLongNameLen, cpuShortName, cpuShortNameBufLen, &cpuShortNameLen)) == S_OK)
    {
    }

    }

    if ((status = g_dbgControl->GetCurrentTimeDate(&dmpTime)) != S_OK)
        printf("Error in GetCurrentTimeDate: 0x%x\n", status);

    if ((status = g_dbgControl->GetDebuggeeType2(DEBUG_EXEC_FLAGS_NONBLOCK, &dbgeClass, &dbgeQualifier)) != S_OK)
        printf("Error in GetDebuggeeType2: 0x%x\n", status);

    if ((status = g_dbgControl->GetDumpFormatFlags(&dmpFormat)) != S_OK)
        printf("Error in GetDumpFormatFlags: 0x%x\n", status);

    pdbgTypeName = GetDebugTypeName(dmpFormat);


    // TO DO: Check the GetLastStoredEvent API
    if ((status = g_dbgControl->GetLastEventInformation(&gleType, &gleProcId, &gleThreadId, gleExtraInfo, gleExtraInfoSize, &gleExtraInfoUsed, gleDesc, gleDescSize, &gleDescUsed)) == S_OK)
    {
        //printf("gleType: %#X, gleProcId: %#X, gleThreadId: %#X, gleExtraInfoUsed: %u, gleDescUsed: %u\n", gleType, gleProcId, gleThreadId, gleExtraInfoUsed, gleDescUsed);
        //DEBUG_REQUEST_TARGET_EXCEPTION_CONTEXT
        if (gleExtraInfoUsed > 0)
        {
            gleExtraInfo = (LPVOID)malloc(sizeof(LPVOID) * gleExtraInfoUsed);
            gleExtraInfoSize = gleExtraInfoUsed;
        }

        if (gleDescUsed > 0)
        {
            gleDesc = (PSTR)malloc(sizeof(char) * gleDescUsed);
            gleDescSize = gleDescUsed;
        }

        if ((status = g_dbgControl->GetLastEventInformation(&gleType, &gleProcId, &gleThreadId, gleExtraInfo, gleExtraInfoSize, &gleExtraInfoUsed, gleDesc, gleDescSize, &gleDescUsed)) != S_OK)
            printf("Error in GetLastEventInformation: 0x%x\n", status);

        if (gleType == DEBUG_EVENT_EXCEPTION)
        {
            if (gleExtraInfo != NULL)
                expCode = *(ULONG*)gleExtraInfo;

            printf("The last event code was %#X\n", expCode);

            // https://stackoverflow.com/questions/39230167/retrieving-the-stack-trace-from-the-stored-exception-context-in-a-minidump-simi
            /*status = g_dbgSymbols->SetScopeFromStoredEvent();
            DumpStack();
            status = g_dbgSymbols->SetScope(NULL, NULL, NULL, 0);*/
        }
    }

    if ((status = g_dbgControl->GetNumberProcessors(&cpuCount)) != S_OK)
        printf("Error in GetNumberProcessors: 0x%x\n", status);

    /*ULONG count = 10;
    PULONG procList = NULL;

    procList = (PULONG)malloc(sizeof(ULONG) * count);

    status = g_dbgControl->GetPossibleExecutingProcessorTypes(0, count, procList);*/

    if ((status = g_dbgControl->GetSystemVersionValues(&platId, &win32Major, &win32Minor, &kdMajor, &kdMinor)) != S_OK)
        printf("Error in GetNumberProcessors: 0x%x\n", status);

    PWSTR wszVersion = NULL;
    ULONG verBufSize = 0;
    ULONG verStringSize = 0;

    if ((status = g_dbgControl->GetSystemVersionStringWide(DEBUG_SYSVERSTR_BUILD, wszVersion, verBufSize, &verStringSize)) == S_OK)
    {
        wszVersion = (PWSTR)malloc(sizeof(wchar_t) * ( (static_cast<unsigned long long>(verStringSize) + 1) ));
        verBufSize = verStringSize;

        if ((status = g_dbgControl->GetSystemVersionStringWide(DEBUG_SYSVERSTR_BUILD, wszVersion, verBufSize, &verStringSize)) != S_OK)
        {
            if (wszVersion != NULL)
                wcscpy_s(wszVersion, verBufSize, _T("N/A"));
            printf("Error in GetSystemVersionStringWide: 0x%x\n", status);
        }
    }

    if ( (status = g_dbgControl->GetSystemVersion(&svPlatId, &svMajor, &svMinor, svSPString, svSPStringSize, &svSPStringUsed, &svSPNumber, svBuildString, svBuildStringSize, &svBuildStringUsed)) == S_OK)
    {
        svSPString = (PSTR)malloc(sizeof(char) * ((static_cast<unsigned long long>(svSPStringUsed) + 1)) );
        svBuildString = (PSTR)malloc(sizeof(char) * ((static_cast<unsigned long long>(svBuildStringUsed) + 1)) );
        svSPStringSize = svSPStringUsed;
        svBuildStringSize = svBuildStringUsed;

        if ((status = g_dbgControl->GetSystemVersion(&svPlatId, &svMajor, &svMinor, svSPString, svSPStringSize, &svSPStringUsed, &svSPNumber, svBuildString, svBuildStringSize, &svBuildStringUsed)) != S_OK)
        {
            free(svSPString);
            free(svBuildString);
            svSPString = (PSTR)malloc(sizeof(char) * 40);
            strcpy_s(svSPString, 40, "Service Pack N/A");
            svBuildString = (PSTR)malloc(sizeof(char) * 40);
            strcpy_s(svBuildString, 40, "Build N/A");
        }
    }
    
    /*
    * The output was:
    * Server Name: Full memory user mini dump: F:\TempData\AKBANK\BIZTALK_PTO\normal\YeniOdeme08112020\BTSNTSvc64.exe_201108_010030.dmp
    * hence give it up for now
    */
    /*PSTR srvName = NULL;
    ULONG srvBufSize = 0;
    ULONG srvNameSize = 0;

    if ((status = g_dbgSysObjects->GetCurrentSystemServerName(srvName, srvBufSize, &srvNameSize)) == S_OK)
    {
        srvName = (PSTR)malloc(sizeof(char) * ( static_cast<unsigned long long>(srvNameSize) + 1 ) );
        srvBufSize = srvNameSize;
        if ((status = g_dbgSysObjects->GetCurrentSystemServerName(srvName, srvBufSize, &srvNameSize)) != S_OK)
        {
            free(srvName);
            srvName = (PSTR)malloc(sizeof(char) * 5);
            strcpy_s(srvName, 5, "N/A");
        }
    }*/

    // No Joy !!!
    //ULONG64 sysServer;
    //status = g_dbgSysObjects->GetCurrentSystemServer(&sysServer);

    ULONG64 pebOffset = 0;
    PVOID pebBuffer = NULL;
    ULONG pebBufferSize = 0;
    ULONG pebBytesRead = 0;
    _PEB* pPEB = NULL;
    PVOID envPtr = NULL;
    _RTL_USER_PROCESS_PARAMETERS *pprocParam = NULL;
    pebBufferSize =4000;
    pebBuffer = (PVOID)malloc(pebBufferSize);

    ULONG envBufLen = MAX_ENV_BUFFER_LENGTH;
    ULONG envBufRead = 0;
    char envBuf[MAX_ENV_BUFFER_LENGTH];
    wchar_t *envItemBuf=NULL;
    
    ULONG64 Offset = 0;
    ULONG   MaxBytes;
    ULONG   CodePage;
    PWSTR   Buffer;
    ULONG   BufferSize;
    ULONG  StringBytes;
    int retry = 1;

    // RTL_USER_PROCESS_PARAMETERS 

    ULONG cmdLnOff = 0;
    wchar_t *wszCmdLn = NULL;
    ULONG cmdLnLen = 1000;
    ULONG cmdLnRd = 0;
    ULONG cmdLnMax = 1000;
    
    RTL_USER_PROCESS_PARAMETERS* usrProcParam = NULL;
    ULONG usrProcParamOff = 0;
    ULONG usrProcParamLen = 0;
    ULONG usrProcParamRd = 0;

    // Getting the environment variables from the PEB address
    // TO DO: This could should be revisited and developed better
    if ((status = g_dbgSysObjects->GetCurrentProcessPeb(&pebOffset)) == S_OK)
    {
        status = g_dbgDataSpace->ReadVirtual(pebOffset, pebBuffer, pebBufferSize, &pebBytesRead);
        if ( status == S_OK && pebBytesRead > 0 )
        {
            // GET THE PEB
            pPEB = (_PEB*)pebBuffer;
            pebOffset = (ULONG)(pPEB->ProcessParameters);
            pebOffset = pebOffset + 0x80;

            // GET THE RTL_USER_PROCESS_PARAMETERS 
            usrProcParamOff = (ULONG)(pPEB->ProcessParameters);
            usrProcParamLen = sizeof(RTL_USER_PROCESS_PARAMETERS);
            usrProcParam = (RTL_USER_PROCESS_PARAMETERS*)malloc(usrProcParamLen);
            status = g_dbgDataSpace->ReadVirtual(usrProcParamOff, usrProcParam, usrProcParamLen, &usrProcParamRd);

            // GET THE COMMAND LINE            
            cmdLnOff = (ULONG64)(usrProcParam->CommandLine.Buffer);            
            cmdLnMax = cmdLnRd = cmdLnLen = usrProcParam->CommandLine.MaximumLength;
            wszCmdLn = (wchar_t*)malloc(sizeof(wchar_t) * cmdLnLen);
            if ( (status = g_dbgDataSpace->ReadUnicodeStringVirtualWide(cmdLnOff, cmdLnLen, wszCmdLn, cmdLnMax, &cmdLnRd) ) == S_OK)
            {
                g_wszCmdLine = wszCmdLn;
            }

            // Free the previous allocation
            free(pebBuffer);
            pebBuffer = (PVOID)malloc(sizeof(PVOID));
            pebBufferSize = sizeof(PVOID);
            pebBytesRead = pebBufferSize;
            status = g_dbgDataSpace->ReadVirtual(pebOffset, pebBuffer, pebBufferSize, &pebBytesRead);
            envItemBuf = (wchar_t*)malloc(sizeof(wchar_t) * 1000);
            int cmpRes = 0;

                        
            if (status == S_OK && pebBytesRead > 0)
            {

                Offset = *((PULONG64)pebBuffer);
                do
                {
                    StringBytes = 0;                    
                    status = g_dbgDataSpace->ReadUnicodeStringVirtualWide(Offset, 1000*retry, envItemBuf, 1000*retry, &StringBytes);
                   cmpRes = wcscmp(envItemBuf, _T(""));

                    if (status == S_OK && StringBytes > 0 && cmpRes )
                    {
                        if (retry > 1)
                        {
                            wprintf(_T("%s\n"), envItemBuf);
                            free(envItemBuf);                            
                            envItemBuf = (wchar_t*)malloc(sizeof(wchar_t) * 1000);
                            wmemset(envItemBuf, 0, 1000);
                        }
                        else
                        {                            
                            wprintf(_T("%s\n"), envItemBuf);
                            wmemset(envItemBuf, 0, 1000);                                                        
                        }
                        retry = 1;
                        Offset += StringBytes;
                    }
                    else 
                        if (cmpRes)
                        {                        
                            free(envItemBuf);
                            // TO DO: We need to implement a better solution. If the null terminator is not within 2000 characters then it will loop forever!!!
                            retry++;
                            envItemBuf = (wchar_t*)malloc(sizeof(wchar_t) * 1000*retry);
                            wmemset(envItemBuf, 0, 1000 * retry);                            
                        }
                } while (cmpRes); //(StringBytes > 0);
            }
        }

    }


    PDEBUG_THREAD_BASIC_INFORMATION  thInfo = NULL;
    ULONG thBufSize = 0;
    ULONG thInfoSize = 0;
        
   

    // TO DO: Convert into better data structures
    wprintf(_T("The dump type is %s\n"), GetDebuggeeTypeName(dbgeClass, dbgeQualifier));
    printf("ProcessID is 0x:%x\n", procID);
    wprintf(_T("Process Name is: %s\n"), exeName);
    printf("Total Threads: %d\n", threadCount);
    printf("The system is up for  %u seconds\n", static_cast<unsigned long>(sysUpTime));
    printf("The process is up for %u seconds\n", static_cast<unsigned long>(procUpTime));
    printf("The dump time is %u seconds\n", static_cast<unsigned long>(dmpTime));
    wprintf(_T("The CPU short name: %s, long name is: %s\n"), cpuShortName, cpuLongName);
    wprintf(_T("Number of processors: %d\n"), cpuCount);
    wprintf(_T("The dump content is %s\n"), pdbgTypeName);
    if (g_wszCmdLine != NULL)
        wprintf(_T("COMMAND LINE: %s\n"), g_wszCmdLine);
    else
        wprintf(_T("COMMAND LINE: %s\n"), _T("N/A"));

    if (pdbgTypeName != NULL)
        free(pdbgTypeName);
    printf("Desc: %s\n", gleDesc);

    // TO DO: Design a better output : https://docs.microsoft.com/en-us/windows/win32/api/winnt/ns-winnt-osversioninfoexa
    wprintf(_T("Major: %u Minor: %u %s Build Number: %u\n"), win32Major, win32Minor, kdMajor == 12 ? _T("Checked Build"):_T("Free Build")  , kdMinor );
    wprintf(_T("System version string: %s\n"), wszVersion);
    printf("Major: %u Minor: %u SP Number: %u SP String: %s Build String: %s\n", svMajor, svMinor, svSPNumber, svSPString, svBuildString);
    //printf("Server Name: %s\n", srvName);


    // Temporarly comment out so that the output is easier to read. 
    // Merged the output into the next loop
    //for (ULONG i = 0; i < threadCount; i++)
    //    printf("[%4u:0x%-5X]\n", pThreads[i], pSysThreads[i] );

    for (int i = 0; i != threadCount; i++)
    {

        if ((status = g_dbgAdvanced->GetSystemObjectInformation(DEBUG_SYSOBJINFO_THREAD_BASIC_INFORMATION, 0, i, thInfo, thBufSize, &thInfoSize)) == S_OK)
        {
            thInfo = (PDEBUG_THREAD_BASIC_INFORMATION)malloc(thInfoSize);
            thBufSize = thInfoSize;

            if ((status = g_dbgAdvanced->GetSystemObjectInformation(DEBUG_SYSOBJINFO_THREAD_BASIC_INFORMATION, 0, i, thInfo, thBufSize, &thInfoSize)) == S_OK)
            {
                if (thInfo->Valid & DEBUG_TBINFO_TIMES)
                {
                    printf("[%4u:0x%-5X] CreateTime: %u ExitTime: %u UserTime: %u KernelTime: %u\n", pThreads[i], pSysThreads[i], thInfo->CreateTime, thInfo->ExitTime, thInfo->UserTime, thInfo->KernelTime);
                }
            }

            free(thInfo);
            thInfo = NULL;
            thBufSize = 0;
            thInfoSize = 0;
        }
    }
}

wchar_t* GetDebuggeeTypeName(ULONG Class, ULONG Qualifier)
{
    int i = 0, j = 0;

    if (Class == DEBUG_CLASS_UNINITIALIZED)
        return DebuggeeTypeName[0];

    if (Class == DEBUG_CLASS_KERNEL)
    {
        switch (Qualifier)
        {
        case DEBUG_KERNEL_CONNECTION:
            return DebuggeeTypeName[1];
            
        case DEBUG_KERNEL_LOCAL:
            return DebuggeeTypeName[2];            

        case DEBUG_KERNEL_EXDI_DRIVER:
            return DebuggeeTypeName[3];

        case DEBUG_KERNEL_SMALL_DUMP:
            return DebuggeeTypeName[4];

        case DEBUG_KERNEL_DUMP:
            return DebuggeeTypeName[5];

        case DEBUG_KERNEL_FULL_DUMP:
            return DebuggeeTypeName[6];
        
        default:
            return DebuggeeTypeName[11];
        }
    }

    if (Class == DEBUG_CLASS_USER_WINDOWS)
    {
        switch (Qualifier)
        {
        case DEBUG_USER_WINDOWS_PROCESS:
            return DebuggeeTypeName[7];

        case DEBUG_USER_WINDOWS_PROCESS_SERVER:
            return DebuggeeTypeName[8];

        case DEBUG_USER_WINDOWS_SMALL_DUMP:
            return DebuggeeTypeName[9];

        case DEBUG_USER_WINDOWS_DUMP:
            return DebuggeeTypeName[10];

        default:
            return DebuggeeTypeName[12];
        }
    }

    return DebuggeeTypeName[13];

}

// TO DO: Expand with the undocumented definitions from dbgeng.h
wchar_t* GetDebugTypeName(ULONG dbgType)
{
    wchar_t* wszMsg = NULL;
    
    wszMsg = (wchar_t*)malloc(sizeof(wchar_t) * 300);    
    memset(wszMsg, 0, sizeof(wchar_t) * 300);

    if ( dbgType & DEBUG_FORMAT_WRITE_CAB)
        wcscat_s(wszMsg, DEBUG_FORMAT_MESSAGE_LENGTH, g_dbgTypeName[1]);
    
    if (dbgType & DEBUG_FORMAT_CAB_SECONDARY_FILES)
        wcscat_s(wszMsg, DEBUG_FORMAT_MESSAGE_LENGTH, g_dbgTypeName[2]);

    if (dbgType & DEBUG_FORMAT_NO_OVERWRITE)
        wcscat_s(wszMsg, DEBUG_FORMAT_MESSAGE_LENGTH, g_dbgTypeName[3]);

    if (dbgType & DEBUG_FORMAT_USER_SMALL_FULL_MEMORY)
        wcscat_s(wszMsg, DEBUG_FORMAT_MESSAGE_LENGTH, g_dbgTypeName[4]);

    if (dbgType & DEBUG_FORMAT_USER_SMALL_HANDLE_DATA)
        wcscat_s(wszMsg, DEBUG_FORMAT_MESSAGE_LENGTH, g_dbgTypeName[5]);

    if (dbgType & DEBUG_FORMAT_USER_SMALL_UNLOADED_MODULES)
        wcscat_s(wszMsg, DEBUG_FORMAT_MESSAGE_LENGTH, g_dbgTypeName[6]);

    if (dbgType & DEBUG_FORMAT_USER_SMALL_INDIRECT_MEMORY)
        wcscat_s(wszMsg, DEBUG_FORMAT_MESSAGE_LENGTH, g_dbgTypeName[7]);

    if (dbgType & DEBUG_FORMAT_USER_SMALL_DATA_SEGMENTS)
        wcscat_s(wszMsg, DEBUG_FORMAT_MESSAGE_LENGTH, g_dbgTypeName[8]);

    if (dbgType & DEBUG_FORMAT_USER_SMALL_FILTER_MEMORY)
        wcscat_s(wszMsg, DEBUG_FORMAT_MESSAGE_LENGTH, g_dbgTypeName[9]);

    if (dbgType & DEBUG_FORMAT_USER_SMALL_FILTER_PATHS)
        wcscat_s(wszMsg, DEBUG_FORMAT_MESSAGE_LENGTH, g_dbgTypeName[10]);

    if (dbgType & DEBUG_FORMAT_USER_SMALL_PROCESS_THREAD_DATA)
        wcscat_s(wszMsg, DEBUG_FORMAT_MESSAGE_LENGTH, g_dbgTypeName[11]);

    if (dbgType & DEBUG_FORMAT_USER_SMALL_PRIVATE_READ_WRITE_MEMORY)
        wcscat_s(wszMsg, DEBUG_FORMAT_MESSAGE_LENGTH, g_dbgTypeName[12]);

    if (dbgType & DEBUG_FORMAT_USER_SMALL_NO_OPTIONAL_DATA)
        wcscat_s(wszMsg, DEBUG_FORMAT_MESSAGE_LENGTH, g_dbgTypeName[13]);

    if (dbgType & DEBUG_FORMAT_USER_SMALL_FULL_MEMORY_INFO)
        wcscat_s(wszMsg, DEBUG_FORMAT_MESSAGE_LENGTH, g_dbgTypeName[14]);

    if (dbgType & DEBUG_FORMAT_USER_SMALL_THREAD_INFO)
        wcscat_s(wszMsg, DEBUG_FORMAT_MESSAGE_LENGTH, g_dbgTypeName[15]);

    if (dbgType & DEBUG_FORMAT_USER_SMALL_CODE_SEGMENTS)
        wcscat_s(wszMsg, DEBUG_FORMAT_MESSAGE_LENGTH, g_dbgTypeName[16]);

    if (!wcscmp(wszMsg, _T("")))
        wcscpy_s(wszMsg, DEBUG_FORMAT_MESSAGE_LENGTH, g_dbgTypeName[0]);

    return wszMsg;
}

void DumpStack(void)
{
    HRESULT status;
    PDEBUG_STACK_FRAME frames = NULL;
    int count = 50;

    //printf("\nFirst %d frames of the call stack:\n", Count);

       ULONG Filled;

        frames = new DEBUG_STACK_FRAME[count];
        if (frames == NULL)
        {            
            _tprintf(_T("Unable to allocate stack frames\n"));
        }

        if ((status = g_dbgControl->GetStackTrace(0,0,0, frames, count, &Filled)) != S_OK)
        {
            _tprintf(_T("Error happened 0x:%x\n"), status);            
        }

        count = Filled;
    

    // Print the call stack.
    if ((status = g_dbgControl->OutputStackTrace(DEBUG_OUTCTL_ALL_CLIENTS, frames, count, DEBUG_STACK_FUNCTION_INFO	| DEBUG_STACK_SOURCE_LINE | DEBUG_STACK_FRAME_ADDRESSES | DEBUG_STACK_COLUMN_NAMES | DEBUG_STACK_FRAME_NUMBERS)) != S_OK)
    {
        
        _tprintf(_T("Error happened 0x:%x\n"), status);
    }

    delete[] frames;
}

// TO DO: Need to loop until the returned filled is ZERO !!!
void DumpAllStacks()
{
    HRESULT status = S_OK;
    PDEBUG_STACK_FRAME frames = NULL;
    int count = 50;
    ULONG Filled;

    printf("ALL CALL STACKS\n");
    printf("===============\n");

    for (int i = 0; i != g_thCount; i++)
    {
        status = g_dbgSysObjects->SetCurrentThreadId(i);
        count = 50;
        Filled = 0;
        printf("ThreadId: %4d\n", i);
        frames = new DEBUG_STACK_FRAME[count];
        if (frames == NULL)
        {
            _tprintf(_T("Unable to allocate stack frames\n"));
        }

        if ((status = g_dbgControl->GetStackTrace(0, 0, 0, frames, count, &Filled)) != S_OK)
        {
            _tprintf(_T("Error happened 0x:%x\n"), status);
        }

        count = Filled;


        // Print the call stack.
        if ((status = g_dbgControl->OutputStackTrace(DEBUG_OUTCTL_ALL_CLIENTS, frames, count, DEBUG_STACK_SOURCE_LINE | DEBUG_STACK_FRAME_ADDRESSES | DEBUG_STACK_COLUMN_NAMES | DEBUG_STACK_FRAME_NUMBERS)) != S_OK)
        {

            _tprintf(_T("Error happened 0x:%x\n"), status);
        }

        delete[] frames;

    }
}

int wmain(int argc, wchar_t* argv[])
{
    size_t dmpNameLen = 0;
    errno_t err = -1;
    Initialize();
    HRESULT status = S_OK;


    if (argc > 1)
    {
        //dmpNameLen = strlen(argv[1]);
        dmpNameLen = wcslen(argv[1]);
        if (dmpNameLen != 0)
        {
            dmpNameLen++;
            //g_dumpFile = (PSTR)malloc(sizeof(wchar_t) * dmpNameLen);
            g_dumpFile = (wchar_t*) malloc(sizeof(wchar_t) * (dmpNameLen+1));
            if (g_dumpFile != NULL)
            {
                //err = strcpy_s(g_dumpFile, dmpNameLen, argv[1]);
                err = wcscpy_s(g_dumpFile, dmpNameLen, argv[1]);
            }
        }
    }

    if (!err)
    {
        MakeTheSymbolsWork();
           

        //if ((status = g_dbgClient->OpenDumpFileWide(L"F:\\TempData\\AKBANK\\BIZTALK_PTO\\normal\\YeniOdeme08112020\\BTSNTSvc64.exe_201108_010030.dmp", 0)) != S_OK)
        if ((status = g_dbgClient->OpenDumpFileWide(g_dumpFile, 0)) != S_OK)
        //if ((status = g_dbgClient->OpenDumpFile(g_dumpFile)) != S_OK)
            _tprintf(_T("Error happened 0x:%x\n"), status);

        if ((status = g_dbgControl->WaitForEvent(DEBUG_WAIT_DEFAULT, INFINITE)) != S_OK)
            _tprintf(_T("Error happened 0x:%x\n"), status);


      /*  if ((status = g_dbgSymbols->SetScopeFromStoredEvent()) != S_OK)
            _tprintf(_T("Error happened 0x:%x\n"), status);*/
                
        if ((status = g_dbgClient->SetOutputCallbacks(&g_OutputCb)) != S_OK)
            _tprintf(_T("Error happened 0x:%x\n"), status);

        GetInformationFromDumpFile();

        //status = g_dbgSymbols->SetScopeFromStoredEvent();

        // DumpStack();
        
        DumpAllStacks();

        
    }

    return 0;
}


