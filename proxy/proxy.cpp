// proxy.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include <tchar.h>

std::queue<std::pair<std::string, std::string>> g_AssemblyQueue;

#define BUF_SIZE 256
TCHAR szName[] = TEXT("Raft_ModKit_Path");
TCHAR szMsg[] = TEXT("Message from first process.");

void Entrypoint(HMODULE hModule)
{
	HANDLE hMapFile;
	LPCTSTR pBuf;

	hMapFile = CreateFileMapping(
		INVALID_HANDLE_VALUE,    // use paging file
		NULL,                    // default security
		PAGE_READWRITE,          // read/write access
		0,                       // maximum object size (high-order DWORD)
		BUF_SIZE,                // maximum object size (low-order DWORD)
		szName);                 // name of mapping object

	if (hMapFile == NULL)
	{
		return;
	}
	pBuf = (LPTSTR)MapViewOfFile(hMapFile,   // handle to map object
		FILE_MAP_ALL_ACCESS, // read/write permission
		0,
		0,
		BUF_SIZE);

	if (pBuf == NULL)
	{
		CloseHandle(hMapFile);
		return;
	}


	CopyMemory((PVOID)pBuf, szMsg, (_tcslen(szMsg) * sizeof(TCHAR)));

	LoadHook();


	//UnmapViewOfFile(pBuf);
	//CloseHandle(hMapFile);
	// RemoveHook();
}

void LoadAssembly(assembly_params* assembly)
{
	std::string dllPath = assembly->szPath;
	std::string dllNamespace = assembly->szNamespace;

	std::pair<std::string, std::string> dllInfo(dllPath, dllNamespace);

	g_AssemblyQueue.push(dllInfo);

}
