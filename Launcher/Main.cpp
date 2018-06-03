#define WIN32_LEAN_AND_MEAN
#include "ProcessHelper.h"
#include "Utility.h"
#include <Shellapi.h>

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
	ProcessHelper::ProcessInfo * raftProcess = nullptr;

	raftProcess = ProcessHelper::GetRaftProcess();
	if (raftProcess) { //Already run
		MessageBoxA(NULL, "Raft.exe already run. It must be started with ModKit", "ModKit startup error", MB_OK);
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
		MessageBoxA(NULL, "Can't inject into Raft (Another architecture).", "[ModKit] Injection error", MB_OK);
		delete raftProcess;
		return 0;
	}

	if (raftProcess->HasModule("mono.dll")) {
		RegistryKey^ rk;
		rk = Registry::CurrentUser->OpenSubKey("Software", true);
		if (!rk)
		{
			Console::WriteLine("Failed to open CurrentUser/Software key");
			return 1;
		}

		RegistryKey^ nk = rk->CreateSubKey("RaftModKit");
		if (!nk)
		{
			Console::WriteLine("Failed to create 'RaftModKit'");
			return 1;
		}

		String^ newValue = gcnew System::String(Utility::CurrentPath().c_str());
		try
		{
			nk->SetValue("ModKitFolder", newValue);
		}
		catch (Exception^)
		{
			Console::WriteLine("Failed to set new values in 'ModKitFolder'");
			return 1;
		}

		std::string modKitDll = Utility::CurrentPath() + "\\ModKit.dll";
		System::String^ dllPathClr = gcnew System::String(modKitDll.c_str());
		if (!ProcessHelper::InjectDLL(modKitDll, Utility::ToString(GetAssemblyName(dllPathClr)), raftProcess)) {
			MessageBoxA(NULL, "Can't inject into Raft.", "[ModKit] Injection error", MB_OK);
		}
	}
	delete raftProcess;
	return 0;
}
