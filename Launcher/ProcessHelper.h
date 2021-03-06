#pragma once

#include <vector>
#include <string>
#include <fstream>
#include <Windows.h>
#include <Psapi.h>

namespace ProcessHelper {
	class ProcessInfo {
	public:
		std::string ProcessName;
		int ProcessId;
		std::vector<std::string> Modules;

		// Checks if process has module loaded
		bool HasModule(const std::string&  moduleName);

		void* GetModuleBase(const std::string&  moduleName);

		// Refreshes modules list
		void UpdateModules();

		// Returns full process file path
		std::string GetExePath();

		// true if same arch as injector
		bool SameArchitecture();

	private:
		short GetArchitecture();
	};

	bool IsProxyLoaded(ProcessInfo *pi);

	/// Proxy module base
	void* LoadProxy(const std::string&, ProcessInfo *pi);
	ProcessInfo * GetRaftProcess();

	bool InjectDLL(const std::string&  dllPath, const std::string&  dllNamespace, ProcessInfo *pi);
}

