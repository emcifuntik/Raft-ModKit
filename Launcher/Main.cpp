#define WIN32_LEAN_AND_MEAN
#include "ProcessHelper.h"
#include "Utility.h"
#include <Shellapi.h>
#include "../shared/CLog.h"

using namespace System;
using namespace System::Reflection;
using namespace Microsoft::Win32;

System::String^ GetAssemblyName(System::String^ dll)
{
	System::String^ assemblyName = "";

	try
	{
		assemblyName = AssemblyName::GetAssemblyName(dll)->Name;
		assemblyName = assemblyName->Replace("-", "_");
	}
	catch (...)
	{

	}

	return assemblyName;
}

[System::STAThreadAttribute]
int WinMain(HINSTANCE, HINSTANCE, LPSTR, int)
{
	my_ostream::LogFile("launcher.log");
	ProcessHelper::ProcessInfo * raftProcess = nullptr;

	raftProcess = ProcessHelper::GetRaftProcess();
	if (raftProcess) {
		MessageBoxA(NULL, "Raft.exe already run. It must be started with ModKit", "[ModKit] startup error", MB_OK);
		delete raftProcess;
		return 0;
	}

	ShellExecute(NULL, NULL, L"steam://run/648800", NULL, NULL, SW_SHOW);
	
	do {
		raftProcess = ProcessHelper::GetRaftProcess();
		if (!raftProcess)
			Sleep(10);
	} while (!raftProcess);

	if (!raftProcess->SameArchitecture()) {
		log_error << "Can't inject into Raft (Another architecture)." << std::endl;
		delete raftProcess;
		return 0;
	}

	while (!raftProcess->HasModule("mono.dll")) {
		Sleep(10);
		raftProcess->UpdateModules();
	}

	if (raftProcess->HasModule("mono.dll")) {
		std::string modKitDll = Utility::CurrentPath() + "\\ModKit.dll";
		System::String^ dllPathClr = gcnew System::String(modKitDll.c_str());
		if (!ProcessHelper::InjectDLL(modKitDll, Utility::ToString(GetAssemblyName(dllPathClr)), raftProcess)) {
			MessageBoxA(NULL, "Can't inject into Raft.", "[ModKit] Injection error", MB_OK);
		}
		else {
			log_info << "Successfully injected into Raft" << std::endl;
		}
	}
	else {
		log_error << "Raft process has no Mono.dll loaded" << std::endl;
	}
	delete raftProcess;
	return 0;
}
