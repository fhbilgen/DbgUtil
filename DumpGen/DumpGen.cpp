// DumpGen.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"



int CreateDumpFile(DWORD dwProcId, HANDLE hProc )
{
			// create the file
			HANDLE hFile = ::CreateFile(L"c:\\delme\\fhb.dmp", GENERIC_WRITE, FILE_SHARE_WRITE, NULL, CREATE_ALWAYS,
				FILE_ATTRIBUTE_NORMAL, NULL);

			if (hFile != INVALID_HANDLE_VALUE)
			{
				_MINIDUMP_EXCEPTION_INFORMATION ExInfo;

				ExInfo.ThreadId = ::GetCurrentThreadId();
				ExInfo.ExceptionPointers = NULL;
				ExInfo.ClientPointers = NULL;

				// write the dump
				BOOL bOK = MiniDumpWriteDump(hProc, dwProcId, hFile, 
					(MINIDUMP_TYPE)(MiniDumpWithFullMemory | MiniDumpWithHandleData | MiniDumpWithUnloadedModules | MiniDumpWithFullMemoryInfo | MiniDumpWithThreadInfo),
					&ExInfo, NULL, NULL);
				
				if (bOK)
				{
					printf("Dump generated successfully\n");
				}
				else
				{
					printf("Failed to save dump file 0x%x\n", GetLastError());					
				}
				::CloseHandle(hFile);
			}
			return 0;
}

int _tmain(int argc, _TCHAR* argv[])
{
	DWORD dwProcId=0;
	HANDLE hProc=NULL;

	while (dwProcId != -1)
	{
		printf("Enter the process id (-1 to exit)\n");
		scanf_s("%d", &dwProcId, 10);

		if (dwProcId == -1)
			break;

		hProc = OpenProcess(READ_CONTROL | PROCESS_ALL_ACCESS, FALSE, dwProcId);

		if (NULL != hProc)
		{
			CreateDumpFile(dwProcId, hProc);
			CloseHandle(hProc);
		}
		else
			printf("Could not get the process handle\n");
				
	}

	return 0;
}

